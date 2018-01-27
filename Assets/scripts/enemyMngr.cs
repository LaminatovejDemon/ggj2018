using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMngr : MonoBehaviour
{

    public GameObject enemyTemplate;
    GameObject[] enemyList;
    int enemyOnScreen = 0;
    void Start()
    {
        enemyList = new GameObject[10];
        for (int i = 0; i < 10; ++i)
        {
            enemyList[i] = null;
        }
        
    }
    public void create(float posX)
    {
        if (enemyOnScreen < 10)
        {
            for (int i = 0; i < 10; i++)
            {
                if (enemyList[i] == null)
                {
                    enemyList[i] = GameObject.Instantiate(enemyTemplate);
                    enemyList[i].transform.position = new Vector3(posX, Camera.main.ViewportToWorldPoint(Vector3.one * 1.2f).y, -10);
                    enemyOnScreen++;
                    return;
                }
                
            }
            
        }

    }
    public void TestEnemy(float startpoint, float endPonit) {
        for (int i = 0; i < 10; i++)
        {
            if (enemyList[i] == null)
            {
                if (enemyList[i].transform.position.x > Camera.main.ViewportToWorldPoint(Vector3.right * startpoint).x
                    && enemyList[i].transform.position.x < Camera.main.ViewportToWorldPoint(Vector3.right * endPonit).x )
                {
                    Destroy(enemyList[i], 0.5f);
                }
                
                
               
            }

        }

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
