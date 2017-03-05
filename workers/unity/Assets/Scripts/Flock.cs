using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {


	public float angularSpeed = 1.0f;  //?
	//private Vector3 averageHeading;
	//private Vector3 averagePosition;
	public float neighborDistance = 2.0f;  // The max distance to its closest neighbor, within which it will display flocking behavior
	public float collisionDistance = 0.2f; // The minimum distance to its neighbors, at which it will avoid those fish
	public float startingSpeed = 0.5f;
	public float maxSpeed = 1.0f;			//clamp the speed to this value.

	private float speed  = 0.5f;  //meters per second
	private GameObject[] allFishHandle;
	private Vector3 goalPosHandle;

	//When you're at the edge of the space, turn around
	public bool turnAround = false;

	// Use this for initialization
	void Start () 
	{
		//Apply a random speed to initialize the fish with, so that they all dont start with the same velocity
		speed = Random.Range (startingSpeed/2.0f, startingSpeed);	

		//Get the handle to all other fish & goal position from globalFlock
		allFishHandle = GlobalFlock.allFish;

				
	}
	
	// Update is called once per frame
	void Update () 
	{
		float distanceFromCenter = Vector3.Distance (this.transform.position, Vector3.zero);

		if (distanceFromCenter >= (GlobalFlock.tankSize * 0.95)) 
		{			
			applyReturn();
			Debug.Log ("Return for Id: " + GetInstanceID());

		} 

		else 
		{
			if (Random.Range (0, 5) < 1)
				ApplySwarm ();
		}

		//Move the fish now

		//Should be using Vector3.forward , but the Z axis (forward) of the fish, is actually towards the left.
		//transform.Translate (/*transform.localRotation * */Vector3.left * speed * Time.deltaTime);
		//transform.Translate(Time.deltaTime * speed,0,0);
		transform.Translate(0,0, Time.deltaTime * speed);
		//transform.Translate (Vector3.left*speed*Time.deltaTime);
	}

	void applyReturn()
	{
		//Towards the center of the tank
		Vector3 direction = Vector3.zero - this.transform.position; 

		//Slowly turn in that direction
		transform.rotation = Quaternion.Slerp ( this.transform.rotation,
			Quaternion.LookRotation (direction),
			angularSpeed * Time.deltaTime);

		//reset speed:
		//speed = Random.Range (startingSpeed/2.0f, startingSpeed);	
		speed = Mathf.Lerp (speed, startingSpeed, Time.deltaTime);
	
	}



	private void ApplySwarm()
	{
		//Group Speed
		float gSpeed = 0.1f;


		//Vectors that need to be calculated
		//Vector that points towards the center of the flock
		Vector3 vCenter = Vector3.zero;

		//Vector that avoids the other fish
		Vector3 vAvoid = Vector3.zero;

		//local vars
		float dist = 0.0f;
		int groupSize = 0;

		goalPosHandle = GlobalFlock.goalPos;

		foreach (GameObject fish in allFishHandle) 
		{
			//If it is not *this* fish
			if (fish != this.gameObject) 
			{

				//Calc *maginitude* of the distance to this fish
				dist = Vector3.Distance(fish.transform.position, this.transform.position);

				//Check if this fish lies within my neighbor range
				if (dist <= neighborDistance) 
				{
					groupSize++;

					//New center
					vCenter += fish.transform.position;

					if (dist <= collisionDistance)
						vAvoid -= this.transform.position - fish.transform.position;

					//Find the group speed
					float otherSpeed = fish.GetComponent<Flock>().speed;
					gSpeed += otherSpeed;
					//Debug.Log (otherSpeed);


				}


			}

		}



		//If this fish is within a group, change its position and velocity
		if (groupSize > 0) 
		{

			vCenter = (vCenter / groupSize) + (goalPosHandle - this.transform.position);

			//Set this fish's speed
			speed = gSpeed / groupSize;
			//Clamp the top speed (safety)
			speed = Mathf.Clamp (speed, startingSpeed, maxSpeed);

			Debug.Log ("Id: " + GetInstanceID() +" Group Size: " + groupSize + "  Speed: " + speed + " Goal: " + goalPosHandle);

			//Set it's orientation
			//Relative Position (or direction) = Target position (i.e. vCenter - vAvoid) minus current position
			//example : https://docs.unity3d.com/ScriptReference/Quaternion.LookRotation.html
			//Vector3 direction = (vCenter + vAvoid) - this.transform.position;
			Vector3 direction = (vCenter - vAvoid) - this.transform.position;


			//Slowly turn in that direction
			transform.rotation = Quaternion.Slerp ( this.transform.rotation,
				Quaternion.LookRotation (direction) ,
				angularSpeed * Time.deltaTime);
				



		}



	}

	public float GetSpeed()
	{
		return speed;
	}


}
