using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoutMngr : baseMngr<shoutMngr> {

	public int _segmentCount = 4;
	public GameObject _segmentPrefab;
	GameObject [] _segment;
	public float _segmentGap = 0.1f;

	public Color _activeColor;
	public Color _inactiveColor;

	int _activeIndex = 0;

	void Start(){
		Initialise ();
	}

	public void Initialise(){
		if (_segment != null) {
			return;
		}
		_segment = new GameObject[_segmentCount];
		for (int i = 0; i < _segmentCount; ++i) {
			_segment[i] = GameObject.Instantiate(_segmentPrefab);
			_segment[i].transform.localScale = new Vector3 (Camera.main.orthographicSize * 2 * Camera.main.aspect / _segmentCount - _segmentGap, Camera.main.orthographicSize * 2, 1);
			_segment[i].transform.parent = Camera.main.transform;
			_segment[i].transform.position = Camera.main.ViewportToWorldPoint (Vector3.zero) + Vector3.right * (_segment [i].transform.localScale.x + _segmentGap) * i + Vector3.back * 1.0f;
			_segment[i].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _inactiveColor);
		}
	}

	void SwitchSegment(int newIndex){
		_segment [_activeIndex].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _inactiveColor);
		_activeIndex = newIndex;
		_segment [_activeIndex].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _activeColor);
	}

	public void SetSweep(float position){
		int segment_ = (int)(_segmentCount * position);
		if (segment_ != _activeIndex) {
			SwitchSegment (segment_);
		}
	}
}
