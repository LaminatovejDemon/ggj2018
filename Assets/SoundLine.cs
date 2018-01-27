using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLine : MonoBehaviour {

    int LineCubeNom = 512;
    int frameLine = 0;
    int increamentPerFrame = 10;

    GameObject[] line;
    public GameObject template;

    float target = 1f;
    float increamentSpeed = 0.5f;
    
	void Start () {
        line = new GameObject[LineCubeNom];
        Vector3 screen = Camera.main.ViewportToWorldPoint(Vector3.zero);
        float step_ = Camera.main.orthographicSize * Camera.main.aspect / LineCubeNom * 2;

        for (int i = 0; i < LineCubeNom; ++i) {
            line [i] = GameObject.Instantiate(template);
            line[i].transform.parent = gameObject.transform;
            line[i].transform.position = screen + Vector3.right * step_ * i;

        }
        

    }


    void Update () {

        // target = microphoneMngr.instance.GetMicrophoneValue() * 1000.0f;
        target = 0 - 2;
        if (frameLine > LineCubeNom-increamentPerFrame) frameLine = 0;
        Debug.Log("" + target);
        increamentPerFrame = (int)(100 * Time.deltaTime);
        for (int i = 0; i < increamentPerFrame; ++i)
        {
            if (frameLine != 0)
            {
                if (line[frameLine + i - 1].transform.localPosition.y < target - 0.5f)
                {
                    line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[frameLine + i - 1].transform.localPosition.y + increamentSpeed, 0);
                }
                else if (line[frameLine + i - 1].transform.localPosition.y > target + 0.5f)
                {
                    line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[frameLine + i - 1].transform.localPosition.y - increamentSpeed, 0);

                }

            }
            else {
                if (line[LineCubeNom - 1].transform.localPosition.y < target)
                {
                    line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[LineCubeNom - 1].transform.localPosition.y + increamentSpeed, 0);
                }
                else if (line[LineCubeNom - 1].transform.localPosition.y > target)
                {
                    line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[LineCubeNom - 1].transform.localPosition.y - increamentSpeed, 0);

                }
            }
            //else if (line[frameLine + i].transform.position.y <= Camera.main.ViewportToWorldPoint(Vector3.zero).y) increamentSpeed *= -1;
        }
        frameLine += increamentPerFrame;
	}
}