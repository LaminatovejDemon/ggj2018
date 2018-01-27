using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homoPacketus : MonoBehaviour {

	public enum state{
		Alive,
		Idling,
		Rush,
		Attack,
		Dead,
	}

	state _state;
	float _stateDuration;
	float _nextIdleTimestamp;
	float _verticalPosition;
	float _lowerBound = 0.4f;
	float _upperBound = 0.6f;
	public GameObject _spearTemplate;
	GameObject _spear;
	GameObject _target;

	public void SetBounds(float lower, float upper){
		_lowerBound = lower;
		_upperBound = upper;
	}

	public state State(){
		return _state;
	}

	public void Start(){
		Initialise ();
	}

	public void Initialise(){
		_state = state.Alive;
		_stateDuration = 0;
		_nextIdleTimestamp = Random.Range (0.5f, 0.7f);
		_verticalPosition = transform.position.y;
		_spear = GameObject.Instantiate (_spearTemplate);
		_spear.SetActive (false);
	}

	public void Attack(GameObject target){
		if (_state != state.Dead) {
			GetComponent<Animator> ().SetTrigger ("throw");
			_state = state.Attack;
			_target = target;
		}
	}

	public void CalculateNextIdle(){
		_stateDuration = 0;
		_nextIdleTimestamp = Random.Range (1.5f, 6.5f);
	}

	public void Die(){
		_state = state.Dead;
	}

	public void OnThrow(){
		_spear.transform.position = _spearTemplate.transform.position;
		_spear.transform.rotation = _spearTemplate.transform.rotation;
		Interpolate (_spear, _target, 0.5f);
	}


	GameObject interpWhat, interpWhere;
	float interpSpeed;
	bool interpEnabled;

	void Interpolate(GameObject what, GameObject target, float speed){
		interpEnabled = true;
		interpWhat = what;
		interpWhere = target;
		interpSpeed = speed;
	}

	void UpdateInterpolate(){
		if (!interpEnabled) {
			return;
		}

		Vector3 currentVector_ = (interpWhere.transform.position - interpWhat.transform.position).normalized;
		Vector3 step_ = currentVector_ * interpSpeed;
		Vector3 plannedVector_ = (interpWhere.transform.position - (interpWhat.transform.position + step_)).normalized;

		if (Vector3.Dot (currentVector_, plannedVector_) < 0) {
			interpWhat.transform.position = interpWhere.transform.position;
			interpWhat.transform.parent = interpWhere.transform;
			interpEnabled = false;
			Debug.Log ("Hit");
			return;
		} else {
			interpWhat.transform.position += step_;
		}
	}
		
	// Update is called once per frame
	void Update () {
		UpdateInterpolate ();
//		if ( _state == state.Attack && GetComponent<Animator>()

		if (_state == state.Alive && homoMngr.instance.AnyoneWalking(this) ) {
			_stateDuration += Time.deltaTime;
			if (_stateDuration > _nextIdleTimestamp) {
				GetComponent<Animator> ().SetTrigger ("idle");
				_state = state.Idling;
			}
		}
	
		if ((_state == state.Rush && (Camera.main.WorldToViewportPoint (transform.position).x > _upperBound ))
			|| !homoMngr.instance.AnyoneWalking(this) ) {
			_state = state.Alive;
			GetComponent<Animator>().SetTrigger("walk");
			CalculateNextIdle ();
		}

		if (_state == state.Idling ) {
			if ( Camera.main.WorldToViewportPoint(transform.position).x < _lowerBound ){
				_state = state.Rush;
				GetComponent<Animator> ().SetTrigger ("rush");
			}
		}

		Vector3 bak_ = transform.position;
		bak_.y = _verticalPosition;
		transform.position = bak_;
	}
}
