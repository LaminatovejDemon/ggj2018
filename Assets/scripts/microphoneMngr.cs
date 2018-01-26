using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class microphoneMngr : MonoBehaviour {

	string _microphone;
	public AudioSource _audio;
	MicrophoneState _state;
	static microphoneMngr _instance;

	public static microphoneMngr instance {
		get{
			if (_instance == null) {
				_instance = GameObject.Instantiate<microphoneMngr>(Resources.Load<microphoneMngr>("microphoneMngr"));
				_instance.name = "#microphoneMngr";
			}
			return _instance;
		}
		private set{
			instance = value;
		}
	}
		
	enum MicrophoneState{
		Off,
		Initialized,
		Recording,
	};

	void InitAudio(){
		if (_state != MicrophoneState.Off) {
			return;
		} 

		for (int i = 0; i < Microphone.devices.Length; ++i) {
			if (Microphone.devices [i] != null) {
				_microphone = Microphone.devices [i];
				Debug.Log ("Our first device is " + _microphone);
				_audio.clip = Microphone.Start (_microphone, true, 16, 44100);
				_audio.loop = true;
				_state = MicrophoneState.Initialized;
			}
		}
	}

	void StartRecording(){
		if (_state != MicrophoneState.Initialized) {
			return;
		}

		if ( !_audio.isPlaying && Microphone.IsRecording(_microphone) ){
			if (Microphone.GetPosition (_microphone) == 0) {
				Debug.Log ("not recording");
				return;
			}

			_audio.Play ();
			_state = MicrophoneState.Recording;
		}
	}
		
	void Update () {
		InitAudio ();
		StartRecording ();
	}
		
	public float GetMicrophoneValue(){
		if (_state != MicrophoneState.Recording) {
			return 0;
		}

		float[] data = new float[735];
		_audio.GetOutputData (data, 0);
		ArrayList _array = new ArrayList ();
		for (int i = 0; i < 735; ++i) {
			_array.Add(Mathf.Abs(data[i]));
		}
		_array.Sort();
		return (float)_array [735 / 2];
	}
}
