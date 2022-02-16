using Graph;
using Graph.Structs;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace F1TS
{
    public class DynamicPlotGraph : MonoBehaviour
    {
        [DllImport("F12020Telemetry")]
        protected static extern byte F1TS_playerCarIndex();


        protected GraphInfo graphInfo;
        protected ShapeRenderer shapeRenderer;

        protected byte playerCarId;

        protected List<Vector2> lines;
        protected float initialYPos = 0.0f;

        private StaticPlotGraph staticPlotGraph;

        public void SetGraphInfo(GraphInfo _graphInfo)
        {
            graphInfo = _graphInfo;
        }

        public void SetPlayerCarId(byte newPlayerCarId)
        {
            playerCarId = newPlayerCarId;
        }

        
        void Start()
        {
            Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
            shapeRenderer = this.gameObject.AddComponent<ShapeRenderer>();
            lines = new List<Vector2>();
            Init();
        }


        protected virtual void Init() { }

        public void SetStaticPlotGraph(StaticPlotGraph newStaticPlotGraph)
        {
            staticPlotGraph = newStaticPlotGraph;
            staticPlotGraph.graphInfo = graphInfo;
        }


        public virtual void UpdateGraph(float currentLapDistance){}


        protected void CalculateAndDrawLine(Vector2 A, Vector2 B)
        {
            float rangedistY = graphInfo.FinalYPos - graphInfo.InitialYPos;

            Line line = new Line(
                new Vector3(GetXCoordinatesForPoitn(A),
                    graphInfo.InitialYPos + (A.y /
                                             (graphInfo.MaxYValue - graphInfo.MinYValue)) * rangedistY + initialYPos),
                new Vector3(GetXCoordinatesForPoitn(B),
                    graphInfo.InitialYPos + (B.y /
                                             (graphInfo.MaxYValue - graphInfo.MinYValue)) * rangedistY + initialYPos),
                graphInfo.LineWidth, Manager.instance.colorPalette.GraphCurrentLap);
            shapeRenderer.DrawLine(line);
        }

        private float GetXCoordinatesForPoitn(Vector2 point)
        {
            float xValueRange = (graphInfo.MaxXValue - graphInfo.MinXValue);
            float currentValuePos = (point.x / xValueRange) * (graphInfo.FinalXPos - graphInfo.InitialXPos);
            return graphInfo.InitialXPos + currentValuePos;
        }

        public void NewLapStarted()
        {
            staticPlotGraph.PlotGraph(shapeRenderer.GetVertecies(), shapeRenderer.GetTriangles(), Manager.instance.colorPalette.GraphLastLap);
            shapeRenderer.Clear();
            lines.Clear();
        }

        public List<UIVertex> GetShapeRendererVertecies()
        {
            return shapeRenderer.GetVertecies();
        }

        public List<Vector3Int> GetShapeRendererTriangles()
        {
            return shapeRenderer.GetTriangles();
        }

        public void ChangeTrackLength(short length)
        {
            staticPlotGraph.graphInfo.MaxXValue = length;
            graphInfo.MaxXValue = length;
        }
    }
}
