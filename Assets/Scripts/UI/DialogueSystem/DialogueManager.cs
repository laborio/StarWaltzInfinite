using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public TMP_Text dialogueText;
    public Animator animator;
    public Button nextLineButton;
    GameManager gameManager;

    void Awake() {
       
        sentences = new Queue<string>();
    }
    
    void Start()  {
    
    }

    public void StartDialogue(Dialogue dialogue) {
         
        animator.SetBool("isOpen", true);
        nextLineButton.interactable = true;
       
        sentences.Clear();

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
       
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        
    }

    public void EndDialogue() {
        animator.SetBool("isOpen", false);
        
        //update the tutorial tracking key
        // if(PlayerPrefs.HasKey(PlayerPrefKeys.kTutorialStep)) {

             if(PlayerController2.Instance.playState == PlayState.REGULAR) { //so regular mode tutorial goes away properly
                PlayerPrefKeys.kTutorialInt = 99;
                 PlayerPrefs.SetInt(PlayerPrefKeys.kTutorialStep, PlayerPrefKeys.kTutorialInt);
                PlayerPrefs.Save();

             }
       //  }
        //  else
        //  PlayerPrefKeys.kTutorialInt = 0;
       
         PlayerPrefs.SetInt(PlayerPrefKeys.kTutorialStep, PlayerPrefKeys.kTutorialInt);
         PlayerPrefs.Save();
        nextLineButton.interactable = false;

    }

}
