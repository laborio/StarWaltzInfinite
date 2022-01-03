using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.Rendering.Universal;


public class PlayerController_GM02 : PlayerController_GM
{
    GameObject globalLightObj;
    Light2D globalLight2D;
    private bool mouseCheck = false;
    private Vector3 mouseTargetPosition;

   void Start() {
        
   }

   protected override void OnEnable() {
        globalLightObj = GameObject.Find("Global Light 2D");
    	globalLight2D = globalLightObj.GetComponent<Light2D>();

        base.OnEnable();

       // playerReady = true; //turned to true here instead of handletutorialmovement cuz we dont need to wait for the player to be in position
    }
    protected override void OnDisable() {
        base.OnDisable();
       
    }

    protected override void HandleTutorialMovement() {
         if(globalLight2D.intensity > 0f)
        globalLight2D.intensity -= 0.01f;

        playerInPosition = true;

        base.HandleTutorialMovement();
    }

    protected override void HandleStartingMovement()
    {   
         if (InputManager.getTouch(TouchPhase.Began)) {
                                    	 
            mouseTargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseTargetPosition.z = 0;
            mouseCheck = true;
        }

        if(mouseCheck == true) {
        	transform.position = Vector3.Lerp(transform.position, mouseTargetPosition, 6f * Time.deltaTime);

        	if(transform.position.x == mouseTargetPosition.x && transform.position.y == mouseTargetPosition.y)
			mouseCheck = false;
        }                      
								 
    }

    protected override void HandleCounting() {
        base.HandleCounting();
       
       mouseCheck = false;
        if(globalLight2D.intensity < 0.4f)
        globalLight2D.intensity += 0.02f;
    }
}
