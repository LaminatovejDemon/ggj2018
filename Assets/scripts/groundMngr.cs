using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundMngr : baseMngr<groundMngr> {

	public line _groundTemplate;

	public GameObject[] _grassTemplates;
	int _grassAmount = 150;
	 GameObject[] _grassInstances;

	line[] _instances;
	int _instanceCount = 3;

	void InitialiseGrass(){
		_grassInstances = new GameObject[_grassAmount];
		for (int i = 0; i < _grassAmount; ++i) {
			_grassInstances [i] = GameObject.Instantiate (_grassTemplates [Random.Range (0, _grassTemplates.GetLength (0))]);
			_grassInstances [i].name = "Grass_" + i;
		}
	}

	void InitialiseInstances(){
		if (_instances != null) {
			return;
		}
		_instances = new line[_instanceCount];

		InitialiseGrass ();

		for (int i = 0; i < _instanceCount; ++i) {
			_instances [i] = GameObject.Instantiate (_groundTemplate).GetComponent<line>();
			_instances [i].name = "Ground_" + i;
			_instances [i].transform.parent = transform;

			_instances [i].transform.position = Camera.main.ViewportToWorldPoint (Vector3.zero);
			if (i > 0) {
				_instances [i].transform.position += Vector3.right * _instances [i - 1].GetLength ();
			}
			_instances [i].InitialiseVertices(Camera.main.orthographicSize * Camera.main.aspect * 2  + 1);				

			AttachGrass (i);
		}
	}

	void AttachGrass(int index){
		int from_ = index * (_grassAmount / _instanceCount);
		int to_ = ((index + 1) * (_grassAmount / _instanceCount));
		for (int i = from_; i < to_; ++i) {
			_grassInstances [i].transform.parent = _instances [index].transform;
			_grassInstances [i].transform.localScale = Vector3.one * _grassInstances[i].transform.localScale.x * Random.Range (0.4f, 1.7f);
			float normalizedValue_ = (i-from_) * _instances [index].GetLength() / (to_ - from_);
			float distance_ = Random.Range (normalizedValue_ - 0.2f,normalizedValue_ + 0.8f);
			_grassInstances [i].transform.localPosition = Vector3.right * distance_ + Vector3.up * (_instances[index].transform.localScale.y * 0.98f);
			_grassInstances [i].transform.localRotation = _grassInstances[i].transform.rotation * Quaternion.AngleAxis (Random.Range (-50, 50), Vector3.up);
		}
	}
		
	public void UpdateVertices(){
		InitialiseInstances ();
	}

	void RegenerateGround(int index){
		int modulatedIndex_ = index - 1 < 0 ? _instances.Length -1 : index - 1;
		line lastInstance_ = _instances [modulatedIndex_];
		_instances [index].transform.position = lastInstance_.transform.position + Vector3.right * lastInstance_.GetLength ();
	}

	void TeleportGroundToViewport(){
		float viewportMin_ = 1.0f;
		for (int index = 0; index < _instances.Length; ++index) {
			viewportMin_ = Mathf.Min(viewportMin_, Camera.main.WorldToViewportPoint (_instances [index].transform.position).x);
		}

		if (viewportMin_ <= 0) {
			return;
		}

		float worldDistance_ = Camera.main.ViewportToWorldPoint (Vector3.right * viewportMin_).x;
		Debug.Log ("world distance: " + worldDistance_);
		for (int i = 0; i < _instances.Length; ++i) {
			_instances [i].transform.position += Vector3.left * worldDistance_;
		}
	}

	public void Refresh(){
		TeleportGroundToViewport ();
		for (int i = 0; i < _instances.Length; ++i) {
			ClampGround (i);	
		}
	}

	void ClampGround(int index){
		if (Camera.main.WorldToViewportPoint (_instances[index].transform.position + Vector3.right * _instances[index].GetLength()).x < 0) {
			RegenerateGround (index);
		}
	}

	void Update(){
		Refresh ();
	}
}