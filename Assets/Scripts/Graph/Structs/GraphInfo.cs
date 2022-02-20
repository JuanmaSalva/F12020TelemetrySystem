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
    }
}