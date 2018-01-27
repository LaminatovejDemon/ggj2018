using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMngr : baseMngr<backgroundMngr> {

	bool _initialised = false;

	public void Initialise(){
		if (_initialised) {
			return;
		}

		transform.localScale = new Vector3 (Camera.main.orthographicSize * Camera.main.aspect * 2, Camera.main.orthographicSize * 2, 1);
		GetComponent<MeshRenderer> ().material.mainTextureScale = new Vector2(Camera.main.aspect, 1);
		transform.parent = Camera.main.transform;
		transform.localPosition = Vector3.forward * 1;
	}

	void Update(){
		GetComponent<MeshRenderer> ().material.mainTextureOffset = transform.position;
	}
}
