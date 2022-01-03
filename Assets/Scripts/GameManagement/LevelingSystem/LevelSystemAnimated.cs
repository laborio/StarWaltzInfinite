using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this class takes our current lvl and target lvl and animates our values towards them
public class LevelSystemAnimated : MonoBehaviour
{
    public static LevelSystemAnimated Instance;

    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private LevelingSystem levelingSystem;
    PlayerController_GM0 playerController_GM0;
    private bool isAnimating;
    private int level;
    private int experience;
   // private int experienceToNextlevel;
    public float updateTimer;
    public float updateTimerMax;
    
    
    void Awake() {

        if(Instance != null) {
            return;
        } 
        Instance = this;

        level = LevelingSystem.Instance.GetLevelNumber();
        experience =  LevelingSystem.Instance.GetExperience();

        LevelingSystem.Instance.OnExperienceChanged += LevelingSystem_OnExperienceChanged;
        LevelingSystem.Instance.OnLevelChanged += LevelingSystem_OnLevelChanged;
  
    }

    void OnDestroy() {
        LevelingSystem.Instance.OnExperienceChanged -= LevelingSystem_OnExperienceChanged;
        LevelingSystem.Instance.OnLevelChanged -= LevelingSystem_OnLevelChanged;
    }

    void Start() {
       
        updateTimerMax = 0.05f;
       
        
    }

   
    void Update() {

        if(isAnimating) {  // our code should be updating 60 times per second 
            //Check if its time to update
            updateTimer += Time.deltaTime;
            while (updateTimer > updateTimerMax) {
                //time to update
                updateTimer -= updateTimerMax;
                UpdateAddExperience();
            }
        }
    }

    private void UpdateAddExperience() {

            if(level < LevelingSystem.Instance.GetLevelNumber()) {
                //local level under target level
                AddExperience();
            }
            else {
                //local level equals the target level
                if(experience < LevelingSystem.Instance.GetExperience()) {
                    AddExperience();
                } 
                else {
                    isAnimating = false;
                }

            }

    }


    private void AddExperience() {
        experience++;
        if(experience >= LevelingSystem.Instance.GetExperienceToNextLevel(level)) {
            level ++;
            experience = 0;
             if(OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);  //call this event when lvl up
        }
        if(OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);  //call this event when exp up

    }

    public int GetLevelNumber() {
        return level;
    }

    public float GetExperienceNormalized() {  //give the xp value bewtween 0 a 1 to fill the slider 

        return (float) experience / LevelingSystem.Instance.GetExperienceToNextLevel(level);

    }


     private void LevelingSystem_OnLevelChanged(object sender, System.EventArgs e) {
           
           //updateTimerMax = updateTimerMax / 1.2f;
           if(PlayerController2.Instance.playState == PlayState.REGULAR) {
               playerController_GM0 = FindObjectOfType<PlayerController_GM0>();
               
               if(playerController_GM0.regularState == RegularState.WAITINGFORTUTORIAL ||
               playerController_GM0.regularState == RegularState.BOOST) {
                    
                    updateTimerMax = (1 / ((float) level + 1)) / 1500f;
                   
                }
                else {
                    updateTimerMax = (1 / ((float) level + 1)) / 20f;
                    
                }
           }
                
                
                    

        //    else if(PlayerController2.Instance.playState == PlayState.GAMEMODE1) 
        //     updateTimerMax = (3 / (float) ScoreManager.Instance.GetMiniGameXpCount());

            isAnimating = true;
            
            
        }

    private void LevelingSystem_OnExperienceChanged(object sender, System.EventArgs e) {
           
           if(PlayerController2.Instance.playState == PlayState.GAMEMODE1 ||
           PlayerController2.Instance.playState == PlayState.GAMEMODE2 ||
           PlayerController2.Instance.playState == PlayState.GAMEMODE3 ||
           PlayerController2.Instance.playState == PlayState.GAMEMODE4 ) 

            updateTimerMax = (3 / (float)ScoreManager.Instance.GetMiniGameXpCount());
            
         
            else{
                if(PlayerController2.Instance.player.GetComponent<PlayerController_GM0>().regularState == RegularState.BOOST) {
                    Debug.Log("bon timer");
                    updateTimerMax = (1 / ((float) level + 1)) / 1500f;
                }
                
                   
            }
                
           isAnimating = true;
        }

    public void ResetProgression() {
        level = 1;
        experience = 0;
        updateTimerMax = 0.05f;

        if(OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty); 
        if(OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
    }
    public bool GetisAnimating() {
        return isAnimating;
    }

}
