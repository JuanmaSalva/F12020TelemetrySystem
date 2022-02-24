using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class CarStatus : TelemetryListener
{
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_frontLeftWingDamage(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_frontRightWingDamage(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_drsFault(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_rearWingDamage(byte carId);
    
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_tyresWear(byte carId, byte tyre);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_engineDamage(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_gearBoxDamage(byte carId);
    
    public Image cockPit;
    public Image frontWingLeft;
    public Image frontWingRight;
    public Image DRS;
    public Image engine;
    public Image gearBox;
    public Image[] cojinetes;
    public Image[] tyres;

    private byte currentCarId = 0;
    
    void Start()
    {
        cockPit.color = Manager.instance.colorPalette.PanelInfo;
        frontWingLeft.color = Manager.instance.colorPalette.GreenStatus;
        frontWingRight.color = Manager.instance.colorPalette.GreenStatus;
        DRS.color = Manager.instance.colorPalette.GreenStatus;
        engine.color = Manager.instance.colorPalette.GreenStatus;
        gearBox.color = Manager.instance.colorPalette.GreenStatus;
        foreach (Image i in cojinetes)
            i.color = Manager.instance.colorPalette.GreenStatus;
        foreach (Image i in tyres)
            i.color = Manager.instance.colorPalette.GreenStatus;
            
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
        EventManager.instance.AddListener(this);
    }


    void Update()
    {
        frontWingLeft.color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
            Manager.instance.colorPalette.GreenStatus, F1TS_frontLeftWingDamage(currentCarId) / 100.0f);
        frontWingRight.color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
            Manager.instance.colorPalette.GreenStatus, F1TS_frontRightWingDamage(currentCarId) / 100.0f);
        
        if (F1TS_drsFault(currentCarId) == 1)
            DRS.color = Manager.instance.colorPalette.RedStatus;
        else
            DRS.color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
                Manager.instance.colorPalette.GreenStatus, F1TS_rearWingDamage(currentCarId) / 100.0f);
        
        engine.color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
            Manager.instance.colorPalette.GreenStatus, F1TS_engineDamage(currentCarId) / 100.0f);
        gearBox.color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
            Manager.instance.colorPalette.GreenStatus, F1TS_engineDamage(currentCarId) / 100.0f);
        
        UpdateWheels();
    }

    private void UpdateWheels()
    {
        for (byte i = 0; i < 4; i++)
        {
            tyres[i].color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
                Manager.instance.colorPalette.GreenStatus, F1TS_tyresWear(currentCarId, i) / 100.0f);
        }
    }
    
    
    public override void OnPlayerCarIdChanged(byte playerCarId)
    {
        currentCarId = playerCarId;
    }
}
