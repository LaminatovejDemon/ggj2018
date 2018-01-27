using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

	void Start(){
        for (int i = 0; i < 3; ++i)
        {
            enemyMngr.instance.create(0.05f + (0.3f * i));
        }
		lineMngr.instance.Initialise ();
		homoMngr.instance.Initialise ();
	}

	void Update () {
		backgroundMngr.instance.Initialise ();
		groundMngr.instance.UpdateVertices();


		if (Input.GetMouseButtonDown (0)) {
			shoutMngr.instance.TriggerHit ();
		}
	}
}
