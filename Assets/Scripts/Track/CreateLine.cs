using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateLine : MonoBehaviour
{
    public Transform A;
    public Transform B;
    public Transform C;
    public Transform D;

    public int pointsDivision = 10;
    private Vector3[] points; 
    
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(A.position, 1);
        Gizmos.DrawSphere(D.position, 1);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(B.position, 1);
        Gizmos.DrawSphere(C.position, 1);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(A.position, B.position);
        Gizmos.DrawLine(C.position, D.position);
        
        Handles.DrawBezier(A.position, D.position, B.position, C.position,
            Color.white, null, 0.5f);

        points = Handles.MakeBezierPoints(A.position, D.position, B.position, C.position,
            pointsDivision);

        Gizmos.color = Color.yellow;
        foreach (Vector3 point in points)
        {
            Gizmos.DrawSphere(point, 0.25f);
        }
    }
#endif

    public Vector3[] GetPoints()
    {
        float interval = 1.0f / pointsDivision;
        float t = 0;
        int i = 0;
        
        Vector3[] points = new Vector3[pointsDivision];
        
        
        while (i < pointsDivision)
        {
            float x = (1-t)*(1-t)*(1-t)*A.position.x + 3*(1-t)*(1-t)*t*B.position.x + 3*(1-t)*t*t*C.position.x + t*t*t*D.position.x;
            float y = (1-t)*(1-t)*(1-t)*A.position.y + 3*(1-t)*(1-t)*t*B.position.y + 3*(1-t)*t*t*C.position.y + t*t*t*D.position.y;

            points[i] = new Vector3(x, y);
            
            i++;
            t += interval;
        }

        return points;
    }
}
