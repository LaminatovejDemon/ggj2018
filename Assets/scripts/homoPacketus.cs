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
    public GameObject _heel;

    GameObject _spear;
	BirdScrp _target;
	bool _throwPull = false;

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
		_spear.transform.parent = _spearTemplate.transform;
		_spear.transform.localScale = Vector3.one;
		_spear.transform.parent = _spearTemplate.transform.parent;
		_spear.SetActive (false);
	}

	public void Attack(BirdScrp target){
		if (_state != state.Dead) {
			if (target != null) {
				GetComponent<Animator> ().SetTrigger ("throw");
				_state = state.Attack;
				_target = target;
			} else if (target == null) {
				GetComponent<Animator> ().SetTrigger ("suicide");
				_state = state.Dead;
				_spearTemplate.SetActive (true);
			}
		}
	}

	public void CalculateNextIdle(){
		_stateDuration = 0;
		_nextIdleTimestamp = Random.Range (1.5f, 6.5f);
	}

	public void Die(){
		if (_state == state.Dead) {
			return;
		}
		_state = state.Dead;
		GetComponent<Animator>().SetTrigger("victim");
	}

	public void OnThrowPull(){
		_spear.transform.parent = null;
		_spear.transform.position = _spearTemplate.transform.position;
		_spear.transform.rotation = _spearTemplate.transform.rotation;
		_spear.SetActive (true);
		_spearTemplate.SetActive (false);
		_throwPull = true;
	}

	public void OnThrowRelease(){
		Interpolate (_spear, _target, 10.0f);
		_throwPull = false;
	}

	GameObject interpWhat;
	BirdScrp interpWhere;
	float interpSpeed;
	bool interpEnabled;

	void Interpolate(GameObject what, BirdScrp target, float speed){
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
		Vector3 step_ = currentVector_ * interpSpeed * Time.deltaTime;
		Vector3 plannedVector_ = (interpWhere.transform.position - (interpWhat.transform.position + step_)).normalized;

		if (Vector3.Dot (currentVector_, plannedVector_) < 0) {
			interpWhat.transform.position = interpWhere.transform.position;
			interpWhat.transform.parent = interpWhere.transform;
			interpEnabled = false;
            _spear.transform.parent = _spearTemplate.transform;
			_spear.transform.localScale = Vector3.one;
            _spear.gameObject.SetActive(false);
            _spearTemplate.gameObject.SetActive(true);
			interpWhere.Die ();
			return;
		} else {
			interpWhat.transform.position += step_;
			HomingSpear ();
		}
	}

	void HomingSpear(){
		Vector3 differnce_ = (_target.transform.position - _spear.transform.position);
		differnce_.z = 0;
		float bak_ = differnce_.y;
		differnce_.y = -differnce_.x;
		differnce_.x = bak_;
	
		_spear.transform.rotation = Quaternion.LookRotation (differnce_.normalized);
	}

	void UpdateThrowPull(){
		if (!_throwPull) {
			return;
		}
		_spear.transform.position = _spearTemplate.transform.position;
		HomingSpear ();
	}
		
	// Update is called once per frame
	void Update () {
		if (_state == state.Dead) {
			return;
		}

		UpdateInterpolate ();
		UpdateThrowPull ();
//		bool anyoneWalking = homoMngr.instance.AnyoneWalking (this);
		_stateDuration += Time.deltaTime;
		float camX_ = Camera.main.WorldToViewportPoint (transform.position).x;
		bool behind = camX_ < _lowerBound;
		bool ahead = camX_ > _upperBound;
		bool wayahead = camX_ > 0.8f;

		if ((_state == state.Alive /*&& anyoneWalking*/ && !behind) || (wayahead) ) {
			if (_stateDuration > _nextIdleTimestamp) {
				GetComponent<Animator> ().SetTrigger ("idle");
				_state = state.Idling;
			}
		}
	
		if ((_state == state.Rush && ahead) /*|| !anyoneWalking*/ ) {
			_state = state.Alive;
			GetComponent<Animator>().SetTrigger("walk");
			CalculateNextIdle ();
		}

		if ((_state == state.Alive || _state == state.Idling) &&  behind ) {
			{
				GetComponent<Animator> ().SetTrigger ("rush");
				_state = state.Rush;
			}
		}

		Vector3 bak_ = transform.position;
		bak_.y = _verticalPosition;
		transform.position = bak_;
	}

	void OnSuicideFinished(){
		homoMngr.instance.NotifyDead (this);
	}
}
