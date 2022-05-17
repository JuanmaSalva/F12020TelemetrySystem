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

    private int lastLapRegistered = -1;

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
            //update lap
            CheckFastestOverallLap(i);
            
            //Update sectors
            CheckFastestOverallSector(i, 1,  F1TS_bestOverallSector1TimeInMS(i),
                ref _fastestOverallS1Time, _fastestOverallS1);
            CheckFastestOverallSector(i, 2,  F1TS_bestOverallSector2TimeInMS(i),
                ref _fastestOverallS2Time, _fastestOverallS2);
            CheckFastestOverallSector(i, 3,  F1TS_bestOverallSector3TimeInMS(i),
                ref _fastestOverallS3Time, _fastestOverallS3);
        }
    }

    private void CheckFastestOverallLap(byte carId)
    {
        int carFastestLap = (int)(F1TS_bestLapTime(carId) * 1000);
        if (carFastestLap <= 0)
            return;
        
        if (carFastestLap < _fastestOverallLapTime)
        {
            _fastestOverallLapTime = carFastestLap;
            if (_fastestOverallLap != null)
            {
                if (carId != _currentPlayerCarId)
                    _fastestOverallLap.SetLapColor(Manager.instance.colorPalette.PersonalBestTime);
                else
                    _fastestOverallLap.SetLapColor(Manager.instance.colorPalette.NormalTime);
            }

            fastestLapInfo.SetOverallFastestLap(carFastestLap);
        }
    }

    private void CheckFastestOverallSector(byte carId, byte sector, int carFastestSectorTime,
        ref int fastestOverallSectorTime, IndividualLap fastestOverallSector)
    {
        if (carFastestSectorTime <= 0)
            return;

       
        if (carFastestSectorTime < fastestOverallSectorTime)
        {
            //print("NEW FASTEST SECTOR " + sector);
            //print("Time:" + carFastestSector);
            fastestOverallSectorTime = carFastestSectorTime;
            if (fastestOverallSector != null)
            {
                if (carId != _currentPlayerCarId)
                    fastestOverallSector.SetLapColor(Manager.instance.colorPalette.PersonalBestTime);
                else
                    fastestOverallSector.SetLapColor(Manager.instance.colorPalette.NormalTime);
            }
            
            fastestLapInfo.SetOverallFastestSector(sector, carFastestSectorTime);
            currentLapInfo.SetOverallFastestSector(sector, carFastestSectorTime);
        }
    }
    
    
    public void NewLap(int time, int s1Time, int s2Time, int s3Time, int lapNum)
    {
        //invalid lap (not a completed lap or a repeated lap)
        if (lapNum == lastLapRegistered || time == 0 || s1Time == 0 || s2Time == 0 || s3Time == 0)
            return;
        lastLapRegistered = lapNum;
        
        
        IndividualLap individualLap = Instantiate(IndividualLapPrefab, IndividualLapParent).GetComponent<IndividualLap>();
        individualLap.SetTime(time, s1Time, s2Time, s3Time, lapNum);

        
        if (time <= _fastestPersonalLapTime)
        {
            if(_fastestPersonalLap != null)
                _fastestPersonalLap.SetLapColor(Manager.instance.colorPalette.NormalTime);
            _fastestPersonalLapTime = time;
            _fastestPersonalLap = individualLap;
            _fastestPersonalLap.SetLapColor(Manager.instance.colorPalette.PersonalBestTime);
            
            if (time <= _fastestOverallLapTime)
            {
                if(_fastestOverallLap != null)
                    _fastestOverallLap.SetLapColor(Manager.instance.colorPalette.NormalTime);
                _fastestOverallLapTime = time;
                _fastestOverallLap = individualLap;
                _fastestOverallLap.SetLapColor(Manager.instance.colorPalette.OverallBestTime);
            }
        }
        if (s1Time <= _fastestPersonalS1Time)
        {
            if(_fastestPersonalS1 != null)
                _fastestPersonalS1.SetSectorColor(1, Manager.instance.colorPalette.NormalTime);
            _fastestPersonalS1 = individualLap;
            _fastestPersonalS1.SetSectorColor(1, Manager.instance.colorPalette.PersonalBestTime);
            _fastestPersonalS1Time = s1Time;
            
            if (s1Time <= _fastestOverallS1Time)
            {
                if(_fastestOverallS1 != null)
                    _fastestOverallS1.SetSectorColor(1, Manager.instance.colorPalette.NormalTime);
                _fastestOverallS1Time = s1Time;
                _fastestOverallS1 = individualLap;
                _fastestOverallS1.SetSectorColor(1, Manager.instance.colorPalette.OverallBestTime);
            }
        }
        if (s2Time <= _fastestPersonalS2Time)
        {
            if(_fastestPersonalS2 != null)
                _fastestPersonalS2.SetSectorColor(2, Manager.instance.colorPalette.NormalTime);
            _fastestPersonalS2 = individualLap;
            _fastestPersonalS2.SetSectorColor(2, Manager.instance.colorPalette.PersonalBestTime);
            _fastestPersonalS2Time = s2Time;
            
            if (s2Time <= _fastestOverallS2Time)
            {
                if(_fastestOverallS2 != null)
                    _fastestOverallS2.SetSectorColor(2, Manager.instance.colorPalette.NormalTime);
                _fastestOverallS2Time = s2Time;
                _fastestOverallS2 = individualLap;
                _fastestOverallS2.SetSectorColor(2, Manager.instance.colorPalette.OverallBestTime);
            }
        }
        if (s3Time <= _fastestPersonalS3Time)
        {
            if(_fastestPersonalS3 != null)
                _fastestPersonalS3.SetSectorColor(3, Manager.instance.colorPalette.NormalTime);
            _fastestPersonalS3 = individualLap;
            _fastestPersonalS3.SetSectorColor(3, Manager.instance.colorPalette.PersonalBestTime);
            _fastestPersonalS3Time = s3Time;
            
            if (s3Time <= _fastestOverallS3Time)
            {
                if(_fastestOverallS3 != null)
                    _fastestOverallS3.SetSectorColor(3, Manager.instance.colorPalette.NormalTime);
                _fastestOverallS3Time = s3Time;
                _fastestOverallS3 = individualLap;
                _fastestOverallS3.SetSectorColor(3, Manager.instance.colorPalette.OverallBestTime);
            }
        }
    }

    
    public void FastestPersonalSector(int sector, int time)
    {
        if (time == 0)
            return;
        
        
        if (sector == 1)
        {
            if (time == _fastestPersonalS1Time)
                return;
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
