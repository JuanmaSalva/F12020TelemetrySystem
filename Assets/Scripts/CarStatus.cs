using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
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
    private static extern byte F1TS_tyresWear(byte carId, byte tyre);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_tyresDamage(byte carId, byte tyre);
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

    public TextMeshProUGUI[] tyresText;
    public TextMeshProUGUI frontWingLeftText;
    public TextMeshProUGUI frontWingRightText;
    public TextMeshProUGUI engineText;
    public TextMeshProUGUI gearBoxText;
    
    
    private byte _currentCarId = 0;
    
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
        byte aux;


        aux = (byte)(100 - F1TS_frontLeftWingDamage(_currentCarId));
        frontWingLeftText.text = aux.ToString() + "%";
        frontWingLeft.color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
            Manager.instance.colorPalette.GreenStatus, aux / 100.0f);

        aux = (byte)(100 - F1TS_frontRightWingDamage(_currentCarId));
        frontWingRightText.text = aux.ToString() + "%";
        frontWingRight.color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
            Manager.instance.colorPalette.GreenStatus, aux / 100.0f);
        
        if (F1TS_drsFault(_currentCarId) == 1)
            DRS.color = Manager.instance.colorPalette.RedStatus;
        else
            DRS.color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
                Manager.instance.colorPalette.GreenStatus, (100 - F1TS_rearWingDamage(_currentCarId)) / 100.0f);

        aux = (byte)(100 -F1TS_engineDamage(_currentCarId));
        engineText.text = aux.ToString() + "%";
        engine.color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
            Manager.instance.colorPalette.GreenStatus, aux / 100.0f);

        aux = (byte)(100 -F1TS_gearBoxDamage(_currentCarId));
        gearBoxText.text = aux.ToString() + "%";
        gearBox.color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
            Manager.instance.colorPalette.GreenStatus, aux / 100.0f);
        
        UpdateWheels();
    }

    private void UpdateWheels()
    {
        for (byte i = 0; i < 4; i++)
        {
            byte aux = (byte)(100 - Math.Max(F1TS_tyresWear(_currentCarId, i),
                F1TS_tyresDamage(_currentCarId, i)));
            tyresText[i].text = aux.ToString() + "%";
            tyres[i].color = Color.Lerp(Manager.instance.colorPalette.RedStatus,
                Manager.instance.colorPalette.GreenStatus, aux / 100.0f);
        }
    }
    
    
    public override void OnPlayerCarIdChanged(byte playerCarId)
    {
        _currentCarId = playerCarId;
    }
}
