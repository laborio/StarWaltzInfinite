using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{   
    public static SceneManagement Instance;
    GameManager gameManager;
    PlayerController2 playerController2;
    //public Animator transition;
    public float transitionTime = 1f;
    void Awake() {
         if(Instance != null) {
            return;
        }
        
        Instance = this;

    }

    void Update() {
         
    }


    public void BacktoMenu () { 
           
        //StartCoroutine(LoadLevel(0));
        //SceneManager.LoadScene(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    
   }

   public void PlayGame() {

        SceneManager.LoadScene(1);
        //StartCoroutine(LoadLevel(1));
   
        
       
   }

//     IEnumerator LoadLevel(int levelIndex) {

//     transition.SetTrigger("Start");

//     yield return new WaitForSeconds(transitionTime);

//     SceneManager.LoadScene(levelIndex);
//    }

}
