using Improbable;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Worker;
using System;
using Improbable.General;
using Improbable.Math;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Random = UnityEngine.Random;
using Improbable.Worker.Query;
using Improbable.Collections.Internal;
using Improbable.Unity.Core.EntityQueries;
using Improbable.Collections;



namespace Assets.Gamelogic.Fish.Behaviours
{
	// Enable this MonoBehaviour on UnityWorker (server-side) workers only : different syntax on v10 vs v9 : https://spatialos.improbable.io/docs/reference/10.0/releases/upgrade-guides/how-to-upgrade-10
	[WorkerType(WorkerPlatform.UnityWorker)]

	public class FishController : MonoBehaviour
	{

		/* 
         * An entity with this MonoBehaviour will have it enabled only for the single worker (whether client or FSim)
         * which has write-access for its WorldTransform component.
         */
		[Require] private WorldTransform.Writer WorldTransformWriter;

		public float initialSpeed = 3.0f;
		public float maxSpeed = 3.5f;
		public float angularSpeed = 10.0f;
		public double neighborDistance = 5.0;  // The max distance to its closest neighbor, within which it will display swarming behavior
		public float collisionDistance = 0.2f; // The minimum distance to its neighbors, at which it will avoid those fish

		//This will govern its current speed
		private float speed;

	
		//From Goal Parameters
		private GameObject goalObj;
		private EntityId goalId;
		private float numFish;
		private float tankSize;

		public void OnEnable()
		{
			transform.position = WorldTransformWriter.Data.position.ToVector3 ();
			speed = initialSpeed;
		}


		void Start()
		{

			//Pick up the Goal Entity 
			var query = Query.HasComponent<GoalParameters>().ReturnOnlyEntityIds();

			SpatialOS.Commands.SendQuery (WorldTransformWriter, query, result => {
				if (result.StatusCode != StatusCode.Success) {
					Debug.LogError ("Goal Query failed with error: " + result.ErrorMessage);
					return;
				}


				if (result.Response.Count < 1) {
					Debug.LogError ("Goal NOT found!");
					return;  //No goal
				}


				Map<EntityId, Entity> resultMapG = result.Response.Value.Entities;
				goalId = resultMapG.First.Value.Key;

				if (SpatialOS.Universe.ContainsEntity (goalId)) {
					goalObj = SpatialOS.Universe.Get(goalId).UnderlyingGameObject; // this works also
					numFish = SpatialOS.GetLocalEntityComponent<GoalParameters>(goalId).Get().Value.numfish;
					tankSize = SpatialOS.GetLocalEntityComponent<GoalParameters>(goalId).Get().Value.tanksize;

					Debug.Log ("Goal Found @" + goalObj.transform.position + " with ID: " + goalId.Id + "And Num Fish: " + numFish +" in tank of size" + tankSize);


				} else {							
					Debug.LogError ("Goal NOT found!");
					return;
				}
			});
		}


		public void Update()
		{
			//Check if it's too far from center
			//float distanceFromCenter = Vector3.Distance(this.transform.position, Vector3.zero);
			if( (transform.position.x >= tankSize)  || (transform.position.y >= tankSize) || (transform.position.z >= tankSize))
				ApplyReturn ();
			else {
				if (Random.Range (0, 10) < 1)
					ApplySwarmMechanics ();
			}

			Vector3 temp = transform.rotation.eulerAngles;

			Improbable.Math.Vector3f sendRot = new Improbable.Math.Vector3f (temp.x, temp.y, temp.z);

			//Move the fish according to it's current speed value
			transform.Translate(0,0,Time.deltaTime * speed);

			//Broadcast it's component values
			WorldTransformWriter.Send (new WorldTransform.Update ()
				.SetPosition (transform.position.ToCoordinates ())
				.SetRotation (sendRot)
				.SetSpeed (speed)
			);

		}


		private void ApplySwarmMechanics()
		{
			var query = Query.InSphere(transform.position.x, transform.position.y, transform.position.z, neighborDistance).ReturnOnlyEntityIds();
			int groupSize = 0;
			float groupSpeed = 0.0f;


			//Goal's current position (from SwarmGoal entity)
			Vector3 goalPos = Vector3.zero;

			//Vectors that need to be calculated
			//Vector that points towards the center of the flock
			Vector3 vCenter = Vector3.zero;

			//Vector that avoids other fish
			Vector3 vAvoid = Vector3.zero;





			SpatialOS.Commands.SendQuery(WorldTransformWriter, query, result => {
				if (result.StatusCode != StatusCode.Success) {
					Debug.Log("Query failed with error: " + result.ErrorMessage);
					return;
				}

				Debug.Log("Found " + result.Response.Count + " nearby entities");
				if (result.Response.Count < 1) {
					return;  //No fish in range
				}
				Map<EntityId, Entity> resultMap = result.Response.Value.Entities;

				foreach(var item in resultMap)
				{
					EntityId idRef = item.Key;

					if (idRef.Id == numFish)  //i.e. == goal
						continue;
					//Entity   SwarmGoalEntity = item.Value;

					//Debug.Log("Looking for key in loop#" + groupSize );
					//if(SpatialOS.Universe.ContainsEntity(item.Key))   //From Improbable.Core.Entity.SpatialOS.Universe
					//{

					//GameObject otherFish = SpatialOS.Universe.Get(idRef).UnderlyingGameObject; // this works also
					Vector3 otherFishPos = SpatialOS.GetLocalEntityComponent<WorldTransform>(idRef).Get().Value.position.ToVector3();
					float otherSpeed     = SpatialOS.GetLocalEntityComponent<WorldTransform>(idRef).Get().Value.speed;

					vCenter += otherFishPos;
					float otherDistance = Vector3.Distance(this.transform.position, otherFishPos);

					if(otherDistance <= collisionDistance)
						vAvoid -= this.transform.position - otherFishPos;

					groupSpeed += otherSpeed;

					groupSize++;
					//}
				}

				//Debug.Log ("Group Size1: " + groupSize);



				//Debug.Log ("Group Size2: " + groupSize);

				if (groupSize > 0) {

					//EntityId id = new EntityId (1003);

					goalPos = goalObj.transform.position;

					vCenter = (vCenter / groupSize) + (goalPos - this.transform.position);
					//speed = groupSpeed/groupSize;

					Debug.Log("Goal Pos : (" + goalPos.x + "," + goalPos.y + "," + goalPos.z + ")" + " Center ("+ vCenter.x + "," + vCenter.y + "," + vCenter.z + ")" + " GroupSize: " + groupSize + " Speed: " + speed);


					//Clamp max speed, in case it spirals out of control
					//speed = Mathf.Clamp (speed, initialSpeed, maxSpeed);

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
			});


		}



		private void ApplyReturn()
		{
			//Towards the center of the tank
			Vector3 direction = Vector3.zero - this.transform.position; 

			//Slowly turn in that direction
			transform.rotation = Quaternion.Slerp ( this.transform.rotation,
				Quaternion.LookRotation (direction),
				angularSpeed * Time.deltaTime);

			//reset speed:
			//speed = Random.Range (startingSpeed/2.0f, startingSpeed);	
			speed = Mathf.Lerp (speed, initialSpeed, Time.deltaTime);
			
		}


	}

	public static class Vector3Extensions
	{
		public static Coordinates ToCoordinates(this Vector3 vector3)
		{
			return new Coordinates(vector3.x, vector3.y, vector3.z);
		}
	}

}
