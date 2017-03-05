using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour {

	public GameObject fishPrefab;
	public GameObject goalPrefab;
	public float goalSpeed = 1.0f;
	private Vector3 newGoalPos = Vector3.zero;

	public static float tankSize = 5.0f; //meters

	static int numFish = 100;
	public static GameObject[] allFish = new GameObject[numFish];

	//Goal position that the flock swims towards
	public static Vector3 goalPos = Vector3.zero;

	// Use this for initialization : Done here, so that flock.cs can get the handle to the fish objects in Start
	void OnEnable () 
	{
		for (int i = 0; i < numFish; i++) 
		{
			//Spawn them at random positions
			Vector3 pos = new Vector3 (Random.Range (-tankSize, tankSize),
				              Random.Range (-tankSize, tankSize),
				              Random.Range (-tankSize, tankSize));

			//Instantiate the array of fish
			allFish[i] = (GameObject)Instantiate (fishPrefab, pos, Quaternion.identity);
		}

		
	}
	
	// Update is called once per frame
	void Update () 
	{
		

		//Once every 1000 frames, update the goal position
		if (Random.Range (0, 100) < 1) 
		{
			newGoalPos = new Vector3 (Random.Range (-tankSize, tankSize),
				Random.Range (-tankSize, tankSize),
				Random.Range (-tankSize, tankSize));			

		}

		goalPos = Vector3.Lerp (goalPos, newGoalPos, Time.deltaTime * goalSpeed);

		goalPrefab.transform.position = goalPos;

		/*
		float speed = 0.0f;
		//Calculate average speed
		foreach (GameObject fish in allFish)
			speed += fish.GetComponent<Flock> ().GetSpeed ();
		
		speed = speed / numFish;
		Debug.Log (speed);
		*/
		
	}
}
