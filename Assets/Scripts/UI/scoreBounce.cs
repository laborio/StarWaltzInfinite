using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scoreBounce : MonoBehaviour
{
    //public event EventHandler OnDestroyReady;
	TMP_Text text;
    public string textInput;
    public bool ready = false;
	public Animator anim;
    private LevelingSystem levelingSystem;
    [SerializeField] private bool showXPValue;
    

  
    void Awake() {
        
        
        levelingSystem = FindObjectOfType<LevelingSystem>();
        text = GetComponent<TMP_Text>();

       // levelingSystem.OnLevelChanged += levelingSystem_OnLevelChanged;
        
    }

    void OnEnable()
    {
        
        anim.SetBool("idle", false);
        anim.SetBool("pop", true);  
    }

     private void levelingSystem_OnLevelChanged(object sender, System.EventArgs e) {
           
         //lvlup = true;
    }

  
    void OnDisable() {
    
    }

    
    void Update() {

            transform.position += Vector3.up * 3f * Time.deltaTime;

            if(showXPValue == true)
            text.text = textInput + levelingSystem.XpGain(levelingSystem.baseXP); //a garder dans l'update car le jumpCount s'incremente apres la collision
            else
            text.text = textInput;
    }
}
