using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

	void Update () {
		groundMngr.instance.UpdateVertices();

		Debug.Log ("Val: " + microphoneMngr.instance.GetMicrophoneValue ());
	}
}
