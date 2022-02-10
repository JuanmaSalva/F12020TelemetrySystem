using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph.Structs;
using Sirenix.OdinInspector;


namespace Graph.Structs
{
    [CreateAssetMenu(fileName = "AxisInfo", menuName = "Graphs/AxisInfo", order = 1)]
    public class AxisInfo : ScriptableObject
    {
        [TabGroup("X_Axis")]
        [ShowInInspector, TabGroup("X_Axis")]
        public bool XAxis;
        [ShowInInspector, TabGroup("X_Axis")]
        public bool centeredXAxis;
        [ShowInInspector, TabGroup("X_Axis")]
        public string XAxisTitle;
        [ShowInInspector, TabGroup("X_Axis")]
        public List<string> XAxisLabels;
        [ShowInInspector, TabGroup("X_Axis")]
        public float XAxisEdgeTextMargin;
        [ShowInInspector, TabGroup("X_Axis")]
        public float XAxisTextSize;


        [TabGroup("Y_Axis")]

        [Title("Title")]
        [ShowInInspector, TabGroup("Y_Axis")]
        public string YAxisTitle;
        [ShowInInspector, TabGroup("Y_Axis")]
        public float YAxisTitleMargin;
        [ShowInInspector, TabGroup("Y_Axis")]
        public bool YAxisTitleLeftAligment;
        [ShowInInspector, TabGroup("Y_Axis")]
        public float YAxisTitleSize;

        [Title("Labels")]
        [ShowInInspector, TabGroup("Y_Axis")]
        public List<string> YAxisLabels;
        [ShowInInspector, TabGroup("Y_Axis")]
        public float YAxisLabelMargin;
        [ShowInInspector, TabGroup("Y_Axis")]
        public bool YAxisLabelLeftAligment;
        [ShowInInspector, TabGroup("Y_Axis")]
        public float YAxisLabelSize;

        [Title("Labels")]
        [ShowInInspector, TabGroup("Y_Axis"), Tooltip("Lines that divide the axis")]
        public bool YAxisLabelDividingLines;
        [ShowInInspector, TabGroup("Y_Axis")]
        public float YAxisLabelDividingLinesLength;
        [ShowInInspector, TabGroup("Y_Axis")]
        public float YAxisLabelDividingLinesWidth;
        [ShowInInspector, TabGroup("Y_Axis"), Tooltip("Lines that divide the space between labels")]
        public bool YAxisInterLabelDividingLines;
        [ShowInInspector, TabGroup("Y_Axis")]
        public int YAxisInterLabelDividingLinesCount;
        [ShowInInspector, TabGroup("Y_Axis")]
        public float YAxisInterLabelDividingLinesLength;
        [ShowInInspector, TabGroup("Y_Axis")]
        public float YAxisInterLabelDividingLinesWidth;
    }
}
