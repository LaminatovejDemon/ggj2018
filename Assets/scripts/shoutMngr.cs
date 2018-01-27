using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoutMngr : baseMngr<shoutMngr> {

	public int _segmentCount = 4;
	public GameObject _segmentPrefab;
	public hit _hitPrefab;

	GameObject [] _segment;
	hit [] _hit;
	public float _segmentGap = 0.1f;
	float _currentAmount = 0;
	public float _shoutThreshold = 50.0f;

	public Color _activeColor;
	public Color _inactiveColor;
	public Color _hotColor;

	int _activeIndex = 0;

	void Start(){
		Initialise ();
	}

	public void Initialise(){
		if (_segment != null) {
			return;
		}
		_currentAmount = 0;
		_segment = new GameObject[_segmentCount];
		_hit = new hit[_segmentCount];

		for (int i = 0; i < _segmentCount; ++i) {
			_segment[i] = GameObject.Instantiate(_segmentPrefab);
			_segment[i].transform.localScale = new Vector3 (Camera.main.orthographicSize * 2 * Camera.main.aspect / _segmentCount - _segmentGap, Camera.main.orthographicSize * 2, 1);
			_segment[i].transform.parent = Camera.main.transform;
			_segment[i].transform.position = Camera.main.ViewportToWorldPoint (Vector3.zero) + Vector3.right * (_segment [i].transform.localScale.x + _segmentGap) * i + Vector3.back * 1.0f;
			_segment[i].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _inactiveColor);

			_hit [i] = GameObject.Instantiate (_hitPrefab).GetComponent<hit> ();
			_hit[i].transform.localScale = new Vector3 (Camera.main.orthographicSize * 2 * Camera.main.aspect / _segmentCount - _segmentGap, Camera.main.orthographicSize * 0.2f, 1);
			_hit[i].transform.parent = Camera.main.transform;
			_hit[i].transform.position = Camera.main.ViewportToWorldPoint (Vector3.zero) + Vector3.right * (_segment [i].transform.localScale.x + _segmentGap) * i + Vector3.back * 1.1f;
			_hit[i].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _inactiveColor);
		}
	}

	void SwitchSegment(int newIndex){
		_segment [_activeIndex].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _inactiveColor);

		if (_currentAmount > _shoutThreshold) {
			Hit (_activeIndex);
		}

		_activeIndex = newIndex;
		_segment [_activeIndex].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _activeColor);

		//Debug.Log("total Amount is " + _currentAmount);
		_currentAmount = 0;
	}

	public void TriggerHit(){
		SetSweep (_activeIndex / (float)_segmentCount, 1000);
	}

	void Hit(int index){
		_hit [index].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _hotColor);
		_hit [index].Engage ();
	}

	public int GetSegment(){
		return _activeIndex;
	}

	public void SetSweep(float position, float value){
		int segment_ = (int)(_segmentCount * position);
		if (segment_ != _activeIndex) {
			SwitchSegment (segment_);
		}
		_currentAmount += value;
	}
}
