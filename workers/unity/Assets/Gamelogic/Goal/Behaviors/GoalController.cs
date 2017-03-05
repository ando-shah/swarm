using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Improbable;
using Improbable.Unity; 


namespace Assets.Gamelogic.Goal.Behaviours
{
	public class GoalController : MonoBehaviour {

		// Enable this MonoBehaviour on UnityWorker (server-side) workers only : different syntax on v10 vs v9 : https://spatialos.improbable.io/docs/reference/10.0/releases/upgrade-guides/how-to-upgrade-10
		[WorkerType(WorkerPlatform.UnityWorker)]
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () 
		{
			if (Random.Range (0, 100) < 1) 
			{
				newGoalPos = new Vector3 (Random.Range (-tankSize, tankSize),
					Random.Range (-tankSize, tankSize),
					Random.Range (-tankSize, tankSize));			

			}

			goalPos = Vector3.Lerp (goalPos, newGoalPos, Time.deltaTime * goalSpeed);

			goalPrefab.transform.position = goalPos;

			
		}
	}
}
