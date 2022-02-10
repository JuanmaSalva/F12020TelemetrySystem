using UnityEngine;

public struct Line
{
    public Line(Vector3 a, Vector3 b, float width, Color color)
    {
        A = a;
        B = b;
        Width = width;
        Color = color;
    }

    public Vector3 A;
    public Vector3 B;
    public float Width;
    public Color Color;
}
