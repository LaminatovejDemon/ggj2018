﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoutMngr : baseMngr<shoutMngr> {

	public int _segmentCount = 4;
	int _localSegmentCount;

	public GameObject _segmentPrefab;
	public hit _hitPrefab;

	GameObject [] _segment;
	hit [] _hit;
	public float _segmentGap = 0.1f;
	float _currentAmount = 0;
	public float _shoutThreshold = 500.0f;

	public Color _activeColor;
	public Color _inactiveColor;
	public Color _hotColor;

	public void SetSegmentCount(int newNumber){
		_localSegmentCount = newNumber;
	}

	float _segmentSize = 0;

	int _activeIndex = 0;

	void Start(){
		_localSegmentCount = _segmentCount;
		Initialise ();
	}


	public float GetViewPortSegmentSize(){
		Initialise ();

		return 1.0f / _segmentCount;
	}

	public float GetSegmentSize(){
		return _segmentSize;
	}

	public void Initialise(){
		if (_segment != null && _segmentCount == _localSegmentCount) {
			return;
		}

		if (_segment != null) {
			for (int i = 0; i < _segment.Length; ++i) {
				GameObject.Destroy (_segment [i].gameObject);
				GameObject.Destroy (_hit [i].gameObject);
			}
		}

		_segmentCount = _localSegmentCount;

		_currentAmount = 0;
		_segment = new GameObject[_segmentCount];
		_hit = new hit[_segmentCount];

		_segmentSize = Camera.main.orthographicSize * 2 * Camera.main.aspect / _segmentCount - _segmentGap;

		for (int i = 0; i < _segmentCount; ++i) {
			_segment[i] = GameObject.Instantiate(_segmentPrefab);
			_segment[i].transform.localScale = new Vector3 (_segmentSize, Camera.main.orthographicSize * 2, 1);
			_segment[i].transform.parent = Camera.main.transform;
			_segment[i].transform.position = Camera.main.ViewportToWorldPoint (Vector3.zero) + Vector3.right * (_segment [i].transform.localScale.x + _segmentGap) * i + Vector3.back * 1.0f;
			_segment[i].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _inactiveColor);

			_hit [i] = GameObject.Instantiate (_hitPrefab).GetComponent<hit> ();
			_hit[i].transform.localScale = new Vector3 (_segmentSize, Camera.main.orthographicSize * 0.2f, 1);
			_hit[i].transform.parent = Camera.main.transform;
			_hit[i].transform.position = Camera.main.ViewportToWorldPoint (Vector3.zero) + Vector3.right * (_segment [i].transform.localScale.x + _segmentGap) * i + Vector3.back * 1.1f;
			_hit[i].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _inactiveColor);
		}

	}

	void SwitchSegment(int newIndex){
		Initialise ();

		if (_activeIndex >= _segment.Length) {
			_activeIndex = 0;
			return;
		}
		if (newIndex >= _segment.Length) {
			newIndex = 0;
		}

		_segment [_activeIndex].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _inactiveColor);

		if (_currentAmount > _shoutThreshold / _segmentCount) {
			Hit (_activeIndex);
		}

		_activeIndex = newIndex;
		_segment [_activeIndex].transform.GetChild(0).GetComponent<Renderer> ().material.SetColor ("_TintColor", _activeColor);

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
