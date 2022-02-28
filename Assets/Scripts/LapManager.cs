using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LapManager : TelemetryListener
{
    [DllImport("F12020Telemetry")]
     private static extern float F1TS_bestLapTime(byte id);
     [DllImport("F12020Telemetry")]
     private static extern ushort F1TS_bestOverallSector1TimeInMS(byte carId);
     [DllImport("F12020Telemetry")]
     private static extern ushort F1TS_bestOverallSector2TimeInMS(byte carId);
     [DllImport("F12020Telemetry")]
     private static extern ushort F1TS_bestOverallSector3TimeInMS(byte carId);
    
    
    public GameObject IndividualLapPrefab;
    public Transform IndividualLapParent;

    public CurrentLapInfo currentLapInfo;
    public FastestLapInfo fastestLapInfo;
    
    private IndividualLap _fastestPersonalLap;
    private IndividualLap _fastestOverallLap;
    
    private IndividualLap _fastestPersonalS1;
    private IndividualLap _fastestPersonalS2;
    private IndividualLap _fastestPersonalS3;
    
    private IndividualLap _fastestOverallS1;
    private IndividualLap _fastestOverallS2;
    private IndividualLap _fastestOverallS3;

    
    private int _fastestPersonalLapTime = Int32.MaxValue;
    private int _fastestOverallLapTime = Int32.MaxValue;
    
    private int _fastestPersonalS1Time = Int32.MaxValue;
    private int _fastestPersonalS2Time = Int32.MaxValue;
    private int _fastestPersonalS3Time = Int32.MaxValue;
    
    private int _fastestOverallS1Time = Int32.MaxValue;
    private int _fastestOverallS2Time = Int32.MaxValue;
    private int _fastestOverallS3Time = Int32.MaxValue;

    private byte _numActiveCars = 0;
    private byte _currentPlayerCarId = 0;
    
    void Start()
    {
        EventManager.instance.AddListener(this);
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
    }

    void Update()
    {
        //check overall lap and sector times
        for (byte i = 0; i < _numActiveCars; i++)
        {
            int fastestCarLap = (int)(F1TS_bestLapTime(i) * 1000);
            if (fastestCarLap < _fastestOverallLapTime)
            {
                fastestCarLap = _fastestOverallLapTime;
                if (_fastestOverallLap != null)
                {
                    if(i != _currentPlayerCarId)
                        _fastestOverallLap.SetLapColor(Manager.instance.colorPalette.PersonalBestTime);
                    else
                        _fastestOverallLap.SetLapColor(Manager.instance.colorPalette.NormalTime);
                }
                
                fastestLapInfo.SetOverallFastestLap(fastestCarLap);
            }
        }
    }

    public void NewLap(int time, int s1Time, int s2Time, int s3Time, int lapNum)
    {
        if (time == 0)
            return;
        
        IndividualLap individualLap = Instantiate(IndividualLapPrefab, IndividualLapParent).GetComponent<IndividualLap>();
        individualLap.SetTime(time, s1Time, s2Time, s3Time, lapNum);

        
        if (time <= _fastestPersonalLapTime)
        {
            _fastestPersonalLap = individualLap;
            _fastestPersonalLap.SetLapColor(Manager.instance.colorPalette.PersonalBestTime);
        }
        if (s1Time <= _fastestPersonalS1Time)
        {
            _fastestPersonalS1 = individualLap;
            _fastestPersonalS1.SetSectorColor(1, Manager.instance.colorPalette.PersonalBestTime);
        }
        if (s2Time <= _fastestPersonalS2Time)
        {
            _fastestPersonalS2 = individualLap;
            _fastestPersonalS2.SetSectorColor(2, Manager.instance.colorPalette.PersonalBestTime);
        }
        if (s3Time <= _fastestPersonalS3Time)
        {
            _fastestPersonalS3 = individualLap;
            _fastestPersonalS3.SetSectorColor(3, Manager.instance.colorPalette.PersonalBestTime);
        }
        //TODO a lo mejor hay q poner aqui otra condicion para cuando es el tiempo más rápido general y que no parpadea verde entre medio
    }

    
    public void FastestPersonalSector(int sector, int time)
    {
        if (time == 0)
            return;
        
        
        if (sector == 1)
        {
            UpdateSector(sector, time, ref _fastestPersonalS1Time, ref _fastestOverallS1Time,
                _fastestPersonalS1, _fastestOverallS1);
        }
        else if (sector == 2)
        {
            UpdateSector(sector, time, ref _fastestPersonalS2Time, ref _fastestOverallS2Time,
                _fastestPersonalS2, _fastestOverallS2);
        }
        else if (sector == 3)
        {
            UpdateSector(sector, time, ref _fastestPersonalS3Time, ref _fastestOverallS3Time,
                _fastestPersonalS3, _fastestOverallS3);
        }
    }

    private void UpdateSector(int sector, int time, ref int fastestPersonalSectorTime, ref int fastestOverallSectorTime,
        IndividualLap fastestPersonalSector, IndividualLap fastestOverallSector)
    {
        fastestPersonalSectorTime = time;

        //Check if it's the fastest overall time
        if (time < fastestOverallSectorTime)
        {
            fastestOverallSectorTime = time;
            if (fastestOverallSector != null)
                fastestOverallSector.SetSectorColor(sector, Manager.instance.colorPalette.NormalTime);
        }
        else if (fastestPersonalSector != null)
            fastestPersonalSector.SetSectorColor(sector, Manager.instance.colorPalette.NormalTime);
    }


    public override void OnNumActiveCarsChange(byte numActiveCars)
    {
        _numActiveCars = numActiveCars;
    }

    public override void OnPlayerCarIdChanged(byte playerCarId)
    {
        _currentPlayerCarId = playerCarId;
    }

}
