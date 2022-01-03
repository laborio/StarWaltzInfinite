using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    
    LevelingSystem levelingSystem;
    LevelGenerator levelGenerator;
    public event EventHandler OnStarCountChanged;
    public event EventHandler OnUnitCountChanged;
    
    private int starCount;
//mini game var
    private int miniGame_unitCount;
    private int miniGame_starCount;
    private int miniGame_xpCount;
//regular score var
    private int scoreLevel;
    private int scorePercentage;
    private int highScoreLevel;
    private int highScorePercentage;

    void Awake() {

         if(Instance != null) {
            return;
        }
        
        Instance = this;

        levelingSystem = FindObjectOfType<LevelingSystem>();
        levelGenerator = FindObjectOfType<LevelGenerator>();

        LoadScoreData();
    }

    void Start() {
         levelingSystem.OnExperienceChanged += levelingSystem_OnExperienceChanged;
        levelingSystem.OnLevelChanged += levelingSystem_OnLevelChanged;
        levelGenerator.OnTimerOver += LevelGenerator_OnTimerOver;
        AdsManager.Instance.OnRewardedAdFinished += AdsManager_OnRewardedAdFinished;

    }

    private void levelingSystem_OnLevelChanged(object sender, System.EventArgs e) {
           
            scoreLevel = levelingSystem.GetLevelNumber();
           

            if(scoreLevel > highScoreLevel){
                highScorePercentage = 0;
                highScoreLevel = scoreLevel;
                SaveHighScore();
                
            }
           
        }

    private void levelingSystem_OnExperienceChanged(object sender, System.EventArgs e) {
           
            scorePercentage =  Mathf.RoundToInt(levelingSystem.GetExperienceNormalized() * 100);

            if(scoreLevel >= highScoreLevel && scorePercentage >= highScorePercentage) {
                highScorePercentage = scorePercentage;
                SaveHighScore();
            }
            
        }

    private void AdsManager_OnRewardedAdFinished(object sender, System.EventArgs e){
        AddStar(250);
        AudioManager.Instance.Play("coin_touch");
        Analytics.CustomEvent("Got Gift Coins" + starCount);
    }
    void Update() {
        
    }

    public void AddStar(int amount) {
        starCount += amount;
        if(OnStarCountChanged != null) OnStarCountChanged(this, EventArgs.Empty);

        SaveStarCount();
       // Debug.Log("ADDED " + amount);
    }
    public void SpendStar(int cost) {
        starCount -= cost;
        if(OnStarCountChanged != null) OnStarCountChanged(this, EventArgs.Empty);

        SaveStarCount();
       // Debug.Log("SPENT " + cost);
    }

    public bool TrySpendStarAmount(int spendStarAmount) {
        if(starCount >= spendStarAmount) {
            SpendStar(spendStarAmount);
             
             return true;
        }
        else {
            return false;
        }
    }

    public int GetStarCount() {
        return starCount;
    }

    public int GetHighScoreLevel() {
        return highScoreLevel;
    }

    
    public int GetHighScorePercentage() {
        return highScorePercentage;
    }

    public void SaveHighScore() {
        PlayerPrefs.SetInt("HighScoreLevel", highScoreLevel);
        PlayerPrefs.SetInt("HighScorePercentage", highScorePercentage);
        PlayerPrefs.Save();

        Analytics.CustomEvent("New HighScore" + highScoreLevel);
    }

    private void SaveStarCount() {
        PlayerPrefs.SetInt("StarCount", starCount);
        PlayerPrefs.Save();
    }

    private void LoadScoreData() {
        if(PlayerPrefs.HasKey("HighScoreLevel")) {
            highScoreLevel = PlayerPrefs.GetInt("HighScoreLevel", 0);
        }
        if(PlayerPrefs.HasKey("HighScorePercentage")) {
             highScorePercentage = PlayerPrefs.GetInt("HighScorePercentage", 0);
        }
         if(PlayerPrefs.HasKey("StarCount")) {
             starCount = PlayerPrefs.GetInt("StarCount", 0);
        }
    }

    public int GetMiniGameUnitCount() {
        return miniGame_unitCount;
    }
    public void AddUnitCount (int amount) {
        miniGame_unitCount ++;
        if(OnUnitCountChanged != null) OnUnitCountChanged(this, EventArgs.Empty);
    }

    public void ResetMiniGameValuesCount() {
        miniGame_unitCount = 0;
        miniGame_starCount = 0;
        miniGame_xpCount = 0;
    }

    public int GetMiniGameStarCount() {
        return miniGame_starCount;
    }
    public void AddMiniGameStarCount (int amount) {
        miniGame_starCount += amount;
    }
      public int GetMiniGameXpCount() {
        return miniGame_xpCount;
    }
    public void AddMiniGameXpCount (int amount) {
        miniGame_xpCount += amount;
      //  Debug.Log(miniGame_xpCount);
    }
    public void SubstractMiniGameXpCount() {
        if(miniGame_xpCount > 0)
        miniGame_xpCount -= 1;
    }

     private void LevelGenerator_OnTimerOver(float remainingTime) {
    
           int additionalXpValue = Mathf.RoundToInt(((float)LevelingSystem.Instance.GetLevelNumber() * remainingTime * 4f));
           AddMiniGameXpCount(additionalXpValue);
           
           int additionalStarValue = Mathf.RoundToInt(((float)LevelingSystem.Instance.GetLevelNumber() * remainingTime) / 3f);
           AddMiniGameStarCount(additionalStarValue);

        
         levelGenerator.OnTimerOver -= LevelGenerator_OnTimerOver;
    
    }


}
