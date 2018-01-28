using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMngr : baseMngr<enemyMngr>
{

	public BirdScrp enemyTemplate;
	BirdScrp[] enemyList;
    int enemyOnScreen = 0;
    bool[] freeSeg = new bool[10];
    int targetSeg;

	void Initialise(){
		if (enemyList != null) {
			return;
		}

		enemyList = new BirdScrp[10];
		for (int i = 0; i < 10; ++i)
		{
			enemyList[i] = null;
            freeSeg[i] = true;
		}
	}

    public void create()
    {
		Initialise ();
        targetSeg = -1;
        if (enemyOnScreen < 9)
        {
            for (int i = 0; i < 8; i++)
            {
                if (enemyList[i] == null)
                {
					enemyList[i] = GameObject.Instantiate(enemyTemplate).GetComponent<BirdScrp>();
					enemyList [i].transform.parent = Camera.main.transform;
                    for (int a = 0; a < 10000; ++a)
                    {
                        targetSeg = Random.Range(1, 9);
                        if (freeSeg[targetSeg]) {
                            enemyList[i].transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.right * ((0.05f)+(targetSeg*0.1f))).x, Camera.main.ViewportToWorldPoint(Vector3.one * 0.9f).y, -10);
                            freeSeg[targetSeg] = false;
                            enemyList[i].segmentID = targetSeg;
                            return;
                        }
                    }
                
					//enemyList[i].transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.right * posX).x, Camera.main.ViewportToWorldPoint(Vector3.one * 0.9f).y, -10);
                    enemyOnScreen++;
                    return;
          	     }    
            }
        }
    }

	public BirdScrp TestEnemy(float startpoint, float endPoint) {
		Initialise ();

        for (int i = 0; i < 10; i++)
        {
			if (enemyList [i] != null) {
				float posX_ = Camera.main.WorldToViewportPoint (enemyList [i].transform.position).x;
				if (posX_ > startpoint && posX_ < endPoint) {
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");
            create();
        }
        
    }
}
