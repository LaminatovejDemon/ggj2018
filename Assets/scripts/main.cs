﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class main : MonoBehaviour {

	void Start(){
		enemyMngr.instance.create (0.45f);
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
