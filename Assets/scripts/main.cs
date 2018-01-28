using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour {

    public float sliderValue;
    public Slider slider;

	void Start(){
		Restart ();
	}

	public void Restart(){
		lineMngr.instance.Initialise ();
		homoMngr.instance.Initialise ();
	}

	void Update () {
		groundMngr.instance.UpdateVertices();

        sliderValue = slider.value;

		if (Input.GetMouseButtonDown (0)) {
			shoutMngr.instance.TriggerHit ();
		}

		Camera.main.transform.position += Vector3.right * Time.deltaTime;

		float roundedDistance_ = groundMngr.instance.GetRoundedLength ();
		if (Camera.main.transform.position.x > roundedDistance_ * 3.0f) {
			Camera.main.transform.position += Vector3.left * roundedDistance_;
			homoMngr.instance._container.transform.position += Vector3.left * roundedDistance_;
		}

	}
}
