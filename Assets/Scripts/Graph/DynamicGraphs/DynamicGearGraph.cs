using F1TS;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DynamicGearGraph : DynamicPlotGraph
{
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_gear(byte carId);

    protected override void Init()
    {

    }


    public override void UpdateGraph(float currentLapDistance)
    {
        if (lines.Count == 0)
            lines.Add(new Vector2(0, (float)F1TS_gear(playerCarId)));

        Vector2 aux = new Vector2(currentLapDistance, (float)F1TS_gear(playerCarId));
        lines.Add(aux);
        CalculateAndDrawLine(lines[lines.Count - 2], lines[lines.Count - 1]);
    }
}
