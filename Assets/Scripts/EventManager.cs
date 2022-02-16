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


    private byte currentPlayerCarIndex = 0;
    private short currentLap = -10; //invalid number
    private sbyte currentTrackId = -1; //invalid number
    private short currentTrackLength = 0;
    private float bestLapTime = float.MaxValue;

    private bool onLapStarted = false;

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

        if (CheckNewTrack())
        {
            foreach (TelemetryListener tl in listeners)
                tl.OnNewTrack(currentTrackLength, currentTrackId);
        }

        if (CheckNewLap())
        {
            if (CheckFastestLap())
            {
                foreach (TelemetryListener tl in listeners)
                    tl.OnFastestLap(F1TS_bestLapTime(currentPlayerCarIndex));
            }

            foreach (TelemetryListener tl in listeners)
                tl.OnNewLap(currentLap);
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
        if(!onLapStarted && F1TS_sector(currentPlayerCarIndex) == 0)
        {
            print("New lap");
            print(F1TS_lastTimeLap(currentPlayerCarIndex));
            currentLap = F1TS_currentLapNum(currentPlayerCarIndex);
            onLapStarted = true;
            return true;
        }
        else if (F1TS_sector(currentPlayerCarIndex) == 2)
        {
            onLapStarted = false;
        }
        return false;
    }

    bool CheckFastestLap()
    {
        if(F1TS_lastTimeLap(currentPlayerCarIndex) < bestLapTime && F1TS_lastTimeLap(currentPlayerCarIndex) >= 30) //30 = min seconds for a timed lap
        {
            bestLapTime = F1TS_lastTimeLap(currentPlayerCarIndex);
            return true;
        }
        return false;
    }

    bool CheckNewTrack()
    {
        if(F1TS_trackId() != currentTrackId || currentTrackLength != F1TS_trackLength())
        {
            currentTrackId = F1TS_trackId();
            currentTrackLength = F1TS_trackLength();
            return true;
        }
        return false;
    }
}
