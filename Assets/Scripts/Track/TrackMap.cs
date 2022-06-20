using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TrackMap : TelemetryListener
{
    [DllImport("F12020Telemetry")]
    private static extern float F1TS_lapDistance(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_teamId(byte carId);
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_raceNumber(byte carId);
    
    
    
    [SerializeField] private List<TrackCreator> _trackList;
    [SerializeField] private GameObject _driverIconPrefab;
    [SerializeField] private Transform _driversParent;

    private List<Image> _driversIcons;
    private TrackCreator _currentTrack;
    private byte _currentPlayerCarId = 0;
    private short _trackLength;
    
    
    void Start()
    {
        _driversIcons = new List<Image>();
        
        EventManager.instance.AddListener(this);
        Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
        
    }

    void Update()
    {
        for (byte i = 0; i < _driversIcons.Count; i++)
        {
            float currentDriverTrackPos = F1TS_lapDistance(i);
            float range = Math.Min((currentDriverTrackPos / _trackLength), 1.0f);
            if (range < 0) //for the start of a race (distance is negative)
                range += 1;

            _driversIcons[i].transform.position = _currentTrack.GetDriverIconPos(range);
            _driversIcons[i].color = Manager.instance.colorPalette.TeamColors[Math.Min((int)F1TS_teamId(i), 9)];
        }
    }

    public void LoadNewTrack(short length, sbyte trackId)
    {
        if(_currentTrack != null)
            Destroy(_currentTrack.gameObject);

        if (trackId >= 0 && trackId < 27)
        {
            _currentTrack = Instantiate(_trackList[trackId], this.transform);
            _currentTrack.transform.SetSiblingIndex(0);
            _trackLength = length;
        }
    }
    
    public override void OnPlayerCarIdChanged(byte playerCarId)
    {
        _currentPlayerCarId = playerCarId;
    }


    public override void OnNumActiveCarsChange(byte numActiveCars)
    {
        foreach (Image obj in _driversIcons)
        {
            Destroy(obj.gameObject);
        }
        _driversIcons.Clear();

        for (byte i = 0; i < numActiveCars; i++)
        {
            _driversIcons.Add(Instantiate(_driverIconPrefab, _driversParent).GetComponent<Image>());
            _driversIcons[i].GetComponentInChildren<TextMeshProUGUI>().text = F1TS_raceNumber(i).ToString();
        }
    }
}
