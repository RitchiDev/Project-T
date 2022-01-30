using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMaster : MonoBehaviour {

	public static int chunkSize = 100;
	public GameObject chunkMesh;
	public GameObject[,] chunks;
	// Use this for initialization
	void Awake () {
		chunks = new GameObject[GameMaster.mapWidth / chunkSize, GameMaster.mapHeight / chunkSize];


		for (int chunkX = 0; chunkX < chunks.GetLength(0); chunkX++)
        {
			for (int chunkY = 0; chunkY < chunks.GetLength(1); chunkY++)
            {
			
				chunks[chunkX, chunkY] = Instantiate(chunkMesh, new Vector3 (chunkX * chunkSize, chunkY * chunkSize , 0 ),Quaternion.identity);
				chunks[chunkX, chunkY].transform.parent = this.transform;
            }
        }
	}
	void Start()
    {
		for (int chunkX = 0; chunkX < chunks.GetLength(0); chunkX++)
		{
			for (int chunkY = 0; chunkY < chunks.GetLength(1); chunkY++)
			{
				GameObject chunkToBuild = chunks[chunkX, chunkY];
				chunkToBuild.GetComponent<PolygonGenerator>().BuildMesh();
				chunkToBuild.GetComponent<PolygonGenerator>().UpdateMesh();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		for (int chunkX = 0; chunkX < chunks.GetLength(0); chunkX++)
		{
			for (int chunkY = 0; chunkY < chunks.GetLength(1); chunkY++)
			{
				GameObject chunkToUpdate = chunks[chunkX, chunkY];
				//chunkToUpdate.GetComponent<PolygonGenerator>().BuildMesh();
				//chunkToUpdate.GetComponent<PolygonGenerator>().UpdateMesh();
			}
		}

	}
}
