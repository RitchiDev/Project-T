using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EdgeHelpers
{
	/*
		Vector3[] vertices = newMesh.vertices;
        Vector2[] vertices2d = new Vector2[vertices.Length];

        for (var i = 0; i < vertices.Length; i++)
        {
        	vertices2d[i] = new Vector2(vertices[i].x, vertices[i].y);
        }

        vertices2d = vertices2d.Distinct().ToArray();

        col.points = vertices2d;
	*/

	/*
		col.pathCount = 1;
		List<Vector3> vertices = new List<Vector3>();
		newMesh.GetVertices(vertices);
		List<EdgeHelpers.Edge> boundaryPath = EdgeHelpers.GetEdges(newMesh.triangles).FindBoundary().SortEdges();
		//FindBoundary(boundaryPath);
		//SortEdges(boundaryPath);

		Vector3[] yourVectors = new Vector3[boundaryPath.Count];
		for (int i = 0; i < boundaryPath.Count; i++)
		{
			yourVectors[i] = vertices[boundaryPath[i].v1];
		}

		List<Vector2> newColliderVertices = new List<Vector2>();

		for (int i = 0; i < yourVectors.Length; i++)
		{
			newColliderVertices.Add(new Vector2(yourVectors[i].x, yourVectors[i].y));
		}

		Vector2[] newPoints = newColliderVertices.Distinct().ToArray();

		col.SetPath(0, newPoints);
	*/

	public struct Edge
	{
		public int v1;
		public int v2;
		public int triangleIndex;
		public Edge(int aV1, int aV2, int aIndex)
		{
			v1 = aV1;
			v2 = aV2;
			triangleIndex = aIndex;
		}
	}

	public static List<Edge> GetEdges(int[] aIndices)
	{
		List<Edge> result = new List<Edge>();
		for (int i = 0; i < aIndices.Length; i += 3)
		{
			int v1 = aIndices[i];
			int v2 = aIndices[i + 1];
			int v3 = aIndices[i + 2];
			result.Add(new Edge(v1, v2, i));
			result.Add(new Edge(v2, v3, i));
			result.Add(new Edge(v3, v1, i));
		}
		return result;
	}

	public static List<Edge> FindBoundary(this List<Edge> aEdges)
	{
		List<Edge> result = new List<Edge>(aEdges);
		for (int i = result.Count - 1; i > 0; i--)
		{
			for (int n = i - 1; n >= 0; n--)
			{
				if (result[i].v1 == result[n].v2 && result[i].v2 == result[n].v1)
				{
					// shared edge so remove both
					result.RemoveAt(i);
					result.RemoveAt(n);
					i--;
					break;
				}
			}
		}
		return result;
	}
	public static List<Edge> SortEdges(this List<Edge> aEdges)
	{
		List<Edge> result = new List<Edge>(aEdges);
		for (int i = 0; i < result.Count - 2; i++)
		{
			Edge E = result[i];
			for (int n = i + 1; n < result.Count; n++)
			{
				Edge a = result[n];
				if (E.v2 == a.v1)
				{
					// in this case they are already in order so just continoue with the next one
					if (n == i + 1)
						break;
					// if we found a match, swap them with the next one after "i"
					result[n] = result[i + 1];
					result[i + 1] = a;
					break;
				}
			}
		}
		return result;
	}
}