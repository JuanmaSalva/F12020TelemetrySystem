using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph.Structs;
using Sirenix.OdinInspector;
using System.Runtime.InteropServices;
using System.Threading;

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
        [TabGroup("Parents")]
        public Transform bestLapGraphParent;


        [DllImport("F12020Telemetry")]
        private static extern byte F1TS_playerCarIndex();
        [DllImport("F12020Telemetry")]
        private static extern float F1TS_lapDistance(byte carId);
        [DllImport("F12020Telemetry")]
        private static extern short F1TS_trackLength();

        [DllImport("F12020Telemetry")]
        private static extern byte F1TS_currentLapNum(byte carId);



        private List<AxisPlotGraph> axisList;
        private List<DynamicPlotGraph> dynamicPlotGraphList;
        private StaticPlotGraph staticBestLapGraph; //for saved data


        private float currentLapDistance;
        private float maxDistance = 0;
        private short trackLength;
        private sbyte trackId = -1;
        private byte currentPlayerCarId = 0;



        void Start()
        {
            axisList = new List<AxisPlotGraph>();
            dynamicPlotGraphList = new List<DynamicPlotGraph>();

            InstantiateAxis(speedAxisInfo, speedGraphInfo, "SpeedAxis");
            InstantiateDynamicPlotGraph<DynamicSpeedGraph>(speedGraphInfo, "DynamicSpeedGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticSpeedGraph", staticGraphsParent);

            InstantiateAxis(stearingAxisInfo, stearingGraphInfo, "StearingAxis");
            InstantiateDynamicPlotGraph<DynamicStearingGraph>(stearingGraphInfo, "DynamicStearingGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticStearingGraph", staticGraphsParent);

            InstantiateAxis(throttleAxisInfo, throttleGraphInfo, "ThrottleAxis");
            InstantiateDynamicPlotGraph<DynamicThrottleGraph>(throttleGraphInfo, "DynamicThrottleGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticSThrottleGraph", staticGraphsParent);

            InstantiateAxis(brakeAxisInfo, brakeGraphInfo, "BrakeAxis");
            InstantiateDynamicPlotGraph<DynamicBrakeGraph>(brakeGraphInfo, "DynamicBrakeGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticBrakeGraph", staticGraphsParent);

            InstantiateAxis(gearAxisInfo, gearGraphInfo, "GearAxis");
            InstantiateDynamicPlotGraph<DynamicGearGraph>(gearGraphInfo, "DynamicGearGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticGearGraph", staticGraphsParent);

            InstantiateAxis(drsAxisInfo, drsGraphInfo, "DRSAxis");
            InstantiateDynamicPlotGraph<DynamicDRSGraph>(drsGraphInfo, "DynamicDRSGraph");
            InstantiateStaticPlotGraph(dynamicPlotGraphList[dynamicPlotGraphList.Count - 1], "LastLapStaticDRSGraph", staticGraphsParent);

            ChangePlayerCarIndxOnDynamicGraphs();
            EventManager.instance.AddListener(this);
            Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);


            staticBestLapGraph = InstantiateStaticPlotGraph(null, "BestLap", bestLapGraphParent);
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

        private StaticPlotGraph InstantiateStaticPlotGraph(DynamicPlotGraph dynamicPlotGraph, string name, Transform parent)
        {
            GameObject obj = Instantiate(graphObjPrefab, parent);
            obj.name = name;
            StaticPlotGraph staticPlotGraph = obj.AddComponent<StaticPlotGraph>();
            if(dynamicPlotGraph != null) 
                dynamicPlotGraph.SetStaticPlotGraph(staticPlotGraph);
            return staticPlotGraph;
        }







        private void ChangePlayerCarIndxOnDynamicGraphs()
        {
            foreach (DynamicPlotGraph dg in dynamicPlotGraphList)
                dg.SetPlayerCarId(currentPlayerCarId);
        }


        private void Update()
        {
            if (trackLength > 0)
            {
                currentLapDistance = F1TS_lapDistance(currentPlayerCarId);
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
            //Debug.Log("NEW FASTEST LAP, dale carla");

            List<UIVertex> v = new List<UIVertex>();
            List<Vector3Int> t = new List<Vector3Int>();
            int initialTris = 0;
            foreach (DynamicPlotGraph dg in dynamicPlotGraphList)
            {
                v.AddRange(dg.GetShapeRendererVertecies());
                t.AddRange(dg.GetShapeRendererTrianglesOffset(initialTris));
                initialTris = v.Count;
            }

            LapGraphData lapGraphData = new LapGraphData(trackId, time, v, t);
            print("Objeto creado, listo para guardar");
            //Thread thread = new Thread(SaveAndShowFastestLap);
            //thread.Start(lapGraphData);
            SaveAndShowFastestLap(lapGraphData);
        }

        private void SaveAndShowFastestLap(LapGraphData lapGraphData)
        {
            //LapGraphData aux = lapGraphData as LapGraphData;
            Thread saveThread = new Thread(
                () => SaveSystem.SaveObject("SavedLapTrack" + trackId.ToString() + ".track", lapGraphData));
            saveThread.Start();
            
            staticBestLapGraph.PlotGraph(lapGraphData.GetVertecies(), lapGraphData.GetTriangles(), Manager.instance.colorPalette.GraphBestLap);
        }


        public override void OnNewTrack(short length, sbyte trackId)
        {
            this.trackId = trackId;
            trackLength = length;
            foreach (DynamicPlotGraph dg in dynamicPlotGraphList)
                dg.ChangeTrackLength(length);
        }

        public override void OnPlayerCarIdChanged(byte playerCarId)
        {
            currentPlayerCarId = playerCarId;
            ChangePlayerCarIndxOnDynamicGraphs();
        }

        public override void OnLapCleared()
        {
            foreach (DynamicPlotGraph dg in dynamicPlotGraphList)
                dg.Clear();
        }
        

        [Button]
        private void LoadTrack(sbyte trackId)
        {
            Debug.Log("Track to load: " + trackId);

            LapGraphData lapGraphData = SaveSystem.LoadObject("SavedLapTrack" + trackId.ToString() + ".track") as LapGraphData;
            if(lapGraphData != null) 
                Debug.Log("Vuelta cargada");

            staticBestLapGraph.PlotGraph(lapGraphData.GetVertecies(), lapGraphData.GetTriangles(), Manager.instance.colorPalette.GraphBestLap);
        }

    }
}