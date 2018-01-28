using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class baseMngr<T> : MonoBehaviour 
	where T : baseMngr<T>
{
	static baseMngr<T> _instance = null;

	public static T instance {
			
		get{
			if (_instance == null) {
				//Debug.Log ("Instatiating " + typeof(T).ToString ());
				_instance = Object.Instantiate<baseMngr<T>>(Resources.Load<baseMngr<T>>(typeof(T).ToString ()));
				_instance.name = "#" + typeof(T).ToString ();
			}
			return _instance as T;
		}

		private set{
			instance = value;
		}
	}

	public void Drop(){
		GameObject.Destroy (this);
		_instance = null;
	}
}
