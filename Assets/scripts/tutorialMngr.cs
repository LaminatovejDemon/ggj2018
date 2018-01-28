using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialMngr : baseMngr<tutorialMngr> {

	public TextMesh _text;
	float _startTime;

	float _titleStart = 1.0f;
	float _fadeTime = 0.5f;
	float _titleLength = 3.0f;

	enum state{
		Init,
		PreTitle,
		Title,
		PostTitle
	}

	state _state = state.Init;

	public void Initialise(){
		transform.position = Camera.main.transform.position + Vector3.right * 3.0f;
		_state = state.Init;
		_startTime = Time.time;
	}
		
	void Update () {
		if (Time.time >  _startTime + _titleStart && _state < state.Title) {
			_state = state.PreTitle;
			_text.color = new Color (0, 0, 0, (Time.time - _startTime - _titleStart) / _fadeTime);
			if (Time.time > _startTime + _titleStart + _fadeTime) {
				_text.color = new Color (0, 0, 0, 1);
				_state = state.Title;
			}
		}

		if (_state == state.Title && Time.time < _startTime + _titleStart + _titleLength + _fadeTime + _fadeTime) {
			float alpha_ = 1.0f - ((Time.time - _startTime - _titleStart - _fadeTime - _titleLength) / _fadeTime);
			_text.color = new Color (0, 0, 0, alpha_);
		} else if (_state == state.Title) {
			_text.color = new Color (0, 0, 0, 0);
			_state = state.PostTitle;
			InitGame ();
		}

		if (_state == state.PostTitle) {
			UpdateGame ();
		}
	}

	void InitGame(){
		homoMngr.instance.Initialise ();
		enemyMngr.instance.Initialise ();
		enemyMngr.instance.PrepareEnemy (1, 2);
	}

	void UpdateGame(){
		if (Input.GetMouseButtonDown (0)) {
			shoutMngr.instance.TriggerHit ();
		}

		if (Camera.main.GetComponent<main> ().scoreMngr.GetScore () > 0 && shoutMngr.instance._segmentCount == 2) {
			shoutMngr.instance.SetSegmentCount (3);
		}
		else if (Camera.main.GetComponent<main> ().scoreMngr.GetScore () > 4*shoutMngr.instance._segmentCount && shoutMngr.instance._segmentCount < 11 ) {
			shoutMngr.instance.SetSegmentCount (shoutMngr.instance._segmentCount+1);
			enemyMngr.instance.recalculateBirds ();
		}

	}
}
