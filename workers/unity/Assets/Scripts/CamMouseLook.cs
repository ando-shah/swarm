using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMouseLook : MonoBehaviour {

	Vector2 mouseLook;
	GameObject player;

	// Use this for initialization
	void Start () {
		
		player = this.transform.parent.gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {
		var mouseDelta = new Vector2 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"));
		mouseLook += mouseDelta;

		transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);  //inverted system
		player.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, player.transform.up);
		
	}
}
