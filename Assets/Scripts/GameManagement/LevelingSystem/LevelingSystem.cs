using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;



//events are listened by the levelUI class, on the LevelUI object
public class LevelingSystem : MonoBehaviour
{
   
    public static LevelingSystem Instance;
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;
    public event EventHandler OnMiniGameLevelChanged;

    private int level;
    private int experience;
    public int jumpCount; // is increased when bouncing right in PlayerController_GM0
    //public int[] miniGameLevels = {2, 6, 10, 15};
    public int miniGameLevel;
    public int baseMiniGameLevel;
    private int xpAmount;


    [Header("XP values")]
    [HideInInspector] public int baseXP; //gets its value in playercollisions
    public int planetXP = 10;
    public int satelliteXP = 20;

   
    void Awake() {

         if(Instance != null) {
            return;
        } 
        Instance = this;


        level = 1;
        experience = 0;
        jumpCount = 1;

        baseMiniGameLevel = 3;
        miniGameLevel = baseMiniGameLevel;
        
    }

    void Start() { 
        
    }

    void Update() {
        //if(level == 2) 
    }
   

    public void AddExperience(int amount) {
        experience += amount;
        
        while (experience >= GetExperienceToNextLevel(level)) {
            //enough xp to lvl up
            
            experience -= GetExperienceToNextLevel(level);
            level++;
            if(OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);  //call this event when lvl up
            
            //send event on levels that should trigger minigames
            // foreach (int triggerLevels in miniGameLevels) {
            //     if(level == triggerLevels)
            //     if(OnMiniGameLevelChanged != null) OnMiniGameLevelChanged(this, EventArgs.Empty);
            // }
            if(level == miniGameLevel) {
                if(OnMiniGameLevelChanged != null) OnMiniGameLevelChanged(this, EventArgs.Empty);
                miniGameLevel = 0; //new minigame level is calculated in mini game recap controller
            }
            
        }
        
        if(OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);  //call this event when exp up
    }

    public int GetLevelNumber() {
        return level;
    }

    public float GetExperienceNormalized() {  //give the xp value bewtween 0 a 1 to fill the slider 

        return (float) experience / GetExperienceToNextLevel(level);

    }

    public int GetExperience() {  //give the true xp value
        return experience;
    }

    public int GetExperienceToNextLevel(int level) { 
      // return experienceToNextlevel;
       return (level) * 50;
    }

    public int XpGain(int baseXp) {

        
        return baseXp * jumpCount;

    }
    public int GetPlanetXp() {
        if(level < 15)
        return 10;

        else if(level >= 15 && level < 25)
        return 15;

        else if(level >= 25 && level < 30)
        return 20;

        else if(level >= 30 && level < 35)
        return 25;

        else
        return 30;
    }

    public int GetBonusXp() {
        if(level < 15)
        return 20;

        else if(level >= 15 && level < 25)
        return 30;

        else if(level >= 25 && level < 30)
        return 40;

        else if(level >= 30 && level < 35)
        return 50;

        else
        return 60;
    }

    public int GetXpAmount() {
        return xpAmount;
    }

    public void ResetProgression() {
        level = 1;
        experience = 0;

        // if(OnLevelChanged != null) OnLevelChanged(this, EventArgs.Empty);
        // if(OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
    }
    
    public int GetNextMiniGameLevel() {
        if(level < 20)
        return 5;

        // else if(level >= 10 && level < 20)
        // return 4;

        else
        return 4;
    }

    public void ResetMiniGameLevel() {
     
        miniGameLevel = baseMiniGameLevel;
    }

    public void ToggleMiniGames(bool value) {
        if(value) {

             baseMiniGameLevel = 3;
             Analytics.CustomEvent("Enable Mini-Games");

            if(GameManager.Instance.State == GameState.MAINMENU) {
                miniGameLevel = baseMiniGameLevel;
            }
            else {
               // baseMiniGameLevel = LevelingSystem.Instance.GetLevelNumber() + 5;
                miniGameLevel = GetLevelNumber() + 5;
            }
            
        }
        

        else {
            baseMiniGameLevel = 0;
            miniGameLevel = baseMiniGameLevel;
            Analytics.CustomEvent("Disable Mini-Games");
        }
        
    }

    public void XPButton100() {
        AddExperience(100);
    }
    public void XPButton500() {
        AddExperience(500);
    }
    public void XPButton1000() {
        AddExperience(1000);
    }
    public void lvlUP() {
        level++;
    }
 
}
