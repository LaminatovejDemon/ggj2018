﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScrp : MonoBehaviour {

    Animator animator;
    float swapTime, speedDrift, speedSwim, timer;
    bool drift = true;
    float desendSpeed = 0.2f;
    homoPacketus victim;
    float timeStartedAttack;
    bool grabVictim = false;
    public GameObject[] _exclamationMarks = new GameObject[3];
    int startingCompleteLineNo = -1;
    int completeNo = 0;
    int difComplete;
    public GameObject _claw;
    public int key;
//    public int arrayListID;

	enum state {
		Flying,
		Dying,
	};

	state _state;

	void Start () {
		_state = state.Flying;
        animator = GetComponent<Animator>();
        animator.SetFloat("drift", 0);
        animator.speed = 0.4f;
        timer = Time.time;
        swapTime = Random.Range(5.0f, 9.0f);
        desendSpeed = Random.Range(0.025f, 0.1f);
        for (int i = 0; i < 3; ++i) {
            _exclamationMarks[i].SetActive(false);
        }
    }

    void increase() {
        if (animator.speed < speedSwim)
        {

            animator.speed = animator.speed + 0.01f;
        }
    }

	public bool IsAvailableToAttack(){
		return _state == state.Flying;
	}
    
	void capture()
    {
        victim = homoMngr.instance.AnyoneAround(Camera.main.WorldToViewportPoint(gameObject.transform.position).x);
        if (victim != null)
        {
            animator.SetTrigger("attack");
            timeStartedAttack = Time.time;
            grabVictim = true;
            removeFromList();
            //Debug.Log("found victim"+ victim.transform.position);

        }
    }

	public void ResetTriggers(){
		animator.ResetTrigger ("die");
		animator.ResetTrigger ("attack");
	}

	public void Die(){
		_state = state.Dying;
		ResetTriggers ();
		animator.SetTrigger("die");
        Camera.main.GetComponent<main>().scoreMngr.birdScore++;
        removeFromList();
	}

    public void OnAttackFinished()
    {
        homoMngr.instance.VictimIsGone(victim);
        Destroy(gameObject);
    }

	public void OnDeathFinished(){
		Destroy (gameObject);
	}
		
    void removeFromList() {
        enemyMngr.instance.removeBird(this);
        
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

    void desend() {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - (desendSpeed*Time.deltaTime), gameObject.transform.position.z);
    }

    void holdVictim() {
		if (timeStartedAttack + 0.5f < Time.time) {
            //victim.transform.parent = _claw.transform;
			victim.transform.position = (victim.transform.position - victim._heel.transform.position) + _claw.transform.position;
            
            victim.Die();
            //Debug.Log("holding");
        }
    }

	bool _safeBird = false;
	public void SafeBird(){
		_safeBird = true;
	}

    void drawExlanations() {
        difComplete = completeNo - startingCompleteLineNo;
        if (difComplete == 3) {
            _exclamationMarks[0].SetActive(true);
            _exclamationMarks[1].SetActive(true);
            _exclamationMarks[2].SetActive(true);
        }
        else if (difComplete == 2)
        {
            _exclamationMarks[0].SetActive(true);
            _exclamationMarks[1].SetActive(true);
        }
        if (difComplete == 1)
        {
            _exclamationMarks[0].SetActive(true);
        }
    }
	void Update () {

		if (_state != state.Flying) {
			return;
		}

	

        if (startingCompleteLineNo < 0) startingCompleteLineNo = lineMngr.instance.completeNo;

        idle();
        swap();


		if (_safeBird) {
			return;
		}

        completeNo = lineMngr.instance.completeNo;
        //Debug.Log("starting" + startingCompleteLineNo + "    new" + completeNo);
		if (startingCompleteLineNo+3 <= completeNo )
        {
			if (!grabVictim  )
            {
               capture();
            }
            else if (victim != null)
			{
                holdVictim();
            }
        }
        drawExlanations();

	}
}
