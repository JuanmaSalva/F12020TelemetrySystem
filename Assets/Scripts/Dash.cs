using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;

public class Dash : TelemetryListener
{
    [DllImport("F12020Telemetry")]
    private static extern short F1TS_speed(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_throttle(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_brake(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern sbyte F1TS_gear(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern short F1TS_engineRPM(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_revLightsPercent(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_fuelRemainingLaps(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_ersStoreEnergy(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_ersDeployedThisLap(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_ersHarvestedThisLapMGUK(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_ersHarvestedThisLapMGUH(byte carId);



    public TextMeshProUGUI speed;
    public TextMeshProUGUI revs;
    public TextMeshProUGUI gear;
    public TextMeshProUGUI batteryCharge;
    public TextMeshProUGUI fuel;

    public Image lapBatteryLeft;
    public Image lapBatteryRecovered;

    public Image throttle;
    public Image brake;

    public Image batteryIcon;
    public Image fuelIcon;

    [Title("Revmeter")]
    public Image[] revMeter;
    public Color low;
    public Color mid;
    public Color high;
    

    private byte _currentPlayerCarId = 0;
    private float _maxErsStored = 0;

    void Start()
    {
        speed.color = Manager.instance.colorPalette.PanelInfo;
        revs.color = Manager.instance.colorPalette.PanelInfo;
        gear.color = Manager.instance.colorPalette.PanelInfo;
        batteryCharge.color = Manager.instance.colorPalette.PanelInfo;
        fuel.color = Manager.instance.colorPalette.PanelInfo;

        lapBatteryLeft.color = Manager.instance.colorPalette.PanelInfo;
        lapBatteryRecovered.color = Manager.instance.colorPalette.PanelInfo;
        throttle.color = Manager.instance.colorPalette.PanelInfo;
        brake.color = Manager.instance.colorPalette.PanelInfo;

        batteryIcon.color = Manager.instance.colorPalette.PanelInfo;
        fuelIcon.color = Manager.instance.colorPalette.PanelInfo;

        ClearRevLights();

        EventManager.instance.AddListener(this);
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
    }

    void Update()
    {
        speed.text = F1TS_speed(_currentPlayerCarId).ToString();
        revs.text = F1TS_engineRPM(_currentPlayerCarId).ToString();
        gear.text = F1TS_gear(_currentPlayerCarId).ToString();

        //print(F1TS_ersStoreEnergy(currentPlayerCarId));

        fuel.text = F1TS_fuelRemainingLaps(_currentPlayerCarId).ToString("#,##");
        throttle.transform.localScale = new Vector3(F1TS_throttle(_currentPlayerCarId), 1, 1);
        brake.transform.localScale = new Vector3(F1TS_brake(_currentPlayerCarId), 1, 1);

        UpdateRevs(F1TS_revLightsPercent(_currentPlayerCarId));
    }

    private void ClearRevLights()
    {
        foreach (Image dot in revMeter)
            dot.color = Manager.instance.colorPalette.PanelInfo;
    }

    private void UpdateRevs(byte percentage)
    {
        int lightToLight = (int)Mathf.Ceil((percentage * 14.0f) / 100.0f);
        for(int i = 0; i < revMeter.Length; i++)
        {
            if (i < lightToLight)
            {
                if (i < 5)
                    revMeter[i].color = low;
                else if (i < 10)
                    revMeter[i].color = mid;
                else
                    revMeter[i].color = high;
            }
            else
                revMeter[i].color = Manager.instance.colorPalette.PanelInfo;
        }
    }

    private void UpdateErs()
    {

    }

    public override void OnPlayerCarIdChanged(byte playerCarId)
    {
        _currentPlayerCarId = playerCarId;
    }

}
