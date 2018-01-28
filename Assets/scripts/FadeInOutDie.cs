using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutDie : MonoBehaviour {

	public TextMesh _text;
	float _startTimeStamp;
	public float _duration = 4.0f;
	float _fadeTime = 0.5f;
	public float _startOffset = 0;
	Color _color;

	enum state {
		Init,
		FadeIn,
		Display,
		FadeOut,
	}
	state _state = state.Init;

	void Start () {
		_startTimeStamp = Time.time;
		_color = _text.color;
		_color.a = 0;
		_text.color = _color;
	}
	
	void Update () {

		if (Time.time > _startTimeStamp + _startOffset && _state < state.Display) {
			_state = state.FadeIn;
			_color.a = (Time.time - _startTimeStamp - _startOffset) / _fadeTime;
			_text.color = _color;
			if (Time.time > _startTimeStamp + _startOffset + _fadeTime) {
				_color.a = 1;
				_text.color = _color;
				_state = state.Display;
			}
		}

		if (_state == state.Display && Time.time < _startTimeStamp + _duration + _fadeTime + _fadeTime + _startOffset) {
				_color.a = 1.0f - ((Time.time - _startTimeStamp - _fadeTime - _duration - _startOffset) / _fadeTime);
			_text.color = _color;
		} else if (_state == state.Display) {
			_color.a = 0;
			_text.color = _color;

			GameObject.Destroy (this);
		}
	}
}
