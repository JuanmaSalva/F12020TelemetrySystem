using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph.Structs;
using Sirenix.OdinInspector;
using System.Runtime.InteropServices;

namespace F1TS
{    
    public class Graph : TelemetryListener
    {
        [Title("Speed")]
        [TabGroup("Infos")]
        public AxisInfo speedAxisInfo;
        [TabGroup("Infos")]
        public GraphInfo speedGraphInfo;

        [Title("Stearing")]
        [TabGroup("Infos")]
        public AxisInfo stearingAxisInfo;
        [TabGroup("Infos")]
        public GraphInfo stearingGraphInfo;

        [Title("Throttle")]
        [TabGroup("Infos")]
        public AxisInfo throttleAxisInfo;
        [TabGroup("Infos")]
        public GraphInfo throttleGraphInfo;

        [Title("Brake")]
        [TabGroup("Infos")]
        public AxisInfo brakeAxisInfo;
        [TabGroup("Infos")]
        public GraphInfo brakeGraphInfo;

        [Title("Gear")]
        [TabGroup("Infos")]
        public AxisInfo gearAxisInfo;
        [TabGroup("Infos")]
        public GraphInfo gearGraphInfo;

        [Title("DRS")]
        [TabGroup("Infos")]
        public AxisInfo drsAxisInfo;
        [TabGroup("Infos")]
        public GraphInfo drsGraphInfo;

        [TabGroup("Prefabs")]
        public GameObject graphObjPrefab;
        [TabGroup("Prefabs")] 
        public GameObject textObjPrefab;


        [TabGroup("Parents")]
        public Transform dynamicGraphsParent;
        [TabGroup("Parents")]
        public Transform staticGraphsParent;
        [TabGroup("Parents")]
        public Transform axisGraphsParent;

        private List<AxisPlotGraph> axisList;
        private List<StaticPlotGraph> staticGraphList; //for saved data
        private List<DynamicPlotGraph> dynamicPlotGraphList;
        
        void Start()
        {
            axisList = new List<AxisPlotGraph>();
            staticGraphList = new List<StaticPlotGraph>();
            dynamicPlotGraphList = new List<DynamicPlotGraph>();

            InstantiateAxis(speedAxisInfo, speedGraphInfo, "SpeedAxis");
            InstantiateDynamicPlotGraph<DynamicSpeedGraph>(speedGraphInfo, "DynamicSpeedGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticSpeedGraph");

            InstantiateAxis(stearingAxisInfo, stearingGraphInfo, "StearingAxis");
            InstantiateDynamicPlotGraph<DynamicStearingGraph>(stearingGraphInfo, "DynamicStearingGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticStearingGraph");

            InstantiateAxis(throttleAxisInfo, throttleGraphInfo, "ThrottleAxis");
            InstantiateDynamicPlotGraph<DynamicThrottleGraph>(throttleGraphInfo, "DynamicThrottleGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticSThrottleGraph");

            InstantiateAxis(brakeAxisInfo, brakeGraphInfo, "BrakeAxis");
            InstantiateDynamicPlotGraph<DynamicBrakeGraph>(brakeGraphInfo, "DynamicBrakeGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticBrakeGraph");

            InstantiateAxis(gearAxisInfo, gearGraphInfo, "GearAxis");
            InstantiateDynamicPlotGraph<DynamicGearGraph>(gearGraphInfo, "DynamicGearGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticGearGraph");

            InstantiateAxis(drsAxisInfo, drsGraphInfo, "DRSAxis");
            InstantiateDynamicPlotGraph<DynamicDRSGraph>(drsGraphInfo, "DynamicDRSGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticDRSGraph");

            InitDynamicPlotGraphs();
            EventManager.instance.AddListener(this);
        }

        private void InstantiateAxis(AxisInfo axisInfo, GraphInfo graphInfo, string name)
        {
            GameObject obj = Instantiate(graphObjPrefab, axisGraphsParent);
            obj.name = name;
            AxisPlotGraph axis = obj.AddComponent<AxisPlotGraph>();
            axis.CreateAxis(axisInfo, graphInfo);
            if (axisInfo.YAxisTitle != "")
                axis.CreateYTitle(textObjPrefab);
            if (axisInfo.YAxisLabels.Count > 0)
                axis.CreateLabels(textObjPrefab);
            if (axisInfo.YAxisLabelDividingLines)
                axis.CreateLabelDividingLines();
            if (axisInfo.YAxisInterLabelDividingLines)
                axis.CreateInterLabelDividingLines();
            axisList.Add(axis);
        }


        private void InstantiateDynamicPlotGraph<T>(GraphInfo graphInfo, string name) where T : DynamicPlotGraph
        {
            GameObject obj = Instantiate(graphObjPrefab, dynamicGraphsParent);
            obj.name = name;
            DynamicPlotGraph dynamicPlotGraph = obj.AddComponent<T>();
            dynamicPlotGraph.SetGraphInfo(graphInfo);
            dynamicPlotGraphList.Add(dynamicPlotGraph);
        }

        private void InstantiateStaticPlotGraph(DynamicPlotGraph dynamicPlotGraph, string name)
        {
            GameObject obj = Instantiate(graphObjPrefab, staticGraphsParent);
            obj.name = name;
            StaticPlotGraph staticPlotGraph = obj.AddComponent<StaticPlotGraph>();
            dynamicPlotGraph.SetStaticPlotGraph(staticPlotGraph);
        }






        [DllImport("F12020Telemetry")]
        private static extern byte F1TS_playerCarIndex();
        [DllImport("F12020Telemetry")]
        private static extern float F1TS_lapDistance(byte carId);
        [DllImport("F12020Telemetry")]
        private static extern short F1TS_trackLength();

        [DllImport("F12020Telemetry")]
        private static extern byte F1TS_currentLapNum(byte carId);



        private float currentLapDistance;
        private float maxDistance = 0;
        private short trackLength;
        private byte playerCarId;

        private void InitDynamicPlotGraphs()
        {
            playerCarId = F1TS_playerCarIndex();
            //TODO esto se deberia de mirar mas a menudo
            foreach (DynamicPlotGraph dg in dynamicPlotGraphList) 
                dg.SetPlayerCarId(playerCarId);
        }


        private void Update()
        {
            if (trackLength > 0)
            {
                currentLapDistance = F1TS_lapDistance(playerCarId);
                if (currentLapDistance >= 0)
                {
                    if (maxDistance < currentLapDistance)
                    {
                        maxDistance = currentLapDistance;
                        foreach (DynamicPlotGraph dg in dynamicPlotGraphList)
                            dg.UpdateGraph(currentLapDistance);
                    }
                }
            }
        }


        public override void OnNewLap(int lap)
        {
            maxDistance = 0;

            foreach (DynamicPlotGraph dg in dynamicPlotGraphList)
                dg.NewLapStarted();
        }

        public override void OnFastestLap(float time)
        {
            Debug.Log("NEW FASTEST LAP, dale carla");
        }

        public override void OnNewTrack(short length, int trackId)
        {
            trackLength = length;
            foreach (DynamicPlotGraph dg in dynamicPlotGraphList)
                dg.ChangeTrackLength(length);
        }
    }
}