using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homoMngr : baseMngr<homoMngr> {

	public int _homoCount = 10;
	public homoPacketus _homoTemplate;
	List<homoPacketus> _homoInstance;
	public GameObject _container;
//	cameraLink _linkedInstance;

	public void Initialise(){
		if (_homoInstance != null) {
			return;
		}
//		_linkedInstance = null;
		_homoInstance = new List<homoPacketus>();
		_container = new GameObject ();
		_container.name = "#homoContaier";
		_container.transform.position = Vector3.zero;

		for (int i = 0; i < _homoCount; ++i) {
			homoPacketus new_ = GameObject.Instantiate (_homoTemplate);
			Vector3 pos_ = new_.transform.position;
			pos_.x = Camera.main.transform.position.x;
			new_.transform.position = pos_;
			new_.transform.position += Vector3.left * (i - _homoCount*0.5f) + Vector3.right * (Camera.main.orthographicSize * Camera.main.aspect + 2);

			new_.SetBounds (0.3f / _homoCount * i + 0.1f, 0.3f / _homoCount * i + 0.5f);
			new_.name = "HomoPacketus_" + i;
			new_.transform.parent = _container.transform;
			_homoInstance.Add(new_);
		}
//		AssignCameraLink ();
	}

	public void NotifyDead(homoPacketus source){
		source.transform.parent = _container.transform;
		_homoInstance.Remove (source);

		if (_homoInstance.Count == 0) {
			restartMngr.instance.Wipe ();
		}
	}

	public void Attack(BirdScrp target){
		int from_ = Random.Range (0, _homoInstance.Count);

		for (int i = 0; i < _homoInstance.Count; ++i) {
			int realIndex_ = (i + from_) % _homoInstance.Count;

			if (_homoInstance [realIndex_].State () == homoPacketus.state.Alive
				|| _homoInstance[realIndex_].State() == homoPacketus.state.Idling
				|| _homoInstance[realIndex_].State() == homoPacketus.state.Rush) {
				_homoInstance [realIndex_].Attack (target);
				return;
			}
		}
	}

	public homoPacketus AnyoneAround(float viewportPosition){

		float tolerance = shoutMngr.instance.GetViewPortSegmentSize () * 0.5f;

		for (int i = 0; i < _homoInstance.Count; ++i) {
			if (_homoInstance [i].State () != homoPacketus.state.Attack &&
			    _homoInstance [i].State () != homoPacketus.state.Dead)
			{
				float distance_ = Mathf.Abs (Camera.main.WorldToViewportPoint (_homoInstance [i].transform.position).x - viewportPosition);
				
				if ( distance_ < tolerance) {
					return _homoInstance[i];
				}
			}
		}
		return null;
	}

	public bool AnyoneWalking(homoPacketus exception){
		for ( int i = 0; i < _homoInstance.Count; ++i ){
			if (_homoInstance[i] != exception && _homoInstance [i].State () == homoPacketus.state.Alive) {
				return true;
			}
		}
		return false;
	}

    public void VictimIsGone(homoPacketus victim)
    {
		NotifyDead (victim);
    }

}
