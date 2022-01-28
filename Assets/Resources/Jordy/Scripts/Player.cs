using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float playerSpeed = 6;
	Vector3 velocity;
	public float jumpStrength;

	Rigidbody myRigidbody;

	// Use this for initialization
	void Start () {

		myRigidbody = GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update () {

	
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Vector3 direction = input.normalized;
		velocity = new Vector3(direction.x * playerSpeed, direction.y * jumpStrength);
		
	}
	void FixedUpdate()
    {
		myRigidbody.position += velocity * Time.deltaTime;
       
    }
}
