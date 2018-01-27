﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hit : MonoBehaviour {

	enum state{
		Ready,
		Active,
		Flashback,
	};

	state _state = state.Ready;

	public float _speed = 1.0f;

	public void Engage(){
		if (_state == state.Ready) {
			_state = state.Active;	
		}
	}

	void BackFire(){
		// TODO punish player
		Reset ();
	}

	void HandleActive(){
		transform.position += Vector3.up * Time.deltaTime * _speed;

		if (Camera.main.WorldToViewportPoint (transform.position).y > 1.0f) {
			float start_ = Camera.main.WorldToViewportPoint (transform.position).x;
			GameObject target_ = enemyMngr.instance.TestEnemy (start_, start_ + transform.localScale.x * Camera.main.orthographicSize * Camera.main.aspect * 2);
			if (target_ == null) {
				_state = state.Flashback;	
				return;
			} else {
				homoMngr.instance.Attack (target_);
				Reset ();
				return;
			}
		}
		else if (Camera.main.WorldToViewportPoint (transform.position - transform.localScale).y > 1.0f) {
			Reset();
		}
	}

	void HandleFlashback(){
		transform.position += Vector3.down * Time.deltaTime * _speed;
		if (Camera.main.ViewportToWorldPoint (Vector3.zero).y > transform.position.y) {
			BackFire ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_state == state.Ready) {
			return;
		}

		if (_state == state.Active) {
			HandleActive ();
		}

		if (_state == state.Flashback) {
			HandleFlashback ();
		}

	}

	public void Reset(){
		_state = state.Ready;
		Vector3 position_ = transform.position;
		position_.y = Camera.main.ViewportToWorldPoint (Vector3.zero).y;
		transform.position = position_;
	}
}
