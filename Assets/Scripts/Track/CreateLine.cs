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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(A.position, 5);
        Gizmos.DrawSphere(D.position, 5);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(B.position, 5);
        Gizmos.DrawSphere(C.position, 5);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(A.position, B.position);
        Gizmos.DrawLine(C.position, D.position);
        
        Handles.DrawBezier(A.position, D.position, B.position, C.position,
            Color.white, null, 2);

        points = Handles.MakeBezierPoints(A.position, D.position, B.position, C.position,
            pointsDivision);

        Gizmos.color = Color.yellow;
        foreach (Vector3 point in points)
        {
            Gizmos.DrawSphere(point, 2);
        }
    }

    public Vector3[] GetPoints()
    {
       return Handles.MakeBezierPoints(A.position, D.position, B.position, C.position,
            pointsDivision);
    }
}
