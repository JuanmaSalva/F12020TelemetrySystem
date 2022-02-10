using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VertexData
{
    float[] pos;
    float[] color; //rgba

    public VertexData(Vector3 p, Color c)
    {
        pos = new float[2];
        color = new float[4];

        pos[0] = p.x;
        pos[1] = p.y;

        color[0] = c.r;
        color[1] = c.g;
        color[2] = c.b;
        color[3] = c.a;

    }
}

[System.Serializable]
public struct TrianglesData
{
    int[] triangle;

    public TrianglesData(Vector3Int t)
    {
        triangle = new int[3];

        triangle[0] = t.x;
        triangle[1] = t.y;
        triangle[2] = t.z;
    }
}

[System.Serializable]
public class LapGraphData
{
    public sbyte trackId;
    public float time;

    public VertexData[] vertecies;
    public TrianglesData[] triangles;
    
    public LapGraphData(sbyte track, float fastestTime,  List<UIVertex> v, List<Vector3Int> t)
    {
        trackId = track;
        time = fastestTime;

        vertecies = new VertexData[v.Count];
        triangles = new TrianglesData[t.Count];
        
        for(int i = 0; i < v.Count; i++)
            vertecies[i] = new VertexData(v[i].position, v[i].color);
        for (int i = 0; i < t.Count; i++)
            triangles[i] = new TrianglesData(t[i]);
    }
}
