using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restartMngr : baseMngr<restartMngr> {

	public void Wipe(){
		Destroy (enemyMngr.instance._container);
		enemyMngr.instance.Drop ();
		Destroy (homoMngr.instance._container);
		homoMngr.instance.Drop ();

		shoutMngr.instance.SetSegmentCount (2);
		Camera.main.GetComponent<main> ().scoreMngr.ResetScore ();

		tutorialMngr.instance.Initialise ();
	}
}
