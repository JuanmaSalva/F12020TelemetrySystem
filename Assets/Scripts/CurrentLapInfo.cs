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

    public TextMeshProUGUI currentLapText;
    public TextMeshProUGUI sector1Text;
    public TextMeshProUGUI sector2Text;
    public TextMeshProUGUI sector3Text;


    private int _s1Time;
    private int _s2Time;
    private int _s3Time;

    private byte _currentPlayerCarId = 0;
    
    
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

    private void ResetTimes()
    {
        currentLapText.text = "Lap: 00,00.000";
        sector1Text.text = "Sector 1: 00,00.000";
        sector2Text.text = "Sector 2: 00,00.000";
        sector3Text.text = "Sector 3: 00,00.000";
    }
    
    void Update()
    {
        //Lap time
        int currentLapMili = (int)(F1TS_currentLapTime(_currentPlayerCarId) * 1000);
        if (currentLapMili != 0.0)
        {
            int m = currentLapMili / 60000;
            int secMill = (currentLapMili - m * 60000);
            int s = secMill / 1000;
            int ms = secMill % 1000;
            currentLapText.text = "Lap: " + m.ToString("00") + "," + s.ToString("00") + "." + ms.ToString("000");
        }

        int currentSector = F1TS_sector(_currentPlayerCarId);
        if (currentSector == 0) //s1
        {
            int s = currentLapMili / 1000;
            int ms = currentLapMili % 1000;
            sector1Text.text = "Sector 1: 00," + s.ToString("00") + "." + ms.ToString("000");
        }
        else if (currentSector == 1) //s2
        {
            ushort s1Time = F1TS_sector1(_currentPlayerCarId);
            int s = s1Time / 1000;
            int ms = s1Time % 1000;
            sector1Text.text = "Sector 1: 00," + s.ToString("00") + "." + ms.ToString("000");

            int s2Time = currentLapMili - s1Time;
            s = s2Time / 1000;
            ms = s2Time % 1000;
            sector2Text.text = "Sector 2: 00," + s.ToString("00") + "." + ms.ToString("000");
        }
        else if (currentSector == 2) //s3
        {
            ushort s1Time = F1TS_sector1(_currentPlayerCarId);
            int s = s1Time / 1000;
            int ms = s1Time % 1000;
            sector1Text.text = "Sector 1: 00," + s.ToString("00") + "." + ms.ToString("000");

            ushort s2Time = F1TS_sector2(_currentPlayerCarId);
            s = s2Time / 1000;
            ms = s2Time % 1000;
            sector2Text.text = "Sector 2: 00," + s.ToString("00") + "." + ms.ToString("000");

            int s3Time = currentLapMili - s1Time - s2Time;
            s = s3Time / 1000;
            ms = s3Time % 1000;
            sector3Text.text = "Sector 3: 00," + s.ToString("00") + "." + ms.ToString("000");
        }
    }

    public override void OnNewLap(int lap)
    {
        ResetTimes();
    }

    public override void OnPlayerCarIdChanged(byte playerCarId)
    {
        _currentPlayerCarId = playerCarId;
    }
}
