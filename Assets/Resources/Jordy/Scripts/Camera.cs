using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

	GameObject player;
	Vector2 playerPosition;
	public float camPosX;
	public float camPosY;

	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag("Player");
		playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);

	}
	
	// Update is called once per frame
	void Update () {

		float camPosX = player.transform.position.x;
		float camPosY = player.transform.position.y;

		transform.position = new Vector3(camPosX, camPosY, -10f);

	}
}
