using System;
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
     [DllImport("F12020Telemetry")]
     private static extern ushort F1TS_currentLapInvalid(byte carId);
    
    
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

    public List<ILapListener> _lapListeners;

    
    //---------UNITY FUNCTIONS---------
    private void Awake()
    {
        _lapListeners = new List<ILapListener>();
    }

    void Start()
    {
        EventManager.instance.AddListener(this);
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
    }

    void LateUpdate()
    {
        //check overall lap and sector times
        for (byte i = 0; i < _numActiveCars; i++)
        {
            //update lap
            CheckFastestLap(i);
            
            //Update sectors
            CheckFastestOverallSector(i, 0,  F1TS_bestOverallSector1TimeInMS(i),
                ref _fastestOverallS1Time, ref _fastestPersonalS1Time);
            CheckFastestOverallSector(i, 1,  F1TS_bestOverallSector2TimeInMS(i),
                ref _fastestOverallS2Time, ref _fastestPersonalS2Time);
            CheckFastestOverallSector(i, 2,  F1TS_bestOverallSector3TimeInMS(i),
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
            foreach (ILapListener lapListener in _lapListeners)
                lapListener.OnFastestLap(carFastestLap, carId == _currentPlayerCarId);
        }
        //Fastest personal time
        if (carId == _currentPlayerCarId && carFastestLap < _fastestPersonalLapTime)
        {
            _fastestPersonalLapTime = carFastestLap;
            foreach (ILapListener lapListener in _lapListeners)
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
            foreach (ILapListener lapListener in _lapListeners)
                lapListener.OnFastestSector(carFastestSectorTime, sector, carId == _currentPlayerCarId);
        }
        //Fastest personal time
        if (carId == _currentPlayerCarId && carFastestSectorTime < fastestPersonalSectorTime)
        {
            fastestPersonalSectorTime = carFastestSectorTime;
            foreach (ILapListener lapListener in _lapListeners)
                lapListener.OnFastestSector(carFastestSectorTime, sector, carId == _currentPlayerCarId);
        }
    }
    
    
    //---------PUBLIC---------
    public void NewLap(int time, int s1Time, int s2Time, int s3Time, int lapNum)
    {
        //invalid lap (not a completed lap or a repeated lap)
        if (lapNum == lastLapRegistered || time == 0 || s1Time == 0 || s2Time == 0 || s3Time == 0)
            return;
        lastLapRegistered = lapNum;
        
        
        IndividualLap individualLap = Instantiate(IndividualLapPrefab, IndividualLapParent).GetComponent<IndividualLap>();
        individualLap.SetLapManager(this);
        individualLap.SetTime(time, s1Time, s2Time, s3Time, lapNum, F1TS_currentLapInvalid(_currentPlayerCarId) == 0);
        _lapListeners.Add(individualLap);
        
    }

    public void AddLapListener(ILapListener lapListener)
    {
        _lapListeners.Add(lapListener);
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
    

    //---------GETTERS---------
    
    //Overall
    public int GetOverallFastestLap()
    {
        return _fastestOverallLapTime;
    }
    public int GetOverallFastestSector1()
    {
        return _fastestOverallS1Time;
    }
    public int GetOverallFastestSector2()
    {
        return _fastestOverallS2Time;
    }
    public int GetOverallFastestSector3()
    {
        return _fastestOverallS3Time;
    }
    
    //Personal
    public int GetPersonalFastestLap()
    {
        return _fastestPersonalLapTime;
    }
    public int GetPersonalFastestSector1()
    {
        return _fastestPersonalS1Time;
    }
    public int GetPersonalFastestSector2()
    {
        return _fastestPersonalS2Time;
    }
    public int GetPersonalFastestSector3()
    {
        return _fastestPersonalS3Time;
    }
}
