using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Threading;

public class EventManager : MonoBehaviour
{
    public static EventManager instance = null;


    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_playerCarIndex();

    [DllImport("F12020Telemetry")]
    private static extern float F1TS_lastTimeLap(byte id);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_currentLapNum(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern short F1TS_trackLength();
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_sector(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_bestLapTime(byte id);
    [DllImport("F12020Telemetry")]
    private static extern sbyte F1TS_trackId();
    [DllImport("F12020Telemetry")]
    private static extern ushort F1TS_sector2(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_numActiveCars();


    private byte _currentPlayerCarIndex = 0;
    private short _currentLap = 0;
    private sbyte _currentTrackId = -1; //invalid number
    private short _currentTrackLength = 0;
    private float _bestLapTime = float.MaxValue;
    private byte _numActiveCars = 0;

    private bool _onLapStarted = false;

    private List<TelemetryListener> _listeners;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _listeners = new List<TelemetryListener>();
    }

    private void Start()
    {
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
    }

    public void AddListener(TelemetryListener listener)
    {
        _listeners.Add(listener);
    }


    void Update()
    {
        if (CheckNewPlayerCarIndex())
        {
            foreach (TelemetryListener tl in _listeners)
                tl.OnPlayerCarIdChanged(_currentPlayerCarIndex);
        }

        if (CheckNewTrack())
        {
            foreach (TelemetryListener tl in _listeners)
                tl.OnNewTrack(_currentTrackLength, _currentTrackId);
        }

        if (CheckNewLap())
        {
            if (CheckFastestLap())
            {
                foreach (TelemetryListener tl in _listeners)
                    tl.OnFastestLap(F1TS_bestLapTime(_currentPlayerCarIndex));
            }

            foreach (TelemetryListener tl in _listeners)
                tl.OnNewLap(_currentLap);
        }

        if (F1TS_numActiveCars() != _numActiveCars)
        {
            _numActiveCars = F1TS_numActiveCars();
            foreach (TelemetryListener tl in _listeners)
                tl.OnNumActiveCarsChange(_numActiveCars);
            
        }

    }


    bool CheckNewPlayerCarIndex()
    {
        if(F1TS_playerCarIndex() != _currentPlayerCarIndex)
        {
            _currentPlayerCarIndex = F1TS_playerCarIndex();
            return true;
        }
        return false;
    }

    bool CheckNewLap()
    {
        if(!_onLapStarted && F1TS_sector(_currentPlayerCarIndex) == 0)
        {
            print("New lap");
            print(F1TS_lastTimeLap(_currentPlayerCarIndex));
            _currentLap = F1TS_currentLapNum(_currentPlayerCarIndex);
            _onLapStarted = true;
            return true;
        }
        else if (F1TS_sector(_currentPlayerCarIndex) == 2)
        {
            _onLapStarted = false;
            
            if(F1TS_sector2(_currentPlayerCarIndex) == 0)
            {
                //limpiamos el grafo
                foreach (TelemetryListener tl in _listeners)
                    tl.OnLapCleared();
            }
        }
        return false;
    }

    bool CheckFastestLap()
    {
        if(F1TS_lastTimeLap(_currentPlayerCarIndex) < _bestLapTime && F1TS_lastTimeLap(_currentPlayerCarIndex) >= 30) //30 = min seconds for a timed lap
        {
            _bestLapTime = F1TS_lastTimeLap(_currentPlayerCarIndex);
            return true;
        }
        return false;
    }

    bool CheckNewTrack()
    {
        if(F1TS_trackId() != _currentTrackId || _currentTrackLength != F1TS_trackLength())
        {
            _currentTrackId = F1TS_trackId();
            _currentTrackLength = F1TS_trackLength();
            return true;
        }
        return false;
    }
}
