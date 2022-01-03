// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class DialogueTrigger : MonoBehaviour
// {
//    public Dialogue dialogue;
//    PlayerController2 playerController2;
//     GameManager gameManager;
//     TMP_Text text;
    
//     void Start() {

//         playerController2 = FindObjectOfType<PlayerController2>();
//         text = transform.Find("TipContent1").GetComponent<TMP_Text>();
        
//     }

//     void Update() {

//         if(GameManager.Instance.State == GameState.TUTORIAL) {

//             switch (playerController2.playState)
//             {
//                 case PlayState.REGULAR:
//                 TriggerDialogue();
//                 break;
//                 case PlayState.GAMEMODE1:
                
//                 break;
                
//             }
//         }
        
//     }

// //    public void TriggerDialogue() {
// //        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
// //    }
// }
