using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLine : MonoBehaviour {

    int LineCubeNom = 32;
    int frameLine = 0;
    float frameLineIncreament = 0;
    GameObject[] line;
    public GameObject template;
    int lastValueAccessed = 0;
    
	void Start () {
        line = new GameObject[LineCubeNom];
        Vector3 screen = Camera.main.ViewportToWorldPoint(Vector3.zero);
        float step_ = Camera.main.orthographicSize * Camera.main.aspect / LineCubeNom * 2;

        for (int i = 0; i < LineCubeNom; ++i) {
            line [i] = GameObject.Instantiate(template);
            line[i].transform.position = screen + Vector3.right * step_ * i;

        }
	}


    void Update () {

        frameLine++;
        //frameLineIncreament += 1 * Time.deltaTime;
        //frameLine += (int)frameLineIncreament;
        if (frameLine > LineCubeNom) frameLine = 0;
   //     Debug.Log("" + frameLine);

        int i = lastValueAccessed % LineCubeNom;
        for (; i < (int)(Time.time * (float)LineCubeNom) % LineCubeNom; ++i)
        {
            line[frameLine].transform.position = new Vector3(line[frameLine].transform.position.x, line[frameLine].transform.position.y + 0.1f, 0);
        }
        lastValueAccessed = i;
	}
}
