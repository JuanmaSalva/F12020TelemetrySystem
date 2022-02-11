using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelemetryListener : MonoBehaviour
{

    public virtual void OnPlayerCarIdChanged(byte playerCarId) { }

    public virtual void OnNewLap(int lap) { }

    public virtual void OnFastestLap(float time) { }

    public virtual void OnNewTrack(short length, sbyte trackId) { }


}
