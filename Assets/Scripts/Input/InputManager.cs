using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager {

    // private bool isMobile = false;

    public static bool getTouch(TouchPhase phase) {
        // bool touching = false;

        // if (Input.getMouseButtonDown()) {
        //     touching = true;
        // }

        // if (mobile) {
        //     touching = true;
        // }

        // return touching;

        if(MouseOverUI.IsPointerOverUIObject() == false) {
            if (phase == TouchPhase.Began) 
            {
                return Input.GetKeyDown("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
            } 
            else if (phase ==  TouchPhase.Moved) 
            {
                return Input.GetKey("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved);
            } 
            else 
            {
                return Input.GetKeyUp("space") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
            }
        }

        else return false;
        
        
    }
}


