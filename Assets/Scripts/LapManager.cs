using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LapManager : TelemetryListener
{
    //---------TELEMETRY SYSTEM DECLARATIONS---------
    [DllImport("F12020Telemetry")]
     private static extern float F1TS_bestLapTime(byte id);
     [DllImport("F12020Telemetry")]
     private static extern ushort F1TS_bestOverallSector1TimeInMS(byte carId);
     [DllImport("F12020Telemetry")]
     private static extern ushort F1TS_bestOverallSector2TimeInMS(byte carId);
     [DllImport("F12020Telemetry")]
     private static extern ushort F1TS_bestOverallSector3TimeInMS(byte carId);
    
    
     //---------VARIABLES---------
    public GameObject IndividualLapPrefab;
    public Transform IndividualLapParent;

    
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

    public List<LapListener> _lapListeners;

    
    //---------UNITY FUNCTIONS---------
    void Start()
    {
        EventManager.instance.AddListener(this);
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
        _lapListeners = new List<LapListener>();
    }

    void Update()
    {
        //check overall lap and sector times
        for (byte i = 0; i < _numActiveCars; i++)
        {
            //update lap
            CheckFastestLap(i);
            
            //Update sectors
            CheckFastestOverallSector(i, 1,  F1TS_bestOverallSector1TimeInMS(i),
                ref _fastestOverallS1Time, ref _fastestPersonalS1Time);
            CheckFastestOverallSector(i, 2,  F1TS_bestOverallSector2TimeInMS(i),
                ref _fastestOverallS2Time, ref _fastestPersonalS2Time);
            CheckFastestOverallSector(i, 3,  F1TS_bestOverallSector3TimeInMS(i),
                ref _fastestOverallS3Time, ref _fastestPersonalS3Time);
        }
    }

    
    //---------PRIVATE---------
    private void CheckFastestLap(byte carId)
    {
        int carFastestLap = (int)(F1TS_bestLapTime(carId) * 1000);
        if (carFastestLap <= 0)
            return;
        
        //Fastest overall time
        if (carFastestLap < _fastestOverallLapTime)
        {
            _fastestOverallLapTime = carFastestLap;
            foreach (LapListener lapListener in _lapListeners)
                lapListener.OnFastestLap(carFastestLap, carId == _currentPlayerCarId);
        }
        //Fastest personal time
        else if (carFastestLap < _fastestPersonalLapTime)
        {
            _fastestPersonalLapTime = carFastestLap;
            foreach (LapListener lapListener in _lapListeners)
                lapListener.OnFastestLap(carFastestLap, carId == _currentPlayerCarId);
        }
    }

    private void CheckFastestOverallSector(byte carId, byte sector, int carFastestSectorTime,
        ref int fastestOverallSectorTime, ref int fastestPersonalSectorTime)
    {
        if (carFastestSectorTime <= 0)
            return;

       
        //Fastest overall time
        if (carFastestSectorTime < fastestOverallSectorTime)
        {
            fastestOverallSectorTime = carFastestSectorTime;
            foreach (LapListener lapListener in _lapListeners)
                lapListener.OnFastestSector(carFastestSectorTime, sector, carId == _currentPlayerCarId);
        }
        //Fastest personal time
        else if (carFastestSectorTime < fastestPersonalSectorTime)
        {
            fastestPersonalSectorTime = carFastestSectorTime;
            foreach (LapListener lapListener in _lapListeners)
                lapListener.OnFastestSector(carFastestSectorTime, sector, carId == _currentPlayerCarId);
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
    }



    //---------TELEMETRY LISTENERS FUNCTIONS---------
    public override void OnNumActiveCarsChange(byte numActiveCars)
    {
        _numActiveCars = numActiveCars;
    }

    public override void OnPlayerCarIdChanged(byte playerCarId)
    {
        _currentPlayerCarId = playerCarId;
    }

}
