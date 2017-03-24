using Improbable.General;
using Improbable.Player;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine;
using Improbable.Math;
using Improbable.Collections;


namespace Assets.Gamelogic.Player.Behaviours
{
    // This MonoBehaviour will be enabled on only client-side workers
    [WorkerType(WorkerPlatform.UnityClient)]
    public class VRPlayerMovement : MonoBehaviour
    {

        /*It updates the WorldTransform entity that this is attached to. 
		*/

        [Require]
        private WorldTransform.Writer WorldTransformWriter;
        // Inject access to the entity's PlayerControls component
        //[Require]
        //protected PlayerControls.Reader PlayerControlsReader;
        
        private Transform ChildObject;
        private Transform CameraHeadObject;

        // Use this for initialization
        void OnEnable()
        {

            // Initialize the position and orientation
            transform.position = WorldTransformWriter.Data.position.ToVector3();
            transform.rotation = new Quaternion();


        }

        
        void Start()
        {
            //This is done so that the SteamVR camera rig is enabled only on the Client-side
            ChildObject = transform.Find("[CameraRig]");
            if (ChildObject == null)
            {
                Debug.LogError("Didnt find SteamVR CameraRig!!");
                return;
            }
            Debug.Log("Enabling SteamVR CameraRig");

            ChildObject.gameObject.SetActive(true);

            CameraHeadObject = ChildObject.Find("Camera (eye)");
            if (CameraHeadObject == null)
            {
                Debug.LogError("Didnt find SteamVR Camera Head!!");
                return;
            }
            Debug.Log("Found Camera Head");
        }
        
        // Update is called once per frame
        void Update()
        {
            
            Vector3 temp = transform.rotation.eulerAngles;
            Improbable.Math.Vector3f sendRot = new Improbable.Math.Vector3f(temp.x, temp.y, temp.z);

            //To take care of teleports
            Vector3 pos = transform.position + CameraHeadObject.position;

            //Update it's component values
            //Note : the player doesnt move as the person in VR walks around. In order to get the final
            //Cooridnates, we use the sum of player+player.[CameraRig].Camera(head)
            WorldTransformWriter.Send(new WorldTransform.Update()
                .SetPosition(pos.ToCoordinates())
                .SetRotation(sendRot)
                .SetSpeed(0.0f)
            );

        }
    }

    /*public static class Vector3Extensions
    {
        public static Vector3 ToVector3(this Coordinates coordinates)
        {
            return new Vector3((float)coordinates.X, (float)coordinates.Y, (float)coordinates.Z);
        }

        public static Coordinates ToCoordinates(this Vector3 vector3)
        {
            return new Coordinates(vector3.x, vector3.y, vector3.z);
        }

    }*/
}