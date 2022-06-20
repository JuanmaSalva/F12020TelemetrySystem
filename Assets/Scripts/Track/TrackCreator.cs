using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCreator : MonoBehaviour
{
	public List<CreateLine> lines;
	public int pointsDivision = 10;
	
	

	private Vector3[] points;
	private float[] distances;
	private float totalDist = 0;


	private void Awake()
	{
		points = new Vector3[lines.Count * (pointsDivision - 1) + 1];
		distances = new float[lines.Count * (pointsDivision - 1) + 1];

		int id = 0;
		Vector3[] aux = new Vector3[pointsDivision - 1];
		foreach (CreateLine line in lines)
		{
			aux = line.GetPoints();
			for (int i = 0; i < pointsDivision - 1; i++)
			{
				points[id] = aux[i];
				id++;
			}
		}

		points[points.Length - 1] = aux[aux.Length - 1];

		for (int i = 0; i < points.Length - 1; i++)
		{
			float dist = Vector3.Distance(points[i], points[i + 1]);
			totalDist += dist;
			distances[i + 1] = totalDist;
		}
	}

	
	public Vector3 GetDriverIconPos(float range)
	{
		float currentDist = totalDist * range;
		int currentInd = 0;
		while (true)
		{
			if (currentInd >= distances.Length - 2 ||
			    (currentDist < distances[1] && distances[currentInd] <= currentDist) ||
			    (distances[currentInd] <= currentDist && currentDist <= distances[currentInd + 1]))
				break;

			currentInd++;
		}

		float distanceDelta = distances[currentInd + 1] - distances[currentInd];
		float interpolation = (currentDist - distances[currentInd]) / distanceDelta;
		Vector3 currentPos = Vector3.Lerp(points[currentInd], points[currentInd + 1], interpolation);

		return currentPos;
	}
}