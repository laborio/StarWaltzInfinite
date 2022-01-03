using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    private GameObject gameOverPanel;
    private GameObject mainMenuPanel;
    private GameObject storePanel;
    private GameObject levelPanel;
    private GameObject gm1Panel;
    private GameObject playerReadyPanel;
    [HideInInspector] public GameObject boostPanel; //opened in PlayerController GM_0 tuto movement
    private GameObject pausePanel;
    private GameObject  pauseButton;
    public GameObject menuButtonFromPause;
    public AudioMixer audioMixer;
    public Dialogue dialogue_GM0;
    public Dialogue dialogue_GM1;
    public Dialogue dialogue_GM2;
    public Dialogue dialogue_GM3;
    public Dialogue dialogue_GM4;
    DialogueManager dialogueManager;
    GameManager gameManager;
    PlayerController2 playerController2;
    PlayerController_GM playerController_GM;
   
    void Awake() {
        playerController_GM = FindObjectOfType<PlayerController_GM>();
        
        dialogueManager = GetComponent<DialogueManager>();
        gameOverPanel = transform.Find("GameOverUI 2").gameObject;
        mainMenuPanel = transform.Find("MainMenuUI").gameObject;
        storePanel = transform.Find("UI_Shop").gameObject;
        levelPanel = transform.Find("LevelUI").gameObject;
        gm1Panel = transform.Find("GM1_UI").gameObject;
        playerReadyPanel = transform.Find("PlayerReady").gameObject;
        boostPanel = transform.Find("BoostPanel").gameObject;
        pausePanel = transform.Find("PauseMenu").gameObject;
        pauseButton = transform.Find("PauseButton").gameObject;

        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
        PlayerController2.OnPlayStateChanged += PlayerController2_OnPlayStateChanged;
       
        
    }

    // void OnDestroy() {

    //     GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    //     PlayerController2.OnPlayStateChanged -= PlayerController2_OnPlayStateChanged;
    //     playerController_GM.OnGM_StateChanged -= PlayerControllerGM_OnGMStateChanged;
        
    // }


    public void PlayerController2_OnPlayStateChanged(PlayState state)  {

        
        UpdatePlayerRefsAndSubToEvents(state); 
        menuButtonFromPause.SetActive(state == PlayState.REGULAR); //disable back button from pause menu during minigame to avoid bugs 

        //show tutorial guys and hide level UI
         switch (state) {
            case PlayState.MENU:
            levelPanel.SetActive(false);
            break;

            case PlayState.REGULAR:
            levelPanel.SetActive(true);
            if(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep) == 0) 
            HandleDialogueTriggers(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep));
            break;

            case PlayState.GAMEMODE1:
           // if(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep) == 1) 
            HandleDialogueTriggers(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep));
            break;

            case PlayState.GAMEMODE2:
          //  if(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep) == 2) 
            HandleDialogueTriggers(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep));
            break;

            case PlayState.GAMEMODE3:
          //  if(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep) == 3) 
            HandleDialogueTriggers(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep));
            break;

            case PlayState.GAMEMODE4:
         //   if(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep) == 4) 
            HandleDialogueTriggers(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep));
            break;

       }    
    }


    private void GameManagerOnGameStateChanged(GameState state) {
        
       gameOverPanel.SetActive(state == GameState.LOST);
       mainMenuPanel.SetActive(state == GameState.MAINMENU);
       pauseButton.SetActive(state == GameState.PLAYMODE);
       pausePanel.SetActive(state == GameState.PAUSE);
           
       
    }

    private void PlayerControllerGM_OnGMStateChanged(GM_State state) {
        levelPanel.SetActive(state == GM_State.COUNTING);
         
    }

   public void HandleDialogueTriggers(int tutorialStep) {

        switch (tutorialStep) {   
            case 0: 
            dialogueManager.StartDialogue(dialogue_GM0);
            break;
            case 1:
            dialogueManager.StartDialogue(dialogue_GM1);
            break;
            case 2:
            dialogueManager.StartDialogue(dialogue_GM2);
            break;
            case 3:
            dialogueManager.StartDialogue(dialogue_GM3);
            break;
            case 4:
            dialogueManager.StartDialogue(dialogue_GM4);
            break;

           
       }
   }

   public void OpenStoreButton() {
       mainMenuPanel.SetActive(false);
       storePanel.SetActive(true);
       levelPanel.SetActive(false);
   }

   public void BackToMenuButton() {

        // if(PlayerPrefs.HasKey(PlayerPrefKeys.kCurrentSkin)) 
           PlayerPrefKeys.kCurrentSkinInt = PlayerPrefs.GetInt(PlayerPrefKeys.kCurrentSkin, 0);    
        
        PlayerController2.Instance.AddSelectedSkinToList();
        PlayerController2.Instance.SetPlayerPrefab_GM(2);

        storePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        
        
    }

    private void PlayerController_OnPositionReady(object sender, System.EventArgs e) {
        //shows 'im ready button panel'
        playerReadyPanel.SetActive(true);
        playerController_GM.OnPositionReady -= PlayerController_OnPositionReady;        

    }
    
    public void ImReadyButton() {
        dialogueManager.EndDialogue();
        playerReadyPanel.SetActive(false);
        levelPanel.SetActive(false);
        gm1Panel.SetActive(true);
        playerController_GM.playerReady = true;
      
    }
    
    public void WhatShouldIDoButton() { //reassign tutorial key so show the tutorial of the current gameMode again
        PlayerPrefKeys.kTutorialInt = PlayerController2.Instance.gameModeIndex;
        PlayerPrefs.SetInt(PlayerPrefKeys.kTutorialStep, PlayerPrefKeys.kTutorialInt);
        PlayerPrefs.Save();
        HandleDialogueTriggers(PlayerPrefKeys.kTutorialInt);
    }

    public void PauseMenuButton() {
        GameManager.Instance.UpdateGameState(GameState.PAUSE);
    }
    public void ResumeButton() {
        GameManager.Instance.UpdateGameState(GameState.PLAYMODE);
    }

    public void BackFromOptionsButton() {
        if(GameManager.Instance.State == GameState.MAINMENU)
        mainMenuPanel.SetActive(true);

        else
        pausePanel.SetActive(true);

    }

    private void UpdatePlayerRefsAndSubToEvents(PlayState state) {
        
         if(state == PlayState.GAMEMODE1 ||
            state == PlayState.GAMEMODE2 ||
            state == PlayState.GAMEMODE3 ||
            state == PlayState.GAMEMODE4) {
                    playerController_GM = FindObjectOfType<PlayerController_GM>();
                    playerController_GM.OnGM_StateChanged += PlayerControllerGM_OnGMStateChanged;
                    playerController_GM.OnPositionReady += PlayerController_OnPositionReady;
                     
                    
            }

      
    }

    public void boostPanelYesButton() {
        PlayerPrefKeys.kBoostCountInt--;
        PlayerPrefs.SetInt(PlayerPrefKeys.kBoostCount, PlayerPrefKeys.kBoostCountInt);
        PlayerPrefs.Save();

        if(LevelingSystem.Instance.baseMiniGameLevel == 3)
        LevelingSystem.Instance.miniGameLevel = 20;

        PlayerController_GM0 pc0 = PlayerController2.Instance.player.GetComponent<PlayerController_GM0>();
        pc0.regularState = RegularState.BOOST;
        
      
        LevelingSystem.Instance.AddExperience(5250);
        boostPanel.SetActive(false);
    }

    public void boostPanelNoButton() {
        PlayerController_GM0 pc0 = PlayerController2.Instance.player.GetComponent<PlayerController_GM0>();
        pc0.regularState = RegularState.STARTING;

        boostPanel.SetActive(false);
    }


    public void SetVolume(bool value) {
        // audioMixer.SetFloat("volume", volume);
        // if(volume <= -30f)
        // volume = -80f;

        if(value)
        audioMixer.SetFloat("volume", 5f);

        else
        audioMixer.SetFloat("volume", -80f);
    }

    
}
