using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameRecapController : MonoBehaviour
{
    PlayerController_GM playerController_GM;
    private Text unitText;
    private Text starText;
    private Text xpText;
    private Text nextGameText;
    private Image unitIcon;
    private bool counting = false;
    [SerializeField] private Button goButton;

    void Awake() {

         unitText = transform.Find("unitBloc").Find("unitText").GetComponent<Text>();
         starText = transform.Find("starBloc").Find("starText").GetComponent<Text>();
         xpText = transform.Find("XPbloc").Find("xpText").GetComponent<Text>();
         unitIcon = transform.Find("unitBloc").Find("Icon").GetComponent<Image>();
         nextGameText = transform.Find("nextMinigame").Find("Text").GetComponent<Text>();
    }

    void OnEnable() {
        playerController_GM = FindObjectOfType<PlayerController_GM>();
        RectTransform rectTransform = unitIcon.GetComponent<RectTransform>();
        nextGameText.color = new Color32(255, 255, 255, 0);
        counting = false;
        SetValues(ScoreManager.Instance.GetMiniGameUnitCount(), ScoreManager.Instance.GetMiniGameStarCount(), ScoreManager.Instance.GetMiniGameXpCount());

        //update unit icon sprite
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
    }
     void Start() {
        
    }

    void Update() {
        if(counting == true) {
             
            if(LevelSystemAnimated.Instance.GetisAnimating() == false) {
                LevelingSystem.Instance.miniGameLevel = LevelingSystem.Instance.GetLevelNumber() + LevelingSystem.Instance.GetNextMiniGameLevel();

                nextGameText.text = "NEXT BONUS STAGE: Lv. " + LevelingSystem.Instance.miniGameLevel;
                Color tmp = nextGameText.color;
                tmp.a = .1f;
                nextGameText.color += tmp;
                goButton.gameObject.SetActive(true);
            }

        }
       
        
    }

    public void StartCounting() {
        LevelingSystem.Instance.AddExperience(ScoreManager.Instance.GetMiniGameXpCount());
        ScoreManager.Instance.AddStar(ScoreManager.Instance.GetMiniGameStarCount());
        counting = true;
    }

    private void SetValues(int unitAmount, int starAmount, int xpAmount) {
      unitText.text = unitAmount.ToString();
      starText.text = "+" + starAmount.ToString();
      xpText.text = "+" + xpAmount.ToString();
    }

    public void GoButton() {
        ScoreManager.Instance.ResetMiniGameValuesCount();
        goButton.gameObject.SetActive(false);
        playerController_GM.UpdateGMState(GM_State.EXIT);

    }

}
