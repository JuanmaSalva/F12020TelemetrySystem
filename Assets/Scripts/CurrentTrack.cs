using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using F1TS;


public class CurrentTrack : TelemetryListener
{
    [DllImport("F12020Telemetry")]
    private static extern sbyte F1TS_trackId();


    public TextMeshProUGUI currentTrackText;
    public F1TS.Graph graph;

    sbyte currentTrackId = 0;
    string trackName;

    private void Start()
    {
        EventManager.instance.AddListener(this);
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
    }

    public override void OnNewTrack(short length, sbyte trackId)
    {
        currentTrackText.text = GetTrackName(trackId);
    }

    string GetTrackName(sbyte trackName)
    {
        switch(trackName)
        {
            case (0):
                return "Melburne";
            case (1):
                return "Paul Ricard";
            case (2):
                return "Shangai";
            case (3):
                return "Sakhir (Bahrein)";
            case (4):
                return "Cataluya";
            case (5):
                return "Monaco";
            case (6):
                return "Montreal";
            case (7):
                return "Silverstone";
            case (8):
                return "Hockenheim";
            case (9):
                return "Hungaroring";
            case (10):
                return "Spa";
            case (11):
                return "Monza";
            case (12):
                return "Singapore";
            case (13):
                return "Suzuka";
            case (14):
                return "Abu Dhabi";
            case (15):
                return "Texas";
            case (16):
                return "Brazil";
            case (17):
                return "Austria";
            case (18):
                return "Sochi";
            case (19):
                return "Mexico";
            case (20):
                return "Baku";
            case (21):
                return "Sakhir (short)";
            case (22):
                return "Silverstone (short)";
            case (23):
                return "Texas (short)";
            case (24):
                return "Suzuka (short)";
            case (25):
                return "Hanoi";
            case (26):
                return "Zondvoort";
            default:
                return "N/A";
        }
    }
}
