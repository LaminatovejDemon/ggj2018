using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homoMngr : baseMngr<homoMngr> {

	public int _homoCount = 10;
	public homoPacketus _homoTemplate;
	homoPacketus[] _homoInstance;
	cameraLink _linkedInstance;

	public void Initialise(){
		if (_homoInstance != null) {
			return;
		}
		_linkedInstance = null;
		_homoInstance = new homoPacketus[_homoCount];

		for (int i = 0; i < _homoCount; ++i) {
			_homoInstance [i] = GameObject.Instantiate (_homoTemplate);
			_homoInstance [i].transform.position += Vector3.left * (i - _homoCount*0.5f);
			_homoInstance [i].SetBounds (0.3f / _homoCount * i + 0.2f, 0.3f / _homoCount * i + 0.5f);
			_homoInstance [i].name = "HomoPacketus_" + i;
		}
		AssignCameraLink ();
	}

	public void Attack(BirdScrp target){
		int from_ = Random.Range (0, _homoCount);

		for (int i = 0; i < _homoCount; ++i) {
			int realIndex_ = (i + from_) % _homoCount;

			if (_homoInstance [realIndex_].State () != homoPacketus.state.Dead) {
				_homoInstance [realIndex_].Attack (target);
				return;
			}
		}
	}

	public bool AnyoneWalking(homoPacketus exception){
		for ( int i = 0; i < _homoCount; ++i ){
			if (_homoInstance[i] != exception && _homoInstance [i].State () == homoPacketus.state.Alive) {
				return true;
			}
		}
		return false;
	}

	void AssignCameraLink(){
		if (_linkedInstance != null && _linkedInstance.GetComponent<homoPacketus>().State () == homoPacketus.state.Alive) {
			return;
		} else if (_linkedInstance != null && _linkedInstance.GetComponent<homoPacketus>().State ()!= homoPacketus.state.Alive) {
			_linkedInstance.enabled = false;
		}
			
		for (int i = 0; i < _homoCount; ++i) {
			if (_homoInstance [i].State() == homoPacketus.state.Alive ) {
				_linkedInstance = _homoInstance [i].GetComponent<cameraLink>();
				_linkedInstance.GetComponent<cameraLink> ().enabled = true;
				return;
			}	
		}
	}

	void Update(){
		AssignCameraLink ();
	}
}
