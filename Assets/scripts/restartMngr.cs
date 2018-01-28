using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restartMngr : baseMngr<restartMngr> {

	public TextMesh _gameOver;
	float _gameOverTimestamp;
	float _duration;
	bool _deathClock = false;

	public void Wipe(){
		Destroy (enemyMngr.instance._container);
		enemyMngr.instance.Drop ();
		Destroy (homoMngr.instance._container);
		homoMngr.instance.Drop ();

		shoutMngr.instance.SetSegmentCount (2);
		Camera.main.GetComponent<main> ().scoreMngr.ResetScore ();

		GameObject gameOver_ = GameObject.Instantiate (_gameOver).gameObject;
		gameOver_.transform.parent = Camera.main.transform;
		gameOver_.transform.localPosition = _gameOver.transform.localPosition;

		_gameOverTimestamp = Time.time;
		_duration = _gameOver.GetComponent<FadeInOutDie> ()._duration;
		_deathClock = true;
	}

	void Update(){
		if (!_deathClock) {
			return;
		}

		if ( Time.time > _gameOverTimestamp + _duration ) {
			tutorialMngr.instance.Initialise ();	
			_deathClock = false;
		}
	}
}
