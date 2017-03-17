using Improbable.General;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Math;
using Improbable.Collections;


namespace Assets.Gamelogic.Player.Behaviours
{
	// This MonoBehaviour will be enabled on both client and server-side workers
	public class PlayerMovement : MonoBehaviour {

		/* This class takes the postion and orientation from another source, such as PlayerController,
		 * and applies it to the GameObject that this is attached to.
		 * It also updates the WorldTransform entity that this is attached to. 
		*/

		[Require] private WorldTransform.Writer WorldTransformWriter;
		// Inject access to the entity's PlayerControls component
		[Require] protected PlayerControls.Reader PlayerControlsReader;

			// Use this for initialization
		void OnEnable () {

			// Initialize the position and orientation
			transform.position = WorldTransformWriter.Data.position.ToVector3();
			transform.rotation = new Quaternion();

			
		}
		
		// Update is called once per frame
		void Update () {


			transform.Translate(PlayerControlsReader.Data.keyHorizontal * Time.deltaTime, 0.0f, PlayerControlsReader.Data.keyVertical * Time.deltaTime);

			//Send out this rotation 
			//Comparisons for spatial's data structures
			Vector3 temp = transform.rotation.eulerAngles;
			Improbable.Math.Vector3f sendRot = new Improbable.Math.Vector3f (temp.x, temp.y, temp.z);

			//Update it's component values
			WorldTransformWriter.Send (new WorldTransform.Update ()
				.SetPosition (transform.position.ToCoordinates ())
				.SetRotation (sendRot)
				.SetSpeed (0.0f)
			);
						
		}
	}
	public static class Vector3Extensions
	{
		public static Vector3 ToVector3(this Coordinates coordinates)
		{
			return new Vector3((float)coordinates.X, (float)coordinates.Y, (float)coordinates.Z);
		}

		public static Coordinates ToCoordinates(this Vector3 vector3)
		{
			return new Coordinates(vector3.x, vector3.y, vector3.z);
		}
			
	}
}