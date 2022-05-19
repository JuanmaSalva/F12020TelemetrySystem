using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILapListener
{
    public void OnFastestLap(int time, bool personal) { }

    public void OnFastestSector(int time, int sector, bool personal) { }
}
