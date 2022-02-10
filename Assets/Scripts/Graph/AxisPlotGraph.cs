using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Graph;
using Graph.Structs;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class AxisPlotGraph : MonoBehaviour
{
    ShapeRenderer shapeRenderer;
    
    AxisInfo axisInfo;
    GraphInfo graphInfo;
    TextMeshProUGUI textTitle;

    private void Awake()
    {
        shapeRenderer = this.gameObject.AddComponent<ShapeRenderer>();
    }

    public void CreateAxis(AxisInfo _axisInfo, GraphInfo _graphInfo)
    {
        axisInfo = _axisInfo;
        graphInfo = _graphInfo;

        DrawAxis();
    }

    private void DrawAxis()
    {
        Line line;

        //xAxis
        if (axisInfo.XAxis)
        {
            if (axisInfo.centeredXAxis)
                line = new Line(
                    new Vector3(graphInfo.InitialXPos,
                        graphInfo.InitialYPos + ((graphInfo.FinalYPos - graphInfo.InitialYPos)) / 2),
                    new Vector3(graphInfo.FinalXPos,
                        graphInfo.InitialYPos + ((graphInfo.FinalYPos - graphInfo.InitialYPos)) / 2),
                    graphInfo.LineWidth, Color.white);
            else
                line = new Line(new Vector3(graphInfo.InitialXPos, graphInfo.InitialYPos),
                    new Vector3(graphInfo.FinalXPos, graphInfo.InitialYPos), graphInfo.LineWidth,
                    Manager.instance.colorPalette.GraphAxis);

            shapeRenderer.DrawLine(line);
        }

        //yAxis
        line = new Line(new Vector3(graphInfo.InitialXPos, graphInfo.InitialYPos),
            new Vector3(graphInfo.InitialXPos, graphInfo.FinalYPos), graphInfo.LineWidth, Manager.instance.colorPalette.GraphAxis);

        shapeRenderer.DrawLine(line);
    }

  


    public void CreateYTitle(GameObject prefab)
    {
        RectTransform rectTransform = GetComponentInParent<RectTransform>();

        GameObject obj = Instantiate(prefab, this.transform);
        textTitle = obj.GetComponent<TextMeshProUGUI>();

        textTitle.text = axisInfo.YAxisTitle;
        textTitle.color = Manager.instance.colorPalette.GraphAxisTitle;
        int fontSize = (int)(rectTransform.rect.height * axisInfo.YAxisTitleSize * 0.9f);
        textTitle.fontSize = fontSize;
        if (axisInfo.YAxisTitleLeftAligment)
            textTitle.alignment = TextAlignmentOptions.Left;
        else
            textTitle.alignment = TextAlignmentOptions.Right;

        RectTransform textRectTransform = obj.GetComponent<RectTransform>();
        textRectTransform.sizeDelta = new Vector2(fontSize * axisInfo.YAxisTitle.Length, fontSize * 1.1f);
        textRectTransform.anchoredPosition = new Vector2((graphInfo.InitialXPos + axisInfo.YAxisTitleMargin) * rectTransform.rect.width, 
            (((graphInfo.FinalYPos - graphInfo.InitialYPos) / 2) + graphInfo.InitialYPos) * rectTransform.rect.height);


        Invoke("UpdateBackGround", 0.01f);
    }

    private void UpdateBackGround()
    {
        Image backGround = textTitle.gameObject.GetComponentInChildren<Image>();
        backGround.enabled = true;
        backGround.color = Manager.instance.colorPalette.GraphTitleBackGround;
        textTitle.GetComponent<RectTransform>().sizeDelta = textTitle.GetRenderedValues(true);
    }

    public void CreateLabels(GameObject prefab)
    {
        RectTransform rectTransform = GetComponentInParent<RectTransform>();

        float increase = (graphInfo.FinalYPos - graphInfo.InitialYPos) / (axisInfo.YAxisLabels.Count - 1);

        for (int i = 0; i < axisInfo.YAxisLabels.Count; i++)
        {
            GameObject obj = Instantiate(prefab, this.transform);
            TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
            RectTransform textRectTransform = obj.GetComponent<RectTransform>();

            text.text = axisInfo.YAxisLabels[i];
            text.color = Manager.instance.colorPalette.GraphAxisLabel;
            int fontSize = (int)(rectTransform.rect.height * axisInfo.YAxisLabelSize * 0.9f);
            text.fontSize = fontSize;
            if (axisInfo.YAxisLabelLeftAligment)
                text.alignment = TextAlignmentOptions.Left;
            else
            {
                text.alignment = TextAlignmentOptions.Right;
                textRectTransform.pivot = new Vector2(1,0.5f);
            }

            textRectTransform.sizeDelta = new Vector2(fontSize * axisInfo.YAxisLabels[i].Length, fontSize * 1.1f);

            Vector2 anchorPos = new Vector2((graphInfo.InitialXPos + axisInfo.YAxisLabelMargin) * rectTransform.rect.width,
                (graphInfo.InitialYPos + increase *i) * rectTransform.rect.height);

            textRectTransform.anchoredPosition = anchorPos;
        }
    }

    public void CreateLabelDividingLines()
    {
        float increase = (graphInfo.FinalYPos - graphInfo.InitialYPos) / (axisInfo.YAxisLabels.Count - 1);
        for (int i=0; i < axisInfo.YAxisLabels.Count; i++)
        {
            Vector3 A = new Vector3(graphInfo.InitialXPos + graphInfo.LineWidth / 2, graphInfo.InitialYPos + increase * i);
            Vector3 B = new Vector3(graphInfo.InitialXPos + graphInfo.LineWidth / 2 + axisInfo.YAxisLabelDividingLinesLength, graphInfo.InitialYPos + increase * i);
            Line line = new Line(A, B, axisInfo.YAxisLabelDividingLinesWidth, Manager.instance.colorPalette.GraphAxis);
            shapeRenderer.DrawLine(line);
        }
    }

    public void CreateInterLabelDividingLines()
    {
        float increase = (graphInfo.FinalYPos - graphInfo.InitialYPos) / (axisInfo.YAxisLabels.Count - 1);
        for (int i = 0; i < axisInfo.YAxisLabels.Count - 1; i++)
        {
            float ini = graphInfo.InitialYPos + increase * i;
            float end = graphInfo.InitialYPos + increase * (i+1);

            float smallIncrease = increase / axisInfo.YAxisInterLabelDividingLinesCount;

            for (int j=1; j < axisInfo.YAxisInterLabelDividingLinesCount; j++)
            {
                Vector3 A = new Vector3(graphInfo.InitialXPos + graphInfo.LineWidth / 2, graphInfo.InitialYPos + (increase * i) + (smallIncrease * j));
                Vector3 B = new Vector3(graphInfo.InitialXPos + graphInfo.LineWidth / 2 + axisInfo.YAxisInterLabelDividingLinesLength,
                    graphInfo.InitialYPos + (increase * i) + (smallIncrease * j));
                Line line = new Line(A, B, axisInfo.YAxisInterLabelDividingLinesWidth, Manager.instance.colorPalette.GraphAxis);
                shapeRenderer.DrawLine(line);
            }
        }
    }

    public void AddRandomPoint()
    {
        Line line = new Line(new Vector3(Random.Range(0.0f,1.0f), Random.Range(0.0f, 1.0f)),
            new Vector3(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)), graphInfo.LineWidth, Color.white);
        shapeRenderer.DrawLine(line);

    }
}

