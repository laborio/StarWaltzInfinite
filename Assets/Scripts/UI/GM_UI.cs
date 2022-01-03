using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM_UI : MonoBehaviour
{
    private Text timerText;
    private Image timerBarImage;
    private Image unitIcon;
    private Text unitText;
    private GameObject recapPanel;
    public Animator anim;
    ScoreManager scoreManager;
    LevelGenerator levelGenerator;
    PlayerController_GM playerController_GM;
    PlayerController_GM03 playerController_GM03;

    void Awake() {
        levelGenerator = FindObjectOfType<LevelGenerator>();
            
        
        timerText = transform.Find("timerText").GetComponent<Text>();
        timerBarImage = transform.Find("TimerBar").Find("fill").GetComponent<Image>();
        unitIcon = transform.Find("unit").Find("Icon").GetComponent<Image>();
        unitText = transform.Find("unit").Find("unitText").GetComponent<Text>();

        recapPanel = transform.Find("recap").gameObject;


    }

    void OnEnable() {
        playerController_GM = FindObjectOfType<PlayerController_GM>();
        playerController_GM03 = FindObjectOfType<PlayerController_GM03>();

         //Subscribe to events
        levelGenerator.OnTimerChanged += LevelGenerator_OnTimerChanged;
        ScoreManager.Instance.OnUnitCountChanged += ScoreManager_OnUnitCountChanged;
        playerController_GM.OnGM_StateChanged += PlayerController_OnGMStateChanged;

        if(PlayerController2.Instance.playState == PlayState.GAMEMODE3)
        playerController_GM03.OnThrowCountChanged += GM03_OnThrowCountChange;

         //update unit icon sprite
        RectTransform rectTransform = unitIcon.GetComponent<RectTransform>();
        switch (PlayerController2.Instance.playState) {
            case PlayState.GAMEMODE1:
            unitIcon.sprite = StoreAssets.i.sp_rockIcon;
            unitIcon.color = new Color32 (255, 255, 255, 255);
            rectTransform.sizeDelta = new Vector2(320, 320);
            break;
            case PlayState.GAMEMODE2:
            unitIcon.sprite = StoreAssets.i.sp_planetIcon;
            unitIcon.color = new Color32 (240, 240, 100, 255);
            rectTransform.sizeDelta = new Vector2(125, 125);
            break;
            case PlayState.GAMEMODE3:
            unitIcon.sprite = StoreAssets.i.sp_lightSaberIcon;
            unitIcon.color = new Color32 (255, 255, 255, 255);
            rectTransform.sizeDelta = new Vector2(20, 120);
            rectTransform.eulerAngles = new Vector3(0,0,-30);
            break;
            case PlayState.GAMEMODE4:
            unitIcon.sprite = StoreAssets.i.sp_rockIcon;
            unitIcon.color = new Color32 (255, 255, 255, 255);
            rectTransform.sizeDelta = new Vector2(320, 320);
            break;
        }

        anim.SetBool("barON", true);

        InitUI(); //set up the top bar UI according to the game
    }
     void OnDisable() {

        anim.SetBool("barON", false);
        anim.SetBool("barOFF", true);

        levelGenerator.OnTimerChanged -= LevelGenerator_OnTimerChanged;
        ScoreManager.Instance.OnUnitCountChanged -= ScoreManager_OnUnitCountChanged;
        playerController_GM.OnGM_StateChanged -= PlayerController_OnGMStateChanged;

        if(PlayerController2.Instance.playState == PlayState.GAMEMODE3)
         playerController_GM03.OnThrowCountChanged -= GM03_OnThrowCountChange;
    }


    void Start() {

     
    }

    private void SetTimerBar(float remainingTime) { 
        timerBarImage.fillAmount = remainingTime;
    }

    private void SetUnitCount(int unitCount) {
        if(PlayerController2.Instance.playState == PlayState.GAMEMODE3)
        unitText.text = $"{ScoreManager.Instance.GetMiniGameUnitCount()}/{unitCount} ({playerController_GM03.GetNumberOfThrow() - unitCount} left)";
        
        else
        unitText.text = "x " + unitCount;
    }
    private void SetTimerText(float remainingTime) {
        if(remainingTime < 0f) remainingTime = 0;
        
        timerText.text = "Time: " + Mathf.Round(remainingTime); //truncate
        
    }

    private void LevelGenerator_OnTimerChanged (float timer) {
        SetTimerBar(timer / levelGenerator.eventDuration); //get timer normalized
        SetTimerText(timer);
    }

     private void ScoreManager_OnUnitCountChanged (object sender, System.EventArgs e) {
         
         if(PlayerController2.Instance.playState == PlayState.GAMEMODE3) {
             
            SetUnitCount(playerController_GM03.GetThrowCounter());
            SetTimerBar((float)playerController_GM03.GetThrowCounter() /(float) playerController_GM03.GetNumberOfThrow());
         }
        else
         SetUnitCount(ScoreManager.Instance.GetMiniGameUnitCount());
     }

     private void GM03_OnThrowCountChange(int throwCount) {
          SetUnitCount(throwCount);
        //  SetTimerBar((float)playerController_GM03.GetThrowCounter() /(float) playerController_GM03.GetNumberOfThrow());
     }
  
      private void PlayerController_OnGMStateChanged(GM_State state)  {

         switch (state) {
            case GM_State.COUNTING:
            anim.SetBool("barON", false);
            anim.SetBool("barOFF", true);
            recapPanel.SetActive(true);
            recapPanel.GetComponent<Animator>().SetTrigger("pop");
            break;

            case GM_State.EXIT:
            recapPanel.SetActive(false);
            gameObject.SetActive(false);
            break;

       }    
    }

    private void InitUI() {
        switch (PlayerController2.Instance.playState)
        {
            case PlayState.GAMEMODE3:
            playerController_GM03 = FindObjectOfType<PlayerController_GM03>();
            SetTimerBar(ScoreManager.Instance.GetMiniGameUnitCount());
            //SetTimerText(levelGenerator.eventDuration);
            timerText.text = ""; //no text
           // SetUnitCount(ScoreManager.Instance.GetMiniGameUnitCount());
            SetUnitCount(playerController_GM03.GetThrowCounter());
            break;

            case PlayState.GAMEMODE4:
            playerController_GM03 = FindObjectOfType<PlayerController_GM03>();
            SetTimerBar(1);
            //SetTimerText(levelGenerator.eventDuration);
            timerText.text = ""; //no text
            SetUnitCount(ScoreManager.Instance.GetMiniGameUnitCount());
            break;

            default:
            SetTimerBar(levelGenerator.eventDuration);
            SetTimerText(levelGenerator.eventDuration);
            SetUnitCount(ScoreManager.Instance.GetMiniGameUnitCount());
            break;
        }
    }

}
