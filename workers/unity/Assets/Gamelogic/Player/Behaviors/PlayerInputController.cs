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
using Improbable.Player;


namespace Assets.Gamelogic.Player.Behaviours
{
	// Enable this MonoBehaviour on client workers only
	[WorkerType(WorkerPlatform.UnityClient)]
	public class PlayerInputController : MonoBehaviour
	{
		/* 
         * Client will only have write-access for their own designated PlayerShip entity's ShipControls component,
         * so this MonoBehaviour will be enabled on the client's designated PlayerShip GameObject only and not on
         * the GameObject of other players' ships.
         */


		[Require] private PlayerControls.Writer PlayerControlsWriter;
		public float moveSpeed = 10.0f;
		public float turnspeed = 50.0f;

		void Update ()
		{
			//Send the speed/steering inputs

			PlayerControlsWriter.Send (new PlayerControls.Update ()
				.SetKeyHorizontal (Input.GetAxis ("Horizontal") * moveSpeed)
				.SetKeyVertical (Input.GetAxis ("Vertical") * moveSpeed)
				.SetMouseX (Input.GetAxis ("Mouse X") * turnspeed)
				.SetMouseY (Input.GetAxis ("Mouse Y") * turnspeed));



		}
	}
}
