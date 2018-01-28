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
		tutorialMngr.instance.Initialise ();
	}

	void Update () {
		groundMngr.instance.UpdateVertices();
		lineMngr.instance.Initialise ();
		sliderValue = slider.value;

		Camera.main.transform.position += Vector3.right * Time.deltaTime;

		float roundedDistance_ = groundMngr.instance.GetRoundedLength ();
		if (Camera.main.transform.position.x > roundedDistance_ * 3.0f) {
			Camera.main.transform.position += Vector3.left * roundedDistance_;

			if (homoMngr.instance._container != null) {
				homoMngr.instance._container.transform.position += Vector3.left * roundedDistance_;
			}
		}
	}
}
