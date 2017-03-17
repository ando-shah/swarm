using Improbable.General;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Math;


namespace Assets.Gamelogic.Player.Behaviours
{
	// Enable this MonoBehaviour on client workers only
	[WorkerType(WorkerPlatform.UnityClient)]
	public class CameraRotationController : MonoBehaviour {

		/* This class simply moves the camera left/right and up/down based on the mouse, and also move the 
		 * parent (player) around the y-axis (yaw).
		 * Only clients care about this, as we dont need to broadcast where the camera is looking
		 * That will change for the VR implementation
		*/

		// Inject access to the entity's PlayerControls component
		[Require] protected PlayerControls.Reader PlayerControlsReader;

		private Vector2 mouseLook;
		private GameObject player;

		// Use this for initialization
		void Start () {

			//Get the handle of the player (parent)
			player = this.transform.parent.gameObject;

			//Lock the cursor
			Cursor.lockState = CursorLockMode.Locked;
			
		}
		
		// Update is called once per frame
		void Update () {

			var mouseDelta = new Vector2 (PlayerControlsReader.Data.mouseX, PlayerControlsReader.Data.mouseY);
			mouseLook += mouseDelta;

			transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);  //inverted system
			player.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, player.transform.up);


			//For DEBUG:
			if(Input.GetKeyDown("escape"))
				Cursor.lockState = CursorLockMode.None; 
			
			
			
		}
	}
}