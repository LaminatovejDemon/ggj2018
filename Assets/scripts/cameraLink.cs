using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraLink : MonoBehaviour {

	float _cameraSpring = 10.0f;

	void Update () {
		Vector3 cameraBak_ = Camera.main.transform.position;
		cameraBak_.x += (transform.position - Camera.main.transform.position).x * _cameraSpring * Time.deltaTime;
		Camera.main.transform.position = cameraBak_;
	}
}
