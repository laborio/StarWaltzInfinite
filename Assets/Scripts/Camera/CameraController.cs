using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float Speed = 4f;

    PlayerController2 playerController2;
    PlayerController_GM playerController_GM;
    PlayerController_GM0 playerController_GM0;
    GameObject player;
    GameManager gameManager;

    private Vector3 gm_cameraPos;
    
  

  
    void Awake() {
        
        PlayerController2.OnPlayStateChanged += PlayerController2_OnPlayStateChanged; 

        player = GameObject.FindWithTag("Player");
        playerController_GM = FindObjectOfType<PlayerController_GM>();
//        playerController_GM0 = player.GetComponent<PlayerController_GM0>();

    }

    void OnDestroy() {
     PlayerController2.OnPlayStateChanged -= PlayerController2_OnPlayStateChanged; 
    }

  

    private void PlayerController2_OnPlayStateChanged(PlayState state) {
        
      player = PlayerController2.Instance.player;
      playerController_GM = FindObjectOfType<PlayerController_GM>();

      if(state == PlayState.REGULAR)
      playerController_GM0 = player.GetComponent<PlayerController_GM0>();
        
    }
 
    void FixedUpdate()
    {
       // player = PlayerController2.Instance.player;
       

        switch (PlayerController2.Instance.playState) {

            case PlayState.MENU:
            ResetCameraPos();
            break;
            case PlayState.REGULAR:
            Handle_GM0_CameraMovement();
            break;
            case PlayState.GAMEMODE1:
            Handle_GM1_CameraMovement();
            break;
            case PlayState.GAMEMODE2:
            Handle_GM2_CameraMovement();
            break;
            case PlayState.GAMEMODE3:
            Handle_GM1_CameraMovement();
            break;
            case PlayState.GAMEMODE4:
            Handle_GM4_CameraMovement();
            break;

        }
     
    }

    //  void FixedUpdate()
    // {
    //    // player = PlayerController2.Instance.player;
       

    //     switch (PlayerController2.Instance.playState) {

            
    //         case PlayState.REGULAR:
    //         Handle_GM0_CameraMovement();
    //         break;
           
    //     }
     
    // }

  

    public void ResetCameraPos() {
        transform.position = new Vector3 (GameObject.FindWithTag("StartPlanet").transform.position.x,
                                          GameObject.FindWithTag("StartPlanet").transform.position.y,
                                          transform.position.z);
    }

    public void ContinueCameraPos() {
        transform.position = new Vector3(0f, player.transform.position.y, transform.position.z);
    }


    void Handle_GM0_CameraMovement() {
        if(playerController_GM0 != null) {

            if(playerController_GM0.regularState == RegularState.AUTOPILOT || 
                playerController_GM0.regularState == RegularState.STARTING ||
                playerController_GM0.regularState == RegularState.FLYING ||
                playerController_GM0.regularState == RegularState.EXIT ) { 

                gm_cameraPos = new Vector3(0f, player.transform.position.y + 4f, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, gm_cameraPos, Speed * Time.deltaTime);

            }

            else if(playerController_GM0.regularState == RegularState.BOOST) { 
                
               
                gm_cameraPos = new Vector3(0f, player.transform.position.y + 2f, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, gm_cameraPos, 13f * Time.deltaTime);
                

            }
             else if(playerController_GM0.regularState == RegularState.WAITINGFORTUTORIAL && LevelingSystem.Instance.GetLevelNumber() == 1) { 
                
              
                if(PlayerPrefs.GetInt(PlayerPrefKeys.kBoostCount) != 0) {
                    gm_cameraPos = new Vector3(0f,  4f, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, gm_cameraPos, Speed * Time.deltaTime);
                }

                else return;
                

            }
            else {

                if(playerController_GM0.currentTarget != null)
                gm_cameraPos = new Vector3(0f, playerController_GM0.currentTarget.position.y + 2f, transform.position.z);

                if(transform.position.y < gm_cameraPos.y)
                //transform.position += Vector3.up * Speed * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, gm_cameraPos, Speed * Time.deltaTime);

            }
        }

      

    }
    void Handle_GM1_CameraMovement() {

       
            if(playerController_GM.gM_State == GM_State.EXIT ||
            playerController_GM.gM_State == GM_State.WAITINGFORTUTORIAL ||
            playerController_GM.gM_State == GM_State.STARTING) { 
                
             gm_cameraPos = new Vector3(0f, player.transform.position.y + 4.5f, transform.position.z);
                transform.position = Vector3.Lerp(transform.position,gm_cameraPos, Speed * Time.deltaTime);

            }
            //else if()
            else {

            
            gm_cameraPos = new Vector3(0f, player.transform.position.y, transform.position.z);

                if(transform.position.y <gm_cameraPos.y)
               
                transform.position = Vector3.Lerp(transform.position,gm_cameraPos, Speed * Time.deltaTime);

            }

    }

     void Handle_GM2_CameraMovement() {

       if(playerController_GM.gM_State == GM_State.EXIT ||
          playerController_GM.gM_State == GM_State.WAITINGFORTUTORIAL) { 
                
                gm_cameraPos = new Vector3(0f, player.transform.position.y + 4.5f, transform.position.z);
                transform.position = Vector3.Lerp(transform.position,gm_cameraPos, Speed * Time.deltaTime);

            }
       else if(playerController_GM.gM_State == GM_State.STARTING) {
                transform.position = Vector3.Lerp(transform.position,gm_cameraPos, Speed * Time.deltaTime);
         }
        else {
        
                gm_cameraPos = new Vector3(0f, player.transform.position.y, transform.position.z);

                if(transform.position.y <gm_cameraPos.y)
               
                transform.position = Vector3.Lerp(transform.position,gm_cameraPos, Speed * Time.deltaTime);

            }

    }

    void Handle_GM4_CameraMovement() {

           if(playerController_GM.gM_State == GM_State.EXIT ||
            playerController_GM.gM_State == GM_State.WAITINGFORTUTORIAL
            ) { 
                
             gm_cameraPos = new Vector3(0f, player.transform.position.y + 2.5f, transform.position.z);
                transform.position = Vector3.Lerp(transform.position,gm_cameraPos, Speed * Time.deltaTime);

            }
            else if(playerController_GM.gM_State == GM_State.STARTING) {
                transform.position = new Vector3(0f, player.transform.position.y + 2.5f, transform.position.z);
            }
            else {

            
            gm_cameraPos = new Vector3(0f, player.transform.position.y, transform.position.z);

                if(transform.position.y <gm_cameraPos.y)
               
                transform.position = Vector3.Lerp(transform.position,gm_cameraPos, Speed * Time.deltaTime);

            }
    }

     void Handle_GM5_CameraMovement() {
         if(playerController_GM.gM_State == GM_State.EXIT ||
            playerController_GM.gM_State == GM_State.WAITINGFORTUTORIAL) { 
                
             gm_cameraPos = new Vector3(0f, player.transform.position.y + 4.5f, transform.position.z);
                transform.position = Vector3.Lerp(transform.position,gm_cameraPos, Speed * Time.deltaTime);

            }
            else if(playerController_GM.gM_State == GM_State.STARTING) {
                return;
            }


            else {

            
            gm_cameraPos = new Vector3(0f, player.transform.position.y, transform.position.z);

                if(transform.position.y <gm_cameraPos.y)
               
                transform.position = Vector3.Lerp(transform.position,gm_cameraPos, Speed * Time.deltaTime);

            }

    }
    
    
}
