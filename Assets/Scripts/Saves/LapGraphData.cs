using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VertexData
{
    float[] pos;
    //float[] color; //rgba

    public VertexData(Vector3 p/*, Color c*/)
    {
        pos = new float[2];
        //color = new float[4];

        pos[0] = p.x;
        pos[1] = p.y;

        //color[0] = c.r;
        //color[1] = c.g;
        //color[2] = c.b;
        //color[3] = c.a;
    }

    //public Color GetColor()
    //{
    //    Color c = new Color(color[0], color[1], color[2], color[3]);
    //    return c;
    //}

    public Vector3 GetPos()
    {
        return new Vector3(pos[0], pos[1]);
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

    public Vector3Int GetTriangle()
    {
        return new Vector3Int(triangle[0], triangle[1], triangle[2]);
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
            vertecies[i] = new VertexData(v[i].position/*, v[i].color*/);
        for (int i = 0; i < t.Count; i++)
            triangles[i] = new TrianglesData(t[i]);
    }

    public List<UIVertex> GetVertecies()
    {
        List<UIVertex> v = new List<UIVertex>();

        for (int i=0; i < vertecies.Length; i++)
        {
            UIVertex aux = new UIVertex();
            aux.position = vertecies[i].GetPos();
            v.Add(aux);
        }
        return v;
    }

    public List<Vector3Int> GetTriangles()
    {
        List<Vector3Int> t = new List<Vector3Int>();

        for(int i=0; i<triangles.Length; i++)
        {
            t.Add(triangles[i].GetTriangle());
        }

        return t;
    }
}
