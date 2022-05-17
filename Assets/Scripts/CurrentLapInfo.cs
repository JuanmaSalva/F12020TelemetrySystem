using System;
using System.Runtime.InteropServices;
using TMPro;

public class CurrentLapInfo : TelemetryListener
{
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

    public TextMeshProUGUI currentLapText;
    public TextMeshProUGUI sector1Text;
    public TextMeshProUGUI sector2Text;
    public TextMeshProUGUI sector3Text;


    private int _s1Time;
    private int _s2Time;
    private int _s3Time;

    private int _sessionPersonalBestS1 = Int32.MaxValue;
    private int _sessionPersonalBestS2= Int32.MaxValue;
    private int _sessionPersonalBestS3= Int32.MaxValue;

    private int _lastS1Time;
    private int _lastS2Time;
    
    private byte _currentPlayerCarId = 0;
    
    
    
    //---------UNITY FUNCTIONS--------
    void Start()
    {
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
        EventManager.instance.AddListener(this);

        currentLapText.color = Manager.instance.colorPalette.NormalTime;
        sector1Text.color = Manager.instance.colorPalette.NormalTime;
        sector2Text.color = Manager.instance.colorPalette.NormalTime;
        sector3Text.color = Manager.instance.colorPalette.NormalTime;

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
    
    
    //---------PRIVATE--------
    private void ResetTimes()
    {
        currentLapText.text = "Lap: 00,00.000";
        sector1Text.text = "Sector 1: 00,00.000";
        sector2Text.text = "Sector 2: 00,00.000";
        sector3Text.text = "Sector 3: 00,00.000";
    }
    
    private void Sector1(int currentLapMili)
    {
        sector1Text.text = GetSectorTimeStr(currentLapMili, 1);
    }
    
    private void Sector2(int currentLapMili)
    {
        ushort s1Time = F1TS_sector1(_currentPlayerCarId);
        sector1Text.text = GetSectorTimeStr(s1Time, 1);
        
        _lastS1Time = s1Time;
        _sessionPersonalBestS1 = Math.Min(_lastS1Time, _sessionPersonalBestS1);
        
        //includes times from other sessions if any (ex. time trial)
        if (_lastS1Time <= F1TS_bestOverallSector1TimeInMS(_currentPlayerCarId))
            sector1Text.color = Manager.instance.colorPalette.OverallBestTime;
        //only best time in this session
        else if (_lastS1Time <= _sessionPersonalBestS1)
            sector1Text.color = Manager.instance.colorPalette.PersonalBestTime;
        else
            sector1Text.color = Manager.instance.colorPalette.NormalTime;

        int s2Time = currentLapMili - s1Time;
        sector2Text.text = GetSectorTimeStr(s2Time, 2);
    }


    private void Sector3(int currentLapMili)
    {
        ushort s1Time = F1TS_sector1(_currentPlayerCarId);
        sector1Text.text = GetSectorTimeStr(s1Time, 1);

        ushort s2Time = F1TS_sector2(_currentPlayerCarId);
        sector2Text.text = GetSectorTimeStr(s2Time, 2);
        
        _lastS2Time = s2Time;
        _sessionPersonalBestS2 = Math.Min(_lastS2Time, _sessionPersonalBestS2);
        
        //includes times from other sessions if any (ex. time trial)
        if (_lastS2Time <= F1TS_bestOverallSector2TimeInMS(_currentPlayerCarId))
            sector2Text.color = Manager.instance.colorPalette.OverallBestTime;
        //only best time in this session
        else if (_lastS2Time <= _sessionPersonalBestS2)
            sector2Text.color = Manager.instance.colorPalette.PersonalBestTime;
        else
            sector2Text.color = Manager.instance.colorPalette.NormalTime;

        int s3Time = currentLapMili - s1Time - s2Time;
        sector3Text.text = GetSectorTimeStr(s3Time, 3);
    }

    
    /// <summary>
    /// Converts a sector time into a nice and uniform string to display
    /// </summary>
    /// <param name="time">Sector time</param>
    /// <param name="sector">Sector referred to (0-2)</param>
    /// <returns>String format</returns>
    private string GetSectorTimeStr(int time, int sector)
    {
        int s = time / 1000;
        int ms = time % 1000;
        return "Sector " + sector +": 00," + s.ToString("00") + "." + ms.ToString("000");
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

    
    
    //---------TELEMETRY LISTENERS--------
    public override void OnNewLap(int lap)
    {
        int lastLapMili = (int)(F1TS_lastTimeLap(_currentPlayerCarId) * 1000);
        int lastS3 = lastLapMili - _lastS1Time - _lastS2Time;
        _sessionPersonalBestS3 = Math.Min(lastS3, _sessionPersonalBestS3);
        
        ResetTimes();
    }

    public override void OnPlayerCarIdChanged(byte playerCarId)
    {
        _currentPlayerCarId = playerCarId;
    }

    
    
    //---------GETTERS--------
    public int SessionBestS1()
    {
        return _sessionPersonalBestS1;
    }
    public int SessionBestS2()
    {
        return _sessionPersonalBestS2;
    }
    public int SessionBestS3()
    {
        return _sessionPersonalBestS3;
    }
}
