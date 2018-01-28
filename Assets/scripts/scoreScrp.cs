using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreScrp : MonoBehaviour {

    public Text birdKill;
    public Text homoKill;
    public int birdScore = 0;
    public int homoScore = 0;
	void Start () {
		
	}
	
	
	void Update () {
        if (birdScore < 10 ) birdKill.text = ("00"+birdScore.ToString());
        else if (birdScore < 100) birdKill.text = ("0" + birdScore.ToString());
        else birdKill.text = birdScore.ToString();

        if (homoScore < 10) homoKill.text = ("00" + homoScore.ToString());
        else if (homoScore < 100) homoKill.text = ("0" + homoScore.ToString());
        else homoKill.text = homoScore.ToString();
        
	}
}
