using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMngr : baseMngr<enemyMngr>
{

    public GameObject enemyTemplate;
    GameObject[] enemyList;
    int enemyOnScreen = 0;

	void Initialise(){
		if (enemyList != null) {
			return;
		}

		enemyList = new GameObject[10];
		for (int i = 0; i < 10; ++i)
		{
			enemyList[i] = null;
		}
	}

    public void create(float posX)
    {
		Initialise ();

        if (enemyOnScreen < 10)
        {
            for (int i = 0; i < 10; i++)
            {
                if (enemyList[i] == null)
                {
                    enemyList[i] = GameObject.Instantiate(enemyTemplate);
					enemyList [i].transform.parent = Camera.main.transform;
					enemyList[i].transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.right * posX).x, Camera.main.ViewportToWorldPoint(Vector3.one * 1.2f).y, -10);
                    enemyOnScreen++;
                    return;
          	     }    
            }
        }
    }

	public GameObject TestEnemy(float startpoint, float endPoint) {
		Initialise ();

        for (int i = 0; i < 10; i++)
        {
			if (enemyList [i] != null) {
				float posX_ = Camera.main.WorldToViewportPoint (enemyList [i].transform.position).x;
				if (posX_ > startpoint && posX_ < endPoint) {
					Destroy (enemyList [i], 0.5f);
					return enemyList[i];
				} 
			}
        }
		return null;
    }

    void Update()
    {
        for (int i = 0; i < 10; ++i)
        {
            if (enemyList[i] != null)
            {
                //to do
            }
        }
        if (Input.GetKeyDown(KeyCode.Space)) create(0);
        
    }
}
