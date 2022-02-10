using UnityEngine;

namespace Graph
{
    public struct Circle
    {
        public Circle(Vector3 center, float radius, int steps, Color color)
        {
            Center = center;
            Radius = radius;
            Steps = steps;
            Color = color;
        }

        public Vector3 Center;
        public float Radius;
        public int Steps;
        public Color Color;
    }
}
