using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour {

    public float sliderValue;
    public Slider slider;

	void Start(){
        for (int i = 0; i < 3; ++i)
        {
			enemyMngr.instance.create(0.05f + (0.3f * (i+1)));
        }
		lineMngr.instance.Initialise ();
		homoMngr.instance.Initialise ();
	}

	void Update () {
		backgroundMngr.instance.Initialise ();
		groundMngr.instance.UpdateVertices();

        sliderValue = slider.value;

		if (Input.GetMouseButtonDown (0)) {
			shoutMngr.instance.TriggerHit ();
		}

		Camera.main.transform.position += Vector3.right * Time.deltaTime;
	/*	if (Camera.main.transform.position.x > 10.0f) {
			Camera.main.transform.position -= 10.0f;
		}*/
	}
}
