using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	[SerializeField] private int spectatorMapWidth = 1800;
	[SerializeField] private int spectatorMapHeight = 200;
	public static int mapWidth;
	public static int mapHeight;
	public static byte[,] blocks;
	public static long worldseed;

	// Use this for initialization
	void Awake () {

		mapHeight = spectatorMapHeight;
		mapWidth = spectatorMapWidth;
		worldseed = (long)Random.Range(-1000000000000, 1000000000000);

		blocks = new byte[mapWidth, mapHeight];

		for (int x = 0; x < blocks.GetLength(0); x++)
        {
			int surfaceLayer = PerlinNoise.Noise(x, 1, mapWidth, worldseed);
			Debug.Log(surfaceLayer + "SURFACE");
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
            }
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
