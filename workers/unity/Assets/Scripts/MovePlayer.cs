using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

	public float moveSpeed = 10.0F;
	public float rotateSpeed = 100.0F;

	// Use this for initialization
	void Start () {

		Cursor.lockState = CursorLockMode.Locked;
		
	}
	
	// Update is called once per frame
	void Update () {

		//Rotate
		/*float rotateY = rotateSpeed * Input.GetAxis("Mouse X");
		float rotateX = rotateSpeed * Input.GetAxis("Mouse Y");
		transform.Rotate(rotateX * Time.deltaTime, rotateY * Time.deltaTime, 0);*/


		float translationZ = Input.GetAxis("Vertical") * moveSpeed;
		float translationX = Input.GetAxis("Horizontal") * moveSpeed;
		transform.Translate(translationX * Time.deltaTime, 0.0f, translationZ * Time.deltaTime);


		//For DEBUG:
		if(Input.GetKeyDown("escape"))
			Cursor.lockState = CursorLockMode.None; 

	}
}
