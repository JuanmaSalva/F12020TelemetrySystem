using Graph;
using Graph.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace F1TS
{
    public class StaticPlotGraph : MonoBehaviour
    {
        public GraphInfo graphInfo { get; set; }
        private ShapeRenderer shapeRenderer;


        private void Start()
        {
            shapeRenderer = this.gameObject.AddComponent<ShapeRenderer>();
            RectTransform thisRectTrans = GetComponent<RectTransform>();
        }


        public void PlotGraph(List<UIVertex> vertexs, List<Vector3Int> triangles, Color color)
        {
            for(int i = 0; i < vertexs.Count; i++)
            {
                UIVertex v = vertexs[i];
                v.color = color;
                v.position.z = 1;
                vertexs[i] = v;
            }
            shapeRenderer.SetInfo(vertexs, triangles);
        }
    }
}