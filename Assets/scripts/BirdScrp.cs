﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScrp : MonoBehaviour {

    Animator animator;
    float swapTime, speedDrift, speedSwim, timer;
    bool drift = true;


	void Start () {
        animator = GetComponent<Animator>();
        animator.SetFloat("drift", 0);
        animator.speed = 0.4f;
        timer = Time.time;
        swapTime = Random.Range(5.0f, 9.0f);
    }
    void increase() {
        if (animator.speed < speedSwim)
        {

            animator.speed = animator.speed + 0.01f;
        }
    }

	public void Die(){
		Debug.Log ("bird is dying");
		//TODO
	}

    void decrease() {
        if (animator.speed > speedDrift)
        {
            animator.speed = animator.speed - 0.01f;
        }
    }

    void swap() {
        if (drift) {
            if (animator.GetFloat("drift") > 0)
            {
                animator.SetFloat("drift", animator.GetFloat("drift") - Time.deltaTime * 3.0f);
            }
        }
        else {
            if (animator.GetFloat("drift") < 1)
            {
                animator.SetFloat("drift", animator.GetFloat("drift") + Time.deltaTime * 3.0f);
            }
        }
    }

    void idle() {
        if (drift)
        {
            //decrease();
            if (timer + swapTime < Time.time)
            {
                //animator.SetFloat("drift", 1);
                speedSwim = Random.Range(1.5f, 2.0f);
                
                timer = Time.time;
                swapTime = Random.Range(1.0f, 4.0f);
                drift = false;
            }
           

        }
        else
        {
            
            //increase();
            if (timer + swapTime < Time.time)
            {
                //animator.SetFloat("drift", 0);
                speedDrift = 0.4f;
                timer = Time.time;
                swapTime = Random.Range(2.0f, 5.0f);
                drift = true;
            }
            

        }

    }
	
	// Update is called once per frame
	void Update () {
        idle();
        swap();
      
	}
}
