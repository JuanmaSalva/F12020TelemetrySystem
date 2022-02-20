using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TyreController : TelemetryListener
{
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_maxGears(byte carId);
    
        
        
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_tyresWear(byte carId, byte tyre);
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_tyresSurfaceTemperature(byte carId, byte tyre);
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_tyresInnerTemperature(byte carId, byte tyre);
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_tyresAgeLaps(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_actualTyreCompound(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_tyrePressure(byte carId, byte tyre);
    
    public Tyre[] tyres;

    private byte currentPlayerCarId = 0;


    private void Start()
    {
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
        EventManager.instance.AddListener(this);
        foreach (Tyre tyre in tyres)
            tyre.ChangeColor();
    }

    void Update()
    {
        for (byte i = 0; i < 4; i++)
            UpdateTyreData(i);
    }

    void UpdateTyreData(byte tyre)
    {
        UpdateTyreWear(tyre);
        UpdateTyreSurfaceTemp(tyre);
        UpdateTyreInnerTemp(tyre);
        UpdateTyreLaps(tyre);
        UpdateTyreCompound(tyre);
        UpdateTyrePresure(tyre);
    }

    void UpdateTyreCompound(byte tyre)
    {
        ushort curretnCompund = F1TS_actualTyreCompound(currentPlayerCarId);
        tyres[tyre].compound.text = tyres[tyre].GetCompundString(curretnCompund);
    }
    
    void UpdateTyreWear(byte tyre)
    {
        tyres[tyre].wear.text = (100 - F1TS_tyresWear(currentPlayerCarId, tyre)) + "%";
    }
    
    void UpdateTyreSurfaceTemp(byte tyre)
    {
        ushort temp = F1TS_tyresSurfaceTemperature(currentPlayerCarId, tyre);
        if (temp == 0) 
            tyres[tyre].surfaceTemp.text = "Surface temp: - °C";
        else
            tyres[tyre].surfaceTemp.text = "Surface temp: " + temp + "°C";
    }
    
    void UpdateTyreInnerTemp(byte tyre)
    {
        ushort temp = F1TS_tyresInnerTemperature(currentPlayerCarId, tyre);
        if (temp == 0) 
            tyres[tyre].innerTemp.text = "Inner temp: - °C";
        else
            tyres[tyre].innerTemp.text = "Inner temp: " + F1TS_tyresInnerTemperature(currentPlayerCarId, tyre) + "°C";
    }
    
    void UpdateTyreLaps(byte tyre)
    {
        tyres[tyre].laps.text = "Laps: " + F1TS_tyresAgeLaps(currentPlayerCarId);
    }
    
    
    void UpdateTyrePresure(byte tyre)
    {
        tyres[tyre].presure.text = "Presure: " + F1TS_tyrePressure(currentPlayerCarId, tyre);
    }

    public override void OnPlayerCarIdChanged(byte playerCarId)
    {
        currentPlayerCarId = playerCarId;
    }
}
