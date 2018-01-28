﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMngr : baseMngr<enemyMngr>
{

	public BirdScrp enemyTemplate;
	Hashtable enemyList;

//    int enemyOnScreen = 0;
  //  bool[] freeSeg;
    int targetSeg;
    int maxNoSeg;
	public GameObject _container;
	bool _semiWaveAddition = false;

	public void Initialise(){
		if (enemyList != null) {
			return;
		}
		//freeSeg = new bool[shoutMngr.instance._segmentCount];

		_container = new GameObject ();
		_container.name = "#birds";
		_container.transform.parent = Camera.main.transform;

		enemyList = new Hashtable();
//		for (int i = 0; i < shoutMngr.instance._segmentCount; ++i)
//		{
//			enemyList.Add (null);
//            freeSeg[i] = true;
//		}
	}

	public void create(int prefferedSlot = -1)
    {
		Initialise ();
		maxNoSeg = shoutMngr.instance._segmentCount > 5 ? 1 : 0;
        targetSeg = -1;
		if (enemyList.Count < shoutMngr.instance._segmentCount - (maxNoSeg*2) -1 )
        {
			int temp = prefferedSlot == -1 ? Random.Range(0, maxNoSeg+1) : prefferedSlot;
			for (int a = maxNoSeg; a < shoutMngr.instance._segmentCount - (2 * maxNoSeg); a++)
            {
				int id = (a + temp) % shoutMngr.instance._segmentCount - maxNoSeg;
				if (id < maxNoSeg) {
					++id;
				}

				if (!enemyList.Contains(id)) {
					enemyList[id] = GameObject.Instantiate(enemyTemplate).GetComponent<BirdScrp>();
					(enemyList[id] as BirdScrp).transform.parent = _container.transform;
					(enemyList[id] as BirdScrp).transform.position = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.right * (((id+0.5f)* shoutMngr.instance.GetViewPortSegmentSize()))).x, Camera.main.ViewportToWorldPoint(Vector3.one * 0.9f).y, -10);
					(enemyList[id] as BirdScrp).key = id;
					return;
                }
			}
		}    
	}

	public void removeBird(BirdScrp source) {
		enemyList.Remove(source.key);
		source.key = -1;
    }

	public BirdScrp TestEnemy(float startpoint, float endPoint) {
		Initialise ();

		for (int i = 0; i < shoutMngr.instance._segmentCount; i++)
        {
			if (enemyList [i] != null) {
				float posX_ = Camera.main.WorldToViewportPoint ((enemyList[i] as BirdScrp).transform.position).x;
				if (posX_ > startpoint && posX_ < endPoint && (enemyList[i] as BirdScrp).IsAvailableToAttack() ) {
					return (enemyList[i] as BirdScrp);
				} 
			}
        }
		return null;
    }

	public void PrepareEnemy(int number, int prefererSlot = -1){
		for (int i = 0; i < number; i++) { 
			create(prefererSlot); 
		}
		Debug.Log("making new" + enemyList.Count);
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
		if (enemyList.Count == 0 || (enemyList.Count == 1 && _semiWaveAddition)) {
            int number = Random.Range(1, 4);
			_semiWaveAddition = (number == 2 && Random.value < 0.5f);
			PrepareEnemy (number);
        }
        //Debug.Log("" + enemyOnScreen);
    }
}
