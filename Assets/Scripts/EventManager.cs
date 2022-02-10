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
    private static extern ushort F1TS_sector1(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_bestLapTime(byte id);
    [DllImport("F12020Telemetry")]
    private static extern sbyte F1TS_trackId();


    private byte currentPlayerCarIndex = 0;
    private short currentLap = -10; //invalid number
    private sbyte currentTrackId = -1; //invalid number


    private List<TelemetryListener> listeners;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        listeners = new List<TelemetryListener>();
    }


    public void AddListener(TelemetryListener listener)
    {
        listeners.Add(listener);
    }


    void Update()
    {
        if (CheckNewPlayerCarIndex())
        {
            foreach (TelemetryListener tl in listeners)
                tl.OnPlayerCarIdChanged(currentPlayerCarIndex);
        }

        if (CheckNewLap())
        {
            foreach (TelemetryListener tl in listeners)
                tl.OnNewLap(currentLap);
        }

        if (CheckFastestLap())
        {
            foreach (TelemetryListener tl in listeners)
                tl.OnFastestLap(F1TS_bestLapTime(currentPlayerCarIndex));
        }

        if (CheckNewTrack())
        {
            foreach (TelemetryListener tl in listeners)
                tl.OnNewTrack(F1TS_trackLength(), currentTrackId);
        }
    }

    bool CheckNewPlayerCarIndex()
    {
        if(F1TS_playerCarIndex() != currentPlayerCarIndex)
        {
            currentPlayerCarIndex = F1TS_playerCarIndex();
            return true;
        }
        return false;
    }

    bool CheckNewLap()
    {
        if(F1TS_currentLapNum(currentPlayerCarIndex) != currentLap)
        {
            currentLap = F1TS_currentLapNum(currentPlayerCarIndex);
            return true;
        }
        return false;
    }

    bool CheckFastestLap()
    {
        if(F1TS_sector1(currentPlayerCarIndex) > 0)
        {
            if(F1TS_lastTimeLap(currentPlayerCarIndex) == F1TS_bestLapTime(currentPlayerCarIndex) && 
                F1TS_bestLapTime(currentPlayerCarIndex) != 0)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    bool CheckNewTrack()
    {
        if(F1TS_trackId() != currentTrackId)
        {
            currentTrackId = F1TS_trackId();
            return true;
        }
        return false;
    }
}
