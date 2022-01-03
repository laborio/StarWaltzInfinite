using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{

    private Text levelText;
    private Image experienceBarImage;
    private Text starText;
    public Animator anim;
    private LevelSystemAnimated levelSystemAnimated;
    ScoreManager scoreManager;

    void Awake() {
        levelSystemAnimated = FindObjectOfType<LevelSystemAnimated>();
        
        levelText = transform.Find("LevelText").GetComponent<Text>();
        experienceBarImage = transform.Find("XpBar").Find("fill").GetComponent<Image>();
        starText = transform.Find("Stars").Find("StarText").GetComponent<Text>();

    }


    void OnEnable() {
         //Subscribe to events
        LevelSystemAnimated.Instance.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
        LevelSystemAnimated.Instance.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
        ScoreManager.Instance.OnStarCountChanged += ScoreManager_OnStarCountChanged;

        SetLevelNumber(LevelSystemAnimated.Instance.GetLevelNumber());
        SetExperienceBarSize(LevelSystemAnimated.Instance.GetExperienceNormalized()); 
        SetStarCount(ScoreManager.Instance.GetStarCount());

        anim.SetBool("isON", true);
        anim.SetBool("isOFF", false);
    }

    void OnDisable() {

        anim.SetBool("isON", false);
        anim.SetBool("isOFF", true);

        LevelSystemAnimated.Instance.OnExperienceChanged -= LevelSystemAnimated_OnExperienceChanged;
        LevelSystemAnimated.Instance.OnLevelChanged -= LevelSystemAnimated_OnLevelChanged;
        ScoreManager.Instance.OnStarCountChanged -= ScoreManager_OnStarCountChanged;
    
    }

    void Start() {
       

    }

    private void SetExperienceBarSize(float experienceNormalized) { //get the current exp
        experienceBarImage.fillAmount = experienceNormalized;
    }

    private void SetLevelNumber(int levelNumber) {
      levelText.text = "Lv " + (levelNumber);
    }

    private void SetStarCount(int starCount) {
        starText.text = "x " + starCount;
    }

   
    private void LevelSystemAnimated_OnLevelChanged(object sender, System.EventArgs e) {
            //level changed, update text 
            SetLevelNumber(LevelSystemAnimated.Instance.GetLevelNumber());

            //text anim
           LeanTween.scale(levelText.GetComponent<RectTransform>(), levelText.GetComponent<RectTransform>().localScale*1.5f, 1f).setEase (LeanTweenType.punch);           
            
        }

    private void LevelSystemAnimated_OnExperienceChanged(object sender, System.EventArgs e) {
            //Exp changed, update bar size
            SetExperienceBarSize(LevelSystemAnimated.Instance.GetExperienceNormalized());
            
        }

     private void ScoreManager_OnStarCountChanged(object sender, System.EventArgs e) {
           
            SetStarCount(ScoreManager.Instance.GetStarCount());
            
    }


 
}
