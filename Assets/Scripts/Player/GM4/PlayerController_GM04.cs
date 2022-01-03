using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.Experimental.Rendering.Universal;
using Random=UnityEngine.Random;


public class PlayerController_GM04 : PlayerController_GM
{
  
    // private float speed = 20f;
    private float increasingSpeed;
    private float speedDelta;
    private float timerSpeedIncrease;
    private Vector3 mouseStartPos;
    private Vector3 dir;
  
  
    protected override void OnEnable() {
   
        speedDelta = 0f;
        increasingSpeed  = GetStartUpSpeed();
        ScoreManager.Instance.AddMiniGameXpCount(10); //giving a minimum xp value of 10 so this trigger the mini game recap animation 

        base.OnEnable();
    }
    protected override void OnDisable() {
        base.OnDisable();

    }

    protected override void FixedUpdate() {
        
        base.FixedUpdate();
    }

    protected override void HandleTutorialMovement() {
        
         playerInPosition = true;
         base.HandleTutorialMovement();
    }

    protected override void HandleStartingMovement() {
        timerSpeedIncrease += Time.deltaTime;

        if(timerSpeedIncrease > 3f) { 
            speedDelta += 0.5f;
            increasingSpeed  = GetStartUpSpeed() + speedDelta;
            timerSpeedIncrease = 0f;
        }
        

        
         transform.rotation = Quaternion.identity;
         transform.Translate(Vector3.up * increasingSpeed * Time.deltaTime);

        if(InputManager.getTouch(TouchPhase.Began)) {
            mouseStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseStartPos.y = transform.position.y;
            mouseStartPos.z = 0f;

            dir = transform.position - mouseStartPos;
          //   Debug.Log("touching");


        }
        
         
        if(InputManager.getTouch(TouchPhase.Moved)) 
        Move();
     //   Debug.Log("moving");
             
    }

    private void Move() {
        //get mouse position
         Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         mouseWorldPosition.y = transform.position.y;
         mouseWorldPosition.z = 0f;

       //transform.position = Vector3.MoveTowards(transform.position, mouseWorldPosition, speed * Time.deltaTime);
        transform.position = new Vector3(mouseWorldPosition.x + dir.x, transform.position.y, 0f);
      
    }

    public float GetStartUpSpeed() {

        if(LevelingSystem.Instance.GetLevelNumber() >= 8 && LevelingSystem.Instance.GetLevelNumber() < 12) {
           float x =  Mathf.RoundToInt(Mathf.Log((float) LevelingSystem.Instance.GetLevelNumber(), 3) * 2.5f);
           
            return x;
        }

        else if(LevelingSystem.Instance.GetLevelNumber() >= 12) {
          // float x =  Mathf.RoundToInt(Mathf.Log((float) LevelingSystem.Instance.GetLevelNumber(), 3) * 3f);
           
            return 6.5f;
        }
          
            
        else //if(LevelingSystem.Instance.GetLevelNumber() < 8) {
            return 4f;
       // }
                       
    }

     public float GetCurrentSpeed() {

       return increasingSpeed;
                       
    }

    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "rock") {
            GameObject[] asteroids = GameObject.FindGameObjectsWithTag("rock");
            foreach (GameObject asteroid in asteroids)
            {
                asteroid.GetComponent<AsteroidController>().ExplodeAndDespawn();
                AudioManager.Instance.Play("asteroid_touch");
            }
            UpdateGMState(GM_State.COUNTING);
        }
        
    }

}
