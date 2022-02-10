using System;
using System.Collections.Generic;
using Graph.Structs;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Graph
{
	public class GraphRenderer : MonoBehaviour
	{
//		public ShapeRenderer graph;
//		public GraphInfo graphInfo;
//		public Text TextPrefab;


//		private RectTransform _rectTransform;

//		private void Awake()
//		{
//			graph = gameObject.AddComponent<ShapeRenderer>();
//			_rectTransform = GetComponent<RectTransform>();
//		}


//		public void DrawGraph()
//		{
//			DrawAxis();
//			DrawGraphText();
//			if (graphInfo.XAxis)
//				CreateText(graphInfo.XAxisTitle);
//			CreateText(graphInfo.YAxisTitle);

//			for (int i = 0; i < graphInfo.Points.Count - 1; i++)
//			{
//				if (graphInfo.Expand)
//					DrawExpandGraphLines(graphInfo.Points[i], graphInfo.Points[i + 1]);
//				else
//					DrawTwoCoordinatesGraphLines(graphInfo.Points[i], graphInfo.Points[i + 1]);
//			}
//		}


//		public void ClearGraph()
//		{
//			graphInfo.ClearPoint();
//			graph.Clear();
//			DrawAxis();
//		}


//		public void AddPoint(Vector2 point)
//		{
//			if (graphInfo.Points.Count == 0)
//			{
//				graphInfo.AddPoint(new Vector2(0, point.y));
//			}

//			graphInfo.AddPoint(point);

//			if (graphInfo.Expand)
//				DrawExpandGraphLines(graphInfo.Points[graphInfo.Points.Count - 2],
//					graphInfo.Points[graphInfo.Points.Count - 1]);
//			else
//				DrawTwoCoordinatesGraphLines(graphInfo.Points[graphInfo.Points.Count - 2],
//					graphInfo.Points[graphInfo.Points.Count - 1]);
//		}

//		private void DrawExpandGraphLines(Vector2 A, Vector2 B)
//		{
//			float rangedistY = graphInfo.FinalYPos - graphInfo.InitialYPos;
//			float stepX = (graphInfo.FinalXPos - graphInfo.InitialXPos) / (graphInfo.Points.Count - 1);


//			Line line = new Line(
//				new Vector3(graphInfo.InitialXPos + stepX * graphInfo.Points.Count - 1,
//					graphInfo.InitialYPos + (A.y /
//					                         (graphInfo.MaxYValue - graphInfo.MinYValue)) * rangedistY),
//				new Vector3(graphInfo.InitialXPos + stepX * (graphInfo.Points.Count),
//					graphInfo.InitialYPos + (B.y /
//					                         (graphInfo.MaxYValue - graphInfo.MinYValue)) * rangedistY),
//				graphInfo.LineWidth, graphInfo.Color);
//			graph.DrawLine(line);
//		}

//		private void DrawTwoCoordinatesGraphLines(Vector2 A, Vector2 B)
//		{
//			float rangedistY = graphInfo.FinalYPos - graphInfo.InitialYPos;

//			Line line = new Line(
//				new Vector3(GetXCoordinatesForPoitn(A),
//					graphInfo.InitialYPos + (A.y /
//					                         (graphInfo.MaxYValue - graphInfo.MinYValue)) * rangedistY),
//				new Vector3(GetXCoordinatesForPoitn(B),
//					graphInfo.InitialYPos + (B.y /
//					                         (graphInfo.MaxYValue - graphInfo.MinYValue)) * rangedistY),
//				graphInfo.LineWidth, graphInfo.Color);
//			graph.DrawLine(line);
//		}

//		private float GetXCoordinatesForPoitn(Vector2 point)
//		{
//			//print("New point: " + point);
//			float xValueRange = (graphInfo.MaxXValue - graphInfo.MinXValue);
//			float currentValuePos = (point.x / xValueRange) * (graphInfo.FinalXPos - graphInfo.InitialXPos);
//			//print("Coordinate point: " + graphInfo.InitialXPos + currentValuePos);
//			return graphInfo.InitialXPos + currentValuePos;
//		}

//		public void DrawAxis()
//		{
//			Line line;

//			//xAxis
//			if (graphInfo.XAxis)
//			{
//				if (graphInfo.CenteredXAxis)
//					line = new Line(
//						new Vector3(graphInfo.InitialXPos,
//							graphInfo.InitialYPos + ((graphInfo.FinalYPos - graphInfo.InitialYPos)) / 2),
//						new Vector3(graphInfo.FinalXPos,
//							graphInfo.InitialYPos + ((graphInfo.FinalYPos - graphInfo.InitialYPos)) / 2),
//						graphInfo.LineWidth, graphInfo.AxisColor);
//				else
//					line = new Line(new Vector3(graphInfo.InitialXPos, graphInfo.InitialYPos),
//						new Vector3(graphInfo.FinalXPos, graphInfo.InitialYPos), graphInfo.LineWidth,
//						graphInfo.AxisColor);

//				//graphAxis.DrawLine(line);
//			}

//			//yAxis
//			line = new Line(new Vector3(graphInfo.InitialXPos, graphInfo.InitialYPos),
//				new Vector3(graphInfo.InitialXPos, graphInfo.FinalYPos), graphInfo.LineWidth, graphInfo.AxisColor);

//			graph.DrawLine(line);
//		}

//		private void DrawGraphText()
//		{
//			if (graphInfo.XAxis)
//			{
//				foreach (TextInfo text in graphInfo.XAxisTexts)
//					CreateText(text);
//			}

//			foreach (TextInfo text in graphInfo.YAxisTexts)
//				CreateText(text);
//		}

//		private void CreateText(TextInfo text)
//		{
//			Text newText = Instantiate(TextPrefab, this.transform);
//			newText.text = text.Text;
//			newText.color = text.Color;
//			newText.fontSize = (int)(_rectTransform.rect.height * text.TextSize * 0.9f);


//			RectTransform newTextRect = newText.GetComponent<RectTransform>();
//			Rect rect = _rectTransform.rect;
//			newTextRect.anchoredPosition = new Vector2(rect.width * text.XPos,
//				rect.height * text.YPos);
//			newTextRect.sizeDelta = new Vector2(rect.width * text.Size.x,
//				rect.height * text.Size.y);
//			newTextRect.Rotate(new Vector3(0, 0, 1), text.Rotation);
//		}
	}
}