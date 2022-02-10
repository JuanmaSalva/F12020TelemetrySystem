// using System;
// using System.Collections.Generic;
// using System.Xml.Schema;
// using Graph.Structs;
// using UnityEngine;
// using UnityEngine.UI;
// using Random = UnityEngine.Random;
//
// namespace Graph
// {
// 	public class GraphTest : MonoBehaviour
// 	{
// 		private GraphRenderer _graphRenderer;
// 		public List<Vector2> values;
//
// 		private GraphInfo _graphInfo;
//
// 		void Start()
// 		{
// 			_graphRenderer = GetComponent<GraphRenderer>();
// 			_graphInfo = new GraphInfo(20, 50, 0.001f, Color.green,
// 				0.25f, 0.75f, 0.05f, 0.95f);
//
// 			foreach (Vector2 value in values)
// 				_graphInfo.AddPoint(value.x, value.y);
//
//
// 			_graphInfo.CenteredXAxis = false;
//
// 			_graphInfo.XAxisEdgeTextMargin = 0.01f;
// 			_graphInfo.YAxisEdgeTextMargin = 0.02f;
//
// 			_graphInfo.AddXTitle("Meters", 0.055f, 0.05f, Color.white);
// 			_graphInfo.AddYTitle("Speed", 0.035f, 0.05f, Color.white);
//
// 			_graphInfo.AddXAxisTexts(new string[] { "0", "200", "400", "600", "800", "1000", "1200", "1400", "1600" },
// 				0.025f, 0.02f, Color.white);
// 			_graphInfo.AddYAxisTexts(new string[] { "0", "100", "200", "300" }, 0.025f, 0.01f, Color.white);
//
// 			_graphInfo.Expand = true;
//
// 			_graphRenderer.DrawGraph(_graphInfo);
// 			_graphInfo.AddPoint(0, 20);
// 			_graphRenderer.UpdateGraph(_graphInfo, 0);
// 		}
//
//
// 		private void Update()
// 		{
// 			/*float value = Random.Range(0.0f, 50.0f);
// 			graphInfo.AddPoint(value);
// 			_graphRenderer.UpdateGraph(graphInfo, 0);*/
// 		}
// 	}
// }