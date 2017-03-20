using Improbable.Player;
using Improbable.Unity.Visualizer;
using UnityEngine;

public class CameraRigEnabler : MonoBehaviour {

    public GameObject CameraRig;  //Set this explicitly in the editor

    // Update is called once per frame
	void OnEnable () {
        if (CameraRig != null)
            CameraRig.SetActive(true);
		
	}
}
