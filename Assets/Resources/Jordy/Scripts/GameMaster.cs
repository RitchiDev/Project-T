using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour 
{

	[SerializeField] private int spectatorMapWidth = 1800;
	[SerializeField] private int spectatorMapHeight = 200;
	public static int mapWidth;
	public static int mapHeight;
	public static int[,] blocks;
	public static long worldseed;

	// Use this for initialization
	void Awake () 
	{

		mapHeight = spectatorMapHeight;
		mapWidth = spectatorMapWidth;
		worldseed = (long)Random.Range(-1000000000000, 1000000000000);

		blocks = new int[mapWidth, mapHeight];

		for (int x = 0; x < blocks.GetLength(0); x++)
		{
			int surfaceLayer = PerlinNoise.Noise1D(x, 128, 8, mapWidth, worldseed);
			//Debug.Log(surfaceLayer + "SURFACE");
			int terrainHeight = surfaceLayer + 20;
			print(terrainHeight);
			for (int y = 0; y < blocks.GetLength(1); y++)
			{
				if (y == terrainHeight)
				{
					blocks[x, y] = 2; //grass
				}
				else if (y < terrainHeight && y > terrainHeight - 5)
				{
					blocks[x, y] = 3; //dirt
				}
				else if (y <= terrainHeight - 5)
				{
					blocks[x, y] = 1; //stone
				}
				else
				{
					blocks[x, y] = 0; //air
				}

				int dirtLump = PerlinNoise.Noise2D(x, y, 64, 60, mapWidth, worldseed);
				//Debug.Log(cave + " cave");
				if (dirtLump < 50 && blocks[x, y] == 1)
				{
					blocks[x, y] = 3;
				}
				int largeCavern = PerlinNoise.Noise2D(x, y, 64, 40, mapWidth, worldseed);
				if (largeCavern < (50 - y/5) && blocks[x,y] != 0 && blocks[x,y] !=2)
                {
					blocks[x, y] = 0;
                }
				int smallCavern = PerlinNoise.Noise2D(x, y, 32, 80, mapWidth, worldseed);
				if (smallCavern < (13) && blocks[x, y] != 0 && blocks[x, y] != 2)
				{
					blocks[x, y] = 0;
				}

			}

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
