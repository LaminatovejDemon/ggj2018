﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMngr : baseMngr<enemyMngr>
{

	public BirdScrp enemyTemplate;
	BirdScrp[] enemyList;
    int enemyOnScreen = 0;
    bool[] freeSeg = new bool[10];
    int targetSeg;
    int maxNoSeg;

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
        maxNoSeg = shoutMngr.instance._segmentCount -2;
        targetSeg = -1;
        if (enemyOnScreen < 9)
        {
            for (int i = 0; i < maxNoSeg; i++)
            {
                if (enemyList[i] == null)
                {
					enemyList[i] = GameObject.Instantiate(enemyTemplate).GetComponent<BirdScrp>();
					enemyList[i].transform.parent = Camera.main.transform;
                    int temp = Random.Range(0, maxNoSeg+1);
                    for (int a = 1; a < maxNoSeg+1; ++a)
                    {
                        int id = (a + temp) % maxNoSeg+1;
                        if (freeSeg[id]) {
                            enemyList[i].transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.right * ((0.05f)+(id* shoutMngr.instance.GetViewPortSegmentSize()))).x, Camera.main.ViewportToWorldPoint(Vector3.one * 0.9f).y, -10);
                            freeSeg[id] = false;
                            enemyList[i].segmentID = id;
                            enemyList[i].arrayListID = i;
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
    public void removeBird(int listID, int segmentID) {
        enemyList[listID] = null;
        freeSeg[segmentID] = true;
        enemyOnScreen--;
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
        /*
        Debug.Log("" + enemyOnScreen);
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
        */
        if (enemyOnScreen == 0) {
            int temp = Random.Range(1, 9);
            for (int i = 0; i < temp + 1; ++i) create();
        }
    }
}
