using System;
using System.Runtime.InteropServices;
using TMPro;

public class CurrentLapInfo : TelemetryListener, ILapListener
{
    //---------TELEMETRY SYSTEM DECLARATIONS---------
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_currentLapTime(byte id);
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_sector1(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_sector2(byte carId);
    
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_sector(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_lastTimeLap(byte id);
    
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_bestOverallSector1TimeInMS(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_bestOverallSector2TimeInMS(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_bestOverallSector3TimeInMS(byte carId);

    //---------VARIABLES---------
    public TextMeshProUGUI currentLapText;
    public TextMeshProUGUI sector1Text;
    public TextMeshProUGUI sector2Text;
    public TextMeshProUGUI sector3Text;

    public LapManager lapManager;


    private int _lastS1Time;
    private int _lastS2Time;
    
    private byte _currentPlayerCarId = 0;
    
    
    //---------UNITY FUNCTIONS---------
    void Start()
    {
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
        EventManager.instance.AddListener(this);
        lapManager.AddLapListener(this);

        ResetTimes();
    }
    
    
    void Update()
    {
        
        int currentLapMili = (int)(F1TS_currentLapTime(_currentPlayerCarId) * 1000);
        UpdateCurrentLapText(currentLapMili);

        int currentSector = F1TS_sector(_currentPlayerCarId);
        if (currentSector == 0) //s1
        {
            Sector1(currentLapMili);
        }
        else if (currentSector == 1) //s2
        {
            Sector2(currentLapMili);
        }
        else if (currentSector == 2) //s3
        {
            Sector3(currentLapMili);
        }
    }
    
    
    //---------PRIVATE---------
    private void ResetTimes()
    {
        currentLapText.text = "Lap: 00,00.000";
        sector1Text.text = "Sector 1: 00,00.000";
        sector2Text.text = "Sector 2: 00,00.000";
        sector3Text.text = "Sector 3: 00,00.000";
        
        currentLapText.color = Manager.instance.colorPalette.NormalTime;
        sector1Text.color = Manager.instance.colorPalette.NormalTime;
        sector2Text.color = Manager.instance.colorPalette.NormalTime;
        sector3Text.color = Manager.instance.colorPalette.NormalTime;
    }
    

    private void UpdateCurrentLapText(int currentLapMili)
    {
        //Lap time
        if (currentLapMili != 0.0)
        {
            int m = currentLapMili / 60000;
            int secMill = (currentLapMili - m * 60000);
            int s = secMill / 1000;
            int ms = secMill % 1000;
            currentLapText.text = "Lap: " + m.ToString("00") + "," + s.ToString("00") + "." + ms.ToString("000");
        }
    }

    private void Sector1(int currentLapMili)
    {
        sector1Text.text = "Sector 1: " + FromTimeToStringFormat(currentLapMili);
    }

    private void Sector2(int currentLapMili)
    {
        ushort s1Time = F1TS_sector1(_currentPlayerCarId);
        if (s1Time != _lastS1Time) //updates text and color
        {
            sector1Text.text = "Sector 1: " + FromTimeToStringFormat(s1Time);
            _lastS1Time = s1Time;
            ChangeSectorTextColor(sector1Text, _lastS1Time, lapManager.GetPersonalFastestSector1(),
                lapManager.GetOverallFastestSector1());
        }
        
        int s2Time = currentLapMili - s1Time;
        sector2Text.text = "Sector 2: " + FromTimeToStringFormat(s2Time);
    }
    
    private void Sector3(int currentLapMili)
    {
        ushort s1Time = F1TS_sector1(_currentPlayerCarId);
        sector1Text.text = "Sector 1: " + FromTimeToStringFormat(s1Time);

        ushort s2Time = F1TS_sector2(_currentPlayerCarId);
        if (s2Time <= 0)
            return;

        if (_lastS2Time != s2Time)
        {
            sector2Text.text = "Sector 2: " + FromTimeToStringFormat(s2Time);
            _lastS2Time = s2Time;
            ChangeSectorTextColor(sector2Text, _lastS2Time, lapManager.GetPersonalFastestSector2(),
                lapManager.GetOverallFastestSector2());
        }

        
        int s3Time = currentLapMili - s1Time - s2Time;
        sector3Text.text = "Sector 3: " + FromTimeToStringFormat(s3Time);
        
        
    }

    private void ChangeSectorTextColor(TextMeshProUGUI text, int time, int personalBest, int overallBest)
    {
        if (time <= overallBest)
        {
            text.color = Manager.instance.colorPalette.OverallBestTime;
        }
        else if (time <= personalBest)
            text.color = Manager.instance.colorPalette.PersonalBestTime;
        else
            text.color = Manager.instance.colorPalette.NormalTime;
    }
    
    private String FromTimeToStringFormat(int time)
    {            
        int m = time / 60000;
        int secMill = (time - m * 60000);
        int s = secMill / 1000;
        int ms = secMill % 1000;
        return m.ToString("00") + "," + s.ToString("00") + "." + ms.ToString("000");
    }


    
    
    
    //---------LISTENER FUNCTIONS---------
    public override void OnNewLap(int lap)
    {
        //This needs to be here because we need the sector 1 and sector 2 times 
        int lastLapMili = (int)(F1TS_lastTimeLap(_currentPlayerCarId) * 1000);
        if (lastLapMili <= 0)
            return;
        
        int lastS3 = lastLapMili - _lastS1Time - _lastS2Time;
        lapManager.NewLap(lastLapMili, _lastS1Time, _lastS2Time, lastS3, lap - 1);

        ResetTimes();
    }

    public override void OnPlayerCarIdChanged(byte playerCarId)
    {
        _currentPlayerCarId = playerCarId;
    }

    
    //TODO se deberia llamar en una carrera
    public void OnFastestSector(int time, int sector, bool personal)
    {
        print("Fastest sector");
        if (sector == 0 && time <= _lastS1Time)
        {
            ChangeSectorTextColor(sector1Text, _lastS1Time, lapManager.GetPersonalFastestSector1(),
                lapManager.GetOverallFastestSector1());
        }
        else if (sector == 1 && time <= _lastS2Time)
        {
            ChangeSectorTextColor(sector2Text, _lastS2Time, lapManager.GetPersonalFastestSector2(),
                lapManager.GetOverallFastestSector2());
        }
    }
}