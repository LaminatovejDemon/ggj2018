using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLine : MonoBehaviour {

    int LineCubeNom = 1024;
    int frameLine = 0;
    int increamentPerFrame = 10;
    public float maxSoundLocation;

    GameObject[] line;
    public GameObject template;

    float target = -0f;
    float increamentSpeed = 0.5f;
    float oldPosition;
    
	void Start () {
        line = new GameObject[LineCubeNom];
        Vector3 screen = Camera.main.ViewportToWorldPoint(Vector3.zero);
        float step_ = Camera.main.orthographicSize * Camera.main.aspect / LineCubeNom * 2;

        for (int i = 0; i < LineCubeNom; ++i) {
            line [i] = GameObject.Instantiate(template);
            line[i].transform.parent = gameObject.transform;
            line[i].transform.position = screen + Vector3.right * step_ * i;
            line[i].transform.localPosition = new Vector3(line[i].transform.localPosition.x, target, line[i].transform.localPosition.z);

        }
        

    }


    void Update () {

        target = (microphoneMngr.instance.GetMicrophoneValue() * 350.0f)-2;
        if (frameLine > LineCubeNom - increamentPerFrame) frameLine = 0;
        if (target > 2)
        {
            target = 2;
            maxSoundLocation = line[frameLine].transform.position.x;
            Debug.Log("" + maxSoundLocation);
        }
        
        increamentPerFrame = (int)(200 * Time.deltaTime);
        for (int i = 0; i < increamentPerFrame; ++i)
        {
            
            if (frameLine != 0)
            {
                oldPosition = line[frameLine + i].transform.localPosition.y;
                if (line[frameLine + i - 1].transform.localPosition.y <= target - (increamentSpeed/2))
                {
                    line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[frameLine + i - 1].transform.localPosition.y + increamentSpeed, line[frameLine + i].transform.localPosition.z);
                }
                else if (line[frameLine + i - 1].transform.localPosition.y >= target + (increamentSpeed / 2))
                {
                    line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[frameLine + i - 1].transform.localPosition.y - increamentSpeed, line[frameLine + i].transform.localPosition.z);

                }
                else line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[frameLine + i - 1].transform.localPosition.y, line[frameLine + i].transform.localPosition.z);

            }
            else {
                if (line[LineCubeNom - 1].transform.localPosition.y < target )
                {
                    line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[LineCubeNom - 1].transform.localPosition.y + increamentSpeed, line[frameLine + i].transform.localPosition.z);
                }
                else if (line[LineCubeNom - 1].transform.localPosition.y > target)
                {
                    line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[LineCubeNom - 1].transform.localPosition.y - increamentSpeed, line[frameLine + i].transform.localPosition.z);

                }
                else line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[LineCubeNom - 1].transform.localPosition.y, line[frameLine + i].transform.localPosition.z);

            }

            if (oldPosition == line[frameLine + i].transform.localPosition.y) line[frameLine + i].transform.localPosition = new Vector3(line[frameLine + i].transform.localPosition.x, line[frameLine + i].transform.localPosition.y + 0.001f, line[frameLine + i].transform.localPosition.z);

        }
        frameLine += increamentPerFrame;
	}
}