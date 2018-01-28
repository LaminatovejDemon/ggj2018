using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restartMngr : baseMngr<restartMngr> {

	public void Wipe(){
		enemyMngr.instance.Drop ();
		groundMngr.instance.Drop ();
		lineMngr.instance.Drop ();
		Destroy (homoMngr.instance._container);
		homoMngr.instance.Drop ();
		shoutMngr.instance.Drop ();
		backgroundMngr.instance.Drop ();

		Camera.main.GetComponent<Camera> ().enabled = false;
	}
}
