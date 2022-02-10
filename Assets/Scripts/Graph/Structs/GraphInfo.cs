using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace Graph.Structs
{

    [CreateAssetMenu(fileName = "GraphInfo", menuName = "Graphs/GraphInfo", order = 2)]
    public class GraphInfo : ScriptableObject
    {
        const float TEXT_VERTICAL_MARGIN = 1.2f;

        [HideInInspector]
        public List<Vector2> Points;

        [TabGroup("Min-Max values")]
        [ShowInInspector, TabGroup("Min-Max values")]
        public float MinYValue;
        [ShowInInspector, TabGroup("Min-Max values")]
        public float MaxYValue;
        [ShowInInspector, TabGroup("Min-Max values")]
        public float MinXValue;
        [ShowInInspector, TabGroup("Min-Max values")]
        public float MaxXValue;
        [ShowInInspector, TabGroup("Min-Max values")]
        public float LineWidth;

        [TabGroup("InitialPosition")]
        [ShowInInspector, TabGroup("InitialPosition"), Range(0.0f, 1.0f)]
        public float InitialYPos;
        [ShowInInspector, TabGroup("InitialPosition"), Range(0.0f, 1.0f)]
        public float FinalYPos;
        [ShowInInspector, TabGroup("InitialPosition"), Range(0.0f, 1.0f)]
        public float InitialXPos;
        [ShowInInspector, TabGroup("InitialPosition"), Range(0.0f, 1.0f)]
        public float FinalXPos;

       

        public bool Expand = false;


        public void AddPoint(float yValue)
        {
            Points.Add(new Vector2(0, yValue));
        }

        public void AddPoint(Vector2 point)
        {
            Points.Add(point);
        }

        public void ClearPoint()
        {
            Points.Clear();
            Points = new List<Vector2>();
        }

        //public void AddYTitle(string text, float xMargin, float textSize, Color color)
        //{
        //    YAxisTitle = new TextInfo(text, InitialXPos - xMargin, InitialYPos + ((FinalYPos - InitialYPos) / 2), textSize, 90,
        //        new Vector2((FinalYPos - InitialYPos), textSize * TEXT_VERTICAL_MARGIN), color);
        //}

        //public void AddXTitle(string text, float yMargin, float textSize, Color color)
        //{

        //    XAxisTitle = new TextInfo(text, InitialXPos + ((FinalXPos - InitialXPos) / 2), InitialYPos - yMargin, textSize, 0,
        //        new Vector2((FinalYPos - InitialYPos), textSize * TEXT_VERTICAL_MARGIN), color);
        //}


        //public void AddYAxisTexts(string[] texts, float textSize, float margin, Color color)
        //{
        //    YAxisTexts.Clear();
        //    float separation = GetYAxisSeparation(texts.Length);
        //    Vector2 textObjSize = new Vector2((FinalYPos - InitialYPos) / texts.Length, textSize * TEXT_VERTICAL_MARGIN);
        //    float xPos = InitialXPos - margin;

        //    for (int i = 0; i < texts.Length; i++)
        //    {
        //        float yPos = separation * i + InitialYPos;

        //        YAxisTexts.Add(new TextInfo(texts[i], xPos,
        //            yPos + YAxisEdgeTextMargin, textSize, 90, textObjSize, color));
        //    }
        //}

        //public void AddXAxisTexts(string[] texts, float textSize, float margin, Color color)
        //{
        //    XAxisTexts.Clear();
        //    float separation = GetXAxisSeparation(texts.Length);
        //    Vector2 textObjSize = new Vector2((FinalXPos - InitialXPos) / texts.Length, textSize * TEXT_VERTICAL_MARGIN);
        //    float yPos = GetTextYPosForXAxisTexts();

        //    for (int i = 0; i < texts.Length; i++)
        //    {
        //        float xPos = separation * i + InitialXPos + XAxisEdgeTextMargin;

        //        XAxisTexts.Add(new TextInfo(texts[i], xPos,
        //            yPos - margin, textSize, 0, textObjSize, color));
        //    }
        //}

        //private float GetTextYPosForXAxisTexts()
        //{
        //    float yPos;
        //    if (CenteredXAxis) yPos = InitialYPos + ((FinalYPos - InitialYPos) / 2);
        //    else yPos = InitialYPos;
        //    return yPos;
        //}

        //private float GetXAxisSeparation(int numTexts)
        //{
        //    float separation;
        //    if (numTexts == 1) separation = 0;
        //    else separation = ((FinalXPos - XAxisEdgeTextMargin) - (InitialXPos + XAxisEdgeTextMargin)) / (numTexts - 1);
        //    return separation;
        //}
        //private float GetYAxisSeparation(int numTexts)
        //{
        //    float separation;
        //    if (numTexts == 1) separation = 0;
        //    else separation = ((FinalYPos - YAxisEdgeTextMargin) - (InitialYPos + YAxisEdgeTextMargin)) / (numTexts - 1);
        //    return separation;
        //}
    }

}