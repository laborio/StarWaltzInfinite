using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public enum GM_State {WAITINGFORTUTORIAL, STARTING, COUNTING, EXIT}

public class PlayerController_GM : MonoBehaviour
{
    public GM_State gM_State;
    public event EventHandler OnPositionReady;
    public event Action<GM_State> OnGM_StateChanged;
    LevelGenerator levelGenerator;
    GameObject worldSpaceCanvas;
    private protected float timerExit;
    public bool playerReady = false;
    private protected bool playerInPosition = false;
   
     void Awake() {
       levelGenerator = FindObjectOfType<LevelGenerator>();
       worldSpaceCanvas = GameObject.FindWithTag("WorldCanvas");
    }

    protected virtual void OnEnable() {
        
        levelGenerator.OnMinigameOver += LevelGenerator_OnMiniGameOver;
        LevelSystemAnimated.Instance.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;

        timerExit = 0;
        playerInPosition = false;
        playerReady = false;

        gM_State = GM_State.WAITINGFORTUTORIAL;
    }

    protected virtual void OnDisable() {
        levelGenerator.OnMinigameOver -= LevelGenerator_OnMiniGameOver;
        LevelSystemAnimated.Instance.OnLevelChanged -= LevelSystemAnimated_OnLevelChanged;
    
    }

    protected virtual void FixedUpdate() {
        
         if(GameManager.Instance.State == GameState.PLAYMODE) {

            switch (gM_State) {
                case GM_State.WAITINGFORTUTORIAL:
                HandleTutorialMovement();
                break;
                case GM_State.STARTING:
                HandleStartingMovement();               
                break;
                case GM_State.COUNTING:
                HandleCounting();
                break;
                case GM_State.EXIT:
                HandleExitMovement();
                break;

            }
         }  
    }

     public virtual void UpdateGMState(GM_State newState) {  
        gM_State = newState;

        switch (newState) {
            case GM_State.WAITINGFORTUTORIAL:
            break;
            case GM_State.STARTING:              
            break;
            case GM_State.COUNTING:
            break;
            case GM_State.EXIT:
            break;
        
        } 
        OnGM_StateChanged?.Invoke(newState);
     }

    protected virtual void HandleTutorialMovement() {
  
        if(playerInPosition == true)
        OnPositionReady?.Invoke(this, EventArgs.Empty);

        if(playerReady == true)
        UpdateGMState(GM_State.STARTING);
    }

   protected virtual void HandleStartingMovement() {

    }

    protected virtual void HandleCounting() {
        transform.position = Vector3.Lerp(transform.position, new Vector3(0f, Camera.main.transform.position.y -1.5f, 0f), 1f * Time.deltaTime);

     }

    protected virtual void HandleExitMovement() {

        // timerExit += Time.deltaTime; //is reset to 0 in starting movement, when reentering the gm state from another gm

        // if(timerExit <= .5f) {
        //     //move up and get to center of screen
        //     transform.position += Vector3.up * 10f * Time.deltaTime;
           
        // }
        // else {
            
             PlayerController2.Instance.UpdatePlayState(PlayState.REGULAR);
        // }
       
     }

    protected virtual void LevelGenerator_OnMiniGameOver(object sender, EventArgs e) {
        if(ScoreManager.Instance.GetMiniGameXpCount() <= 0)
            ScoreManager.Instance.AddMiniGameXpCount(10);

        UpdateGMState(GM_State.COUNTING);
    }
    protected virtual void LevelSystemAnimated_OnLevelChanged(object sender, EventArgs e) {
        AudioManager.Instance.Play("levelup");
        GameObject lvlUpFlashEffectRef = LeanPool.Spawn(PlayerController2.Instance.playerPrefabList[3], transform.position, Quaternion.identity);
        ScreenShakeController.instance.StartShake(.15f, .15f);
        LeanPool.Despawn(lvlUpFlashEffectRef, 2); 

        GameObject xpAnimObj = LeanPool.Spawn(StoreAssets.i.pf_levelUpText, new Vector3(transform.position.x, transform.position.y -2f, 0f), Quaternion.identity);
        xpAnimObj.transform.SetParent(worldSpaceCanvas.transform);
        
        LeanPool.Despawn(xpAnimObj, 2);                    

    }

}
