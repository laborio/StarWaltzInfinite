using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class MouseOverUI 
{
    public static bool isBusy = false;

     public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        if(eventDataCurrentPosition.position.x > (Screen.width * 0.85f) && eventDataCurrentPosition.position.y < (Screen.height * 0.1f)) {
           // AudioManager.Instance.Play("coin_touch");
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        else return false;
        
    }

   
}
