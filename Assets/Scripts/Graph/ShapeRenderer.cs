using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Graph
{
    public class ShapeRenderer : Graphic
    {
        private List<UIVertex> vertexs;
        private List<Vector3Int> triangles;

        public ShapeRenderer()
        {
            vertexs = new List<UIVertex>();
            triangles = new List<Vector3Int>();
        }

        public void DrawCircle(Circle circle)
        {
        }

        public void DrawLine(Line line)
        {
            DrawSingleLine(line);
            UpdateGeometry();
        }

        public void SetInfo(List<UIVertex> v, List<Vector3Int> t)
        {
            vertexs = v;
            triangles = t;
            UpdateGeometry();
        }


        public void Clear()
        {
            vertexs.Clear();
            triangles.Clear();
            UpdateGeometry();
        }

        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();


            foreach (UIVertex vert in vertexs)
            {
                vh.AddVert(vert);
            }
            foreach(Vector3Int tri in triangles)
            {
                vh.AddTriangle(tri.x, tri.y, tri.z);
            }
        }


        private void DrawSingleLine(Line line)
        {
            int initialVertexIndx = vertexs.Count;
            CreateLineVertex(line);
            CreateLineTriangles(initialVertexIndx);
        }

        private void CreateLineVertex(Line line)
        {
            Vector3 A = FromRelativeToPixels(line.A);
            Vector3 B = FromRelativeToPixels(line.B);


            Vector3 dir = (B - A).normalized;
            Vector3 perpendicularDir = new Vector3(-dir.y, dir.x);
            float width = line.Width * rectTransform.rect.height;



            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = line.Color;

            vertex.position = A + perpendicularDir * width;
            vertexs.Add(vertex);
            vertex.position = A - perpendicularDir * width;
            vertexs.Add(vertex);

            vertex.position = B + perpendicularDir * width;
            vertexs.Add(vertex);
            vertex.position = B - perpendicularDir * width;
            vertexs.Add(vertex);
        }

        private void CreateLineTriangles(int initialVertexIndx)
        {
            triangles.Add(new Vector3Int(initialVertexIndx, initialVertexIndx + 2, initialVertexIndx + 1));
            triangles.Add(new Vector3Int(initialVertexIndx + 1, initialVertexIndx + 2, initialVertexIndx + 3));
        }

        private void DrawSingleCircle(Circle circle, VertexHelper vh)
        {
            int initialVertexIndx = vh.currentVertCount;
            CreateCircleVertex(circle, vh);
            CreateCircleTriangles(circle.Steps, vh, initialVertexIndx);
        }
        private void CreateCircleVertex(Circle circle, VertexHelper vh)
        {
            float angleIncrease = (float)(2 * Math.PI) / circle.Steps;

            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = circle.Color;

            Vector3 center = FromRelativeToPixels(circle.Center);
            float radius = circle.Radius * rectTransform.rect.height;

            vertex.position = center;
            vh.AddVert(vertex);
            for (int i = 0; i < circle.Steps; i++)
            {
                vertex.position = new Vector3(center.x + radius * (float)Math.Cos(angleIncrease * i),
                    center.y + radius * (float)Math.Sin(angleIncrease * i));
                vh.AddVert(vertex);
            }
        }
        private static void CreateCircleTriangles(int steps, VertexHelper vh, int initialVertexIndx)
        {
            for (int i = 0; i < steps; i++)
                vh.AddTriangle(initialVertexIndx, initialVertexIndx + i, initialVertexIndx + i + 1);
            vh.AddTriangle(initialVertexIndx, initialVertexIndx + 1, initialVertexIndx + steps);
        }


        private Vector3 FromRelativeToPixels(Vector3 pos)
        {
            Rect rect = rectTransform.rect;
            return new Vector3(pos.x * rect.width, pos.y * rect.height);
        }

        public List<UIVertex> GetVertecies()
        {
            return vertexs;
        }

        public List<Vector3Int> GetTriangles()
        {
            return triangles;
        }

    }
}
