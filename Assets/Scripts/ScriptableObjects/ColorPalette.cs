using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "ColorPalette", menuName = "General/ColorPalette", order = 1)]
public class ColorPalette : ScriptableObject
{
    [TabGroup("Panles")] public Color PanelTitle;
    [TabGroup("Panles")] public Color PanelInfo;


    [TabGroup("Graphs")] public Color GraphAxis;
    [TabGroup("Graphs")] public Color GraphAxisTitle;
    [TabGroup("Graphs")] public Color GraphAxisLabel;
    [TabGroup("Graphs")] public Color GraphCurrentLap;
    [TabGroup("Graphs")] public Color GraphLastLap;
    [TabGroup("Graphs")] public Color GraphBestLap;
    [TabGroup("Graphs")] public Color GraphTitleBackGround;


    [TabGroup("CarStatus")] public Color GreenStatus;
    [TabGroup("CarStatus")] public Color RedStatus;

    [TabGroup("Sectors")] public Color NormalTime;
    [TabGroup("Sectors")] public Color PersonalBestTime;
    [TabGroup("Sectors")] public Color OverallBestTime;
}
