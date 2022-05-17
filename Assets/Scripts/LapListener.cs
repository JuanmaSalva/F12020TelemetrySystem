using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LapListener
{
    public virtual void OnFastestLap(int time, bool personal) { }
    
    public virtual void OnFastestSector(int time, int sector, bool personal) { }
}
