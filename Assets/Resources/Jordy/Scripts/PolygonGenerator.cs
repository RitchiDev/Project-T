using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour{

	public List<Vector3> newVertices = new List<Vector3> ();
	public List<int> newTriangles = new List<int> ();
	public List<Vector2> newUV = new List<Vector2> ();
	public int[,] blocks;

	public List<Vector3> 	colVertices =	new List<Vector3> ();
	public List<int> 		colTriangles = 	new List<int> ();
	public int colCount;
	public PolygonCollider2D col;

	static float 	tUnit = 0.2f;
	static Vector2 tMud = 			new Vector2(0, 0);
	static Vector2 tSand =	 		new Vector2(0, 1);
	static Vector2 tGrass = 		new Vector2(0, 2);
	static Vector2 tStone = 		new Vector2(0, 3);
	static Vector2 tDirt = 			new Vector2(0, 4);
	static Vector2 tSilverOre =		new Vector2(1, 0);
	static Vector2 tGoldOre = 		new Vector2(1, 1);
	static Vector2 tCopperOre =		new Vector2(1, 2);
	static Vector2 tIronOre = 		new Vector2(1, 3);
	static Vector2 tSandstone =		new Vector2(1, 4);
	static Vector2 tMudGrass = 		new Vector2(2, 0);
	static Vector2 tBorealWood = 	new Vector2(2, 1);
	static Vector2 tChloroOre = 	new Vector2(2, 2);
	static Vector2 tDemonOre = 		new Vector2(2, 3);
	static Vector2 tCrimOre = 		new Vector2(2, 4);
	static Vector2 tWood = 			new Vector2(3, 0);
	static Vector2 tTempleBrick = 	new Vector2(3, 1);
	static Vector2 tAsh = 			new Vector2(3, 2);
	static Vector2 tIce = 			new Vector2(3, 3);
	static Vector2 tSilt = 			new Vector2(3, 4);
	static Vector2 tFossil = 		new Vector2(4, 0);
	static Vector2 tDungeonBrick = 	new Vector2(4, 1);
	static Vector2 tGravel = 		new Vector2(4, 2);
	static Vector2 tSnow = 			new Vector2(4, 3);
	static Vector2 tHellOre = 		new Vector2(4, 4);

	private Mesh mesh;
	private int squareCount;

	int xOffset;
	int yOffset;

	public enum BlockID
    {
		Air=0,
		Stone=1,
		Grass=2,
		Dirt=3,
		Mud=4,
		Sand=5,
		Gravel=6,
		Snow=7,
		Ice=8
    }


	// Use this for initialization
	void Awake()

	{
		xOffset = (int)transform.position.x;
		yOffset = (int)transform.position.y;


		blocks = GameMaster.blocks;
		mesh = GetComponent<MeshFilter>().mesh;
		col = GetComponent<PolygonCollider2D>();
		//print("Chunk at: " + xOffset + " , " + yOffset);

	}

	public void BuildMesh()
	{
		for (int px = 0; px < ChunkMaster.chunkSize; px++)
		{
			
			for (int py = 0; py < ChunkMaster.chunkSize; py++)
			{

				if (blocks[px + xOffset , py + yOffset] != 0)
				{

					switch ((BlockID)blocks[px + xOffset, py + yOffset])
                    {
						case BlockID.Stone:
							GenSquare(px, py, tStone);
							break;
						case BlockID.Grass:
							GenSquare(px, py, tGrass);
							break;
						case BlockID.Dirt:
							GenSquare(px, py, tDirt);
							break;
						case BlockID.Mud:
							GenSquare(px, py, tMud);
							break;
						case BlockID.Sand:
							GenSquare(px, py, tSand);
							break;
						case BlockID.Gravel:
							GenSquare(px, py, tGravel);
							break;
						case BlockID.Snow:
							GenSquare(px, py, tSnow);
							break;
						case BlockID.Ice:
							GenSquare(px, py, tIce);
							break;
						default : break;

					}

					GenCollider(px , py);
				}
			}
		}
	}

	int Block(int x, int y)
	{

		if (x + xOffset == -1 || x + xOffset == blocks.GetLength(0) || y + yOffset == -1 || y + yOffset == blocks.GetLength(1))
		{
			return (byte)1;
		}

		return blocks[x + xOffset, y + yOffset];
	}

	void GenCollider(int x, int y)
	{

		//Top
		if (Block(x, y + 1) == 0)
		{
			colVertices.Add(new Vector3(x, y, 1));
			colVertices.Add(new Vector3(x + 1, y, 1));
			colVertices.Add(new Vector3(x + 1, y, 0));
			colVertices.Add(new Vector3(x, y, 0));

			ColliderTriangles();

			colCount++;
		}

		//bot
		if (Block(x, y - 1) == 0)
		{
			colVertices.Add(new Vector3(x, y - 1, 0));
			colVertices.Add(new Vector3(x + 1, y - 1, 0));
			colVertices.Add(new Vector3(x + 1, y - 1, 1));
			colVertices.Add(new Vector3(x, y - 1, 1));

			ColliderTriangles();
			colCount++;
		}

		//left
		if (Block(x - 1, y) == 0)
		{
			colVertices.Add(new Vector3(x, y - 1, 1));
			colVertices.Add(new Vector3(x, y, 1));
			colVertices.Add(new Vector3(x, y, 0));
			colVertices.Add(new Vector3(x, y - 1, 0));

			ColliderTriangles();

			colCount++;
		}

		//right
		if (Block(x + 1, y) == 0)
		{
			colVertices.Add(new Vector3(x + 1, y, 1));
			colVertices.Add(new Vector3(x + 1, y - 1, 1));
			colVertices.Add(new Vector3(x + 1, y - 1, 0));
			colVertices.Add(new Vector3(x + 1, y, 0));

			ColliderTriangles();

			colCount++;
		}

	}

	void ColliderTriangles()
	{
		colTriangles.Add(colCount * 4);
		colTriangles.Add((colCount * 4) + 1);
		colTriangles.Add((colCount * 4) + 3);
		colTriangles.Add((colCount * 4) + 1);
		colTriangles.Add((colCount * 4) + 2);
		colTriangles.Add((colCount * 4) + 3);
	}


	void GenSquare(int x, int y, Vector2 texture)
	{

		newVertices.Add(new Vector3(x, y, 0));
		newVertices.Add(new Vector3(x + 1, y, 0));
		newVertices.Add(new Vector3(x + 1, y - 1, 0));
		newVertices.Add(new Vector3(x, y - 1, 0));

		newTriangles.Add(squareCount * 4);
		newTriangles.Add((squareCount * 4) + 1);
		newTriangles.Add((squareCount * 4) + 3);
		newTriangles.Add((squareCount * 4) + 1);
		newTriangles.Add((squareCount * 4) + 2);
		newTriangles.Add((squareCount * 4) + 3);

		newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y + tUnit));
		newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y + tUnit));
		newUV.Add(new Vector2(tUnit * texture.x + tUnit, tUnit * texture.y));
		newUV.Add(new Vector2(tUnit * texture.x, tUnit * texture.y));

		squareCount++;

	}

	public void UpdateMesh()
	{
		mesh.Clear();
		mesh.vertices = newVertices.ToArray();
		mesh.triangles = newTriangles.ToArray();
		mesh.uv = newUV.ToArray();
		mesh.RecalculateNormals();

		newVertices.Clear();
		newTriangles.Clear();
		newUV.Clear();
		squareCount = 0;

		Mesh newMesh = new Mesh();
		newMesh.vertices = colVertices.ToArray();
		newMesh.triangles = colTriangles.ToArray();
		col.pathCount = 1;
		List<Vector3> vertices = new List<Vector3>();
		newMesh.GetVertices(vertices);
		List<EdgeHelpers.Edge> boundaryPath = EdgeHelpers.GetEdges(newMesh.triangles).FindBoundary().SortEdges();

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

		colVertices.Clear();
		colTriangles.Clear();
		colCount = 0;
	}
}