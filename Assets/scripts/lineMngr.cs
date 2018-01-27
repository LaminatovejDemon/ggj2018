using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineMngr : baseMngr<lineMngr> {

    int LineCubeNom = 1024;
    int frameLine = 0;
    int increamentPerFrame = 10;
    public float maxSoundLocation;

    GameObject[] line;
    public GameObject template;

    float target = -0f;
    float increamentSpeed = 0.25f;
    float oldPosition;
    

	public void Initialise(){
		if (line != null) {
			return;
		}

		line = new GameObject[LineCubeNom];
		Vector3 screen = Camera.main.ViewportToWorldPoint(Vector3.zero);
		float step_ = Camera.main.orthographicSize * Camera.main.aspect / LineCubeNom * 2;

		Transform container_ = new GameObject ("soundLineContainer").transform;
		container_.localScale = new Vector3 (0.01f, 0.5f, 1.0f);
		container_.position = Vector3.down * 2.5f;
		container_.parent = Camera.main.transform;


		for (int i = 0; i < LineCubeNom; ++i) {
			line [i] = GameObject.Instantiate(template);
			line[i].transform.parent = container_;
			line[i].transform.position = screen + Vector3.right * step_ * i;
			line[i].transform.localPosition = new Vector3(line[i].transform.localPosition.x, target, line[i].transform.localPosition.z);
			/*			Vector2 scaleBak_ = line [i].transform.localScale;
			scaleBak_.x = step_;
			line [i].transform.localScale = scaleBak_;*/

		}
	}

	void DecayColors(){
		Material current_;
		for (int i = 0; i < LineCubeNom; ++i) {
			current_ = line [i].transform.GetChild(0).GetComponent<Renderer> ().material;
			Color color_ = current_.GetColor ("_TintColor");
			if ( color_.a > 0 ){
				color_.a -= Time.deltaTime;
				if (color_.a < 0) {
					color_.a = 0;
				}
			}
			current_.SetColor ("_TintColor", color_);
		}
	}

	void BoostColor(int index){
		Material current_ = line [index].transform.GetChild(0).GetComponent<Renderer> ().material;
		Color color_ = current_.GetColor ("_TintColor");
		color_.a = 1;
		current_.SetColor ("_TintColor", color_);
	}

	void ScaleCube(GameObject cube, GameObject reference)
	{
		Vector3 scale_ = cube.transform.localScale;
		scale_.y = reference.transform.position.y - cube.transform.position.y;
		cube.transform.localScale = scale_;
	}
		
    void Update () {

        target = (microphoneMngr.instance.GetMicrophoneValue() * 50.0f)-2;
        if (target > 2)
        {
            target = 2;
            maxSoundLocation = line[frameLine].transform.position.x;
//            Debug.Log("" + maxSoundLocation);
        }
        
		increamentPerFrame = (int)(250 * Time.deltaTime);
		if (frameLine > LineCubeNom - increamentPerFrame - 1) frameLine = 0;

		DecayColors ();

        for (int i = 0; i < increamentPerFrame; ++i)
        {
			BoostColor (frameLine + i);


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

//				ScaleCube (line [frameLine + i - 1], line [frameLine + i]);
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

			shoutMngr.instance.SetSweep((frameLine + i) / (float)LineCubeNom, (2.0f+line[frameLine + i].transform.localPosition.y) / 2.0f);
        }
        frameLine += increamentPerFrame;

	}
}