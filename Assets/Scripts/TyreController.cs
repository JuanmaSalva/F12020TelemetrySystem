using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TyreController : MonoBehaviour
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


    private void Start()
    {
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
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
        ushort curretnCompund = F1TS_actualTyreCompound(0);
        tyres[tyre].compound.text = tyres[tyre].GetCompundString(curretnCompund);
    }
    
    void UpdateTyreWear(byte tyre)
    {
        tyres[tyre].wear.text = (100 - F1TS_tyresWear(0, tyre)) + "%";
    }
    
    void UpdateTyreSurfaceTemp(byte tyre)
    {
        ushort temp = F1TS_tyresSurfaceTemperature(0, tyre);
        if (temp == 0) 
            tyres[tyre].surfaceTemp.text = "Surface temp: - °C";
        else
            tyres[tyre].surfaceTemp.text = "Surface temp: " + temp + "°C";
    }
    
    void UpdateTyreInnerTemp(byte tyre)
    {
        ushort temp = F1TS_tyresInnerTemperature(0, tyre);
        if (temp == 0) 
            tyres[tyre].innerTemp.text = "Inner temp: - °C";
        else
            tyres[tyre].innerTemp.text = "Inner temp: " + F1TS_tyresInnerTemperature(0, tyre) + "°C";
    }
    
    void UpdateTyreLaps(byte tyre)
    {
        tyres[tyre].laps.text = "Laps: " + F1TS_tyresAgeLaps(0);
    }
    
    
    void UpdateTyrePresure(byte tyre)
    {
        tyres[tyre].presure.text = "Presure: " + F1TS_tyrePressure(0, tyre);
    }
}
