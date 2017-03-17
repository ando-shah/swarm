using UnityEngine;
using Improbable.General;
using Improbable.Math;
using Improbable.Player;
using Improbable.Unity.Core.Acls;
using Improbable.Worker;

namespace Assets.EntityTemplates
{
	
	public class PlayerEntityTemplate : MonoBehaviour
	{
		// Template definition for a Player snapshot entity
		static public Entity GeneratePlayerEntityTemplate(string clientWorkerId, Coordinates initialPosition)
		{
			Vector3f playerInitialRotation = new Vector3f (0.0f, 0.0f, 0.0f);


			var playerEntityTemplate = new Entity();

			// Define components attached to entity
			playerEntityTemplate.Add(new WorldTransform.Data(new WorldTransformData(initialPosition, playerInitialRotation, 0)));
			playerEntityTemplate.Add(new PlayerLifecycle.Data(new PlayerLifecycleData(0, 3, 10)));    	//No missed heartbeats, upto 3 missed Hbs allowed, HB interval = 10
			playerEntityTemplate.Add (new PlayerControls.Data (new PlayerControlsData (0, 0, 0, 0)));


			// Grant component access permissions
			var acl = Acl.Build()
				.SetReadAccess(CommonRequirementSets.PhysicsOrVisual)									  //Both Server and Client workers have read access to all the components of PlayerShip
				.SetWriteAccess<WorldTransform> (CommonRequirementSets.SpecificClientOnly(clientWorkerId)) //only UnityClient has access
				.SetWriteAccess<PlayerControls> (CommonRequirementSets.SpecificClientOnly(clientWorkerId)) //only UnityClient has access
				.SetWriteAccess<PlayerLifecycle>(CommonRequirementSets.PhysicsOnly);						  //UnityWorker has access

			playerEntityTemplate.SetAcl(acl);

			return playerEntityTemplate;   
		}
	}
}