using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameOverUI : MonoBehaviour
{
	ScoreManager scoreManager;
	[SerializeField] Button continueButton;
	[SerializeField] private Image starIcon;
	[SerializeField] private Sprite starSprite;
	[SerializeField] private TMP_Text continueText;
	[SerializeField] private TMP_Text aroundIconText;
	Animator anim;
	TMP_Text highScoreText;
	
	//private Button button;
	//GameObject respawnbutton;


	void Awake()
	{
		anim = GetComponent<Animator>();
		 
		highScoreText = transform.Find("HighScoreGuy").Find("HighScoreText").GetComponent<TMP_Text>();

	}

	void OnEnable()
	{
		highScoreText.text = $"BEST\nLv.{ScoreManager.Instance.GetHighScoreLevel()} ({ScoreManager.Instance.GetHighScorePercentage()}%)"; 
		anim.SetBool("pop", true);
		anim.SetBool("depop", false);
		
		//aroundIconText.text = $"-{PlayerController2.Instance.continueCost[PlayerController2.Instance.continueCostIndex]}";
		aroundIconText.text = $"({5 - PlayerController2.Instance.continueCostIndex})";
	}

	public void DarkenButtonText(bool value) {

		if(value == true) {
			starIcon.GetComponent<Image>().color  = new Color32(100, 100, 100, 255);
			continueText.GetComponent<TMP_Text>().color = new Color32(100, 100, 100, 255);
			aroundIconText.color = new Color32(100, 100, 100, 255);

			continueButton.interactable = false;
		}
		
		else {
			starIcon.GetComponent<Image>().color  = new Color32(255, 255, 255, 255);
			continueText.GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);
			aroundIconText.color = new Color32(210, 110, 110, 255);

			continueButton.interactable = true;
		}

		
		
	} 


}
