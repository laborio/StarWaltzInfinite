using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public enum GameState {MAINMENU, PLAYMODE, PAUSE, LOST }

public static class PlayerPrefKeys {
    public static readonly string kTutorialStep = "TUTORIALSTEP";
    public static int kTutorialInt;

    public static readonly string kCurrentSkin = "SKIN";
    public static int kCurrentSkinInt;

    public static readonly string kBoostCount = "BOOST";
    public static int kBoostCountInt;

}

public class GameManager : MonoBehaviour
{   public static GameManager Instance;

    public static event Action<GameState> OnGameStateChanged; //on declare l'event qui notifiera les autres scripts qu'on a changé de state
    public GameState State;
    LevelGenerator levelGenerator;
    CameraController cameraController;
    GameOverUI gameOverUI;
    MenuManager menuManager;
    public bool mouseOverUI;
    public int sysHour = System.DateTime.Now.Hour;

    
    private void Awake() {

                    
        if(Instance != null) {
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
         //Application.targetFrameRate = 50;
        LoadTutorialKeyData();
        LoadBoostKeyData();
        LoadSkinKeyData();
                
    }


    private void Start() {

        levelGenerator = FindObjectOfType<LevelGenerator>();
        cameraController = FindObjectOfType<CameraController>();;  
        gameOverUI = FindObjectOfType<GameOverUI>();
        menuManager = FindObjectOfType<MenuManager>();


        UpdateGameState(GameState.MAINMENU);
        
    }


    public void UpdateGameState(GameState newState) {   //calling this function updates the current game state
        State = newState;

        switch (newState) {
          
            case GameState.PAUSE:
            Time.timeScale = 0f;
            mouseOverUI = false;
                break;
           
            default:
            Time.timeScale = 1f;
                break;
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);  
                                              
    }
    
    void Update() {
      // Debug.Log(mouseOverUI);
    }
    

    public void ResetTutorialKey() {

        PlayerPrefKeys.kTutorialInt = 0;
        PlayerPrefs.SetInt(PlayerPrefKeys.kTutorialStep, PlayerPrefKeys.kTutorialInt);
        PlayerPrefs.Save();

    }

    private void LoadTutorialKeyData() {
        if(PlayerPrefs.HasKey(PlayerPrefKeys.kTutorialStep)) {
            PlayerPrefKeys.kTutorialInt = PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep);
        }
        else {
            PlayerPrefs.SetInt(PlayerPrefKeys.kTutorialStep, 0);
            PlayerPrefs.Save();
        }

        // Debug.Break();
        // Debug.Log(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep));
        
    }

     private void LoadBoostKeyData() {
        if(PlayerPrefs.HasKey(PlayerPrefKeys.kBoostCount)) {
            PlayerPrefKeys.kBoostCountInt = PlayerPrefs.GetInt(PlayerPrefKeys.kBoostCount);
        }
        else {
            PlayerPrefs.SetInt(PlayerPrefKeys.kBoostCount, 0);
            PlayerPrefs.Save();
        }

        // Debug.Break();
        // Debug.Log(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep));
        
    }

      private void LoadSkinKeyData() {
        if(PlayerPrefs.HasKey(PlayerPrefKeys.kCurrentSkin)) {
            PlayerPrefKeys.kCurrentSkinInt = PlayerPrefs.GetInt(PlayerPrefKeys.kCurrentSkin);
        }
        else {
            PlayerPrefs.SetInt(PlayerPrefKeys.kCurrentSkin, 1);
            PlayerPrefs.Save();
        }

        // Debug.Break();
        // Debug.Log(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep));
        
    }

    public void PlayButton() {
        Analytics.CustomEvent("Play Button");
        
        LevelingSystem.Instance.ResetProgression();
        LevelingSystem.Instance.ResetMiniGameLevel();
        LevelSystemAnimated.Instance.ResetProgression();


        //clear planets and reset cam
        levelGenerator.DeactivatePlanets();
        cameraController.ResetCameraPos();
        
        //reposition player
       // PlayerController2.Instance.PlayerToReplayPos();
        //  if(PlayerPrefs.GetInt(PlayerPrefKeys.kBoostCount) != 0) 
        //     menuManager.boostPanel.SetActive(true);

        UpdateGameState(GameState.PLAYMODE);
        PlayerController2.Instance.UpdatePlayState(PlayState.REGULAR);
    }

     public void BackToMenuFromPlayButton() {
          gameOverUI = FindObjectOfType<GameOverUI>();

          LevelingSystem.Instance.ResetProgression();
          LevelingSystem.Instance.ResetMiniGameLevel();
          LevelSystemAnimated.Instance.ResetProgression();
          PlayerController2.Instance.ResetContinueCost();


          if(gameOverUI)
          gameOverUI.DarkenButtonText(false);

        //  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

          UpdateGameState(GameState.MAINMENU);
          PlayerController2.Instance.UpdatePlayState(PlayState.MENU);
    }

    public void GoToStoreFromPlayButton() {
         gameOverUI = FindObjectOfType<GameOverUI>();

          LevelingSystem.Instance.ResetProgression();
          LevelSystemAnimated.Instance.ResetProgression();
          PlayerController2.Instance.ResetContinueCost();
          gameOverUI.DarkenButtonText(false);

          UpdateGameState(GameState.MAINMENU);
          PlayerController2.Instance.UpdatePlayState(PlayState.MENU);

          menuManager.OpenStoreButton();
    }

     public void ReplayButton() {
         Analytics.CustomEvent("Replay Button");

         gameOverUI = FindObjectOfType<GameOverUI>();
         //reset xp and lvls
        LevelingSystem.Instance.ResetProgression();
        LevelingSystem.Instance.ResetMiniGameLevel();
        LevelSystemAnimated.Instance.ResetProgression();
        PlayerController2.Instance.ResetContinueCost();

        //clear planets and reset cam
        levelGenerator.DeactivatePlanets();
        cameraController.ResetCameraPos();

        //reposition player
        PlayerController2.Instance.PlayerToReplayPos();

        //reactivate continue button
        gameOverUI.DarkenButtonText(false);

          UpdateGameState(GameState.PLAYMODE);
          PlayerController2.Instance.UpdatePlayState(PlayState.REGULAR);
    }

    public void MouseIsOverUI() {
        mouseOverUI = true;
        Debug.Log(mouseOverUI);
    }
    // public void MouseNotOverUI() {
    //     mouseOverUI = false;
    // }

    // public void MouseOverUI(bool value) {
    //     if(value)
    //     mouseOverUI = true;

    //     else
    //     mouseOverUI = false;
    // }

  
}
