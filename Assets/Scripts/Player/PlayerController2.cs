using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lean.Pool;
using UnityEngine.Analytics;

 public enum PlayState { MENU, REGULAR, GAMEMODE1, GAMEMODE2, GAMEMODE3, GAMEMODE4 }

public class PlayerController2 : MonoBehaviour
{
    public static PlayerController2 Instance;
    public static event Action<PlayState> OnPlayStateChanged;
    public event EventHandler OnNewPlayerSpawned;
    public PlayState playState; 
    GameManager gameManager;
    ScoreManager scoreManager;
    CameraController cameraController;
    MenuManager menuManager;
    GameOverUI gameOverUI;
    StoreSaveLoad storeSaveLoad;

    public GameObject player;
    PlayerController_GM0 playerController_GM0;

    public List<GameObject> playerPrefabList = new List<GameObject>();
    private GameObject playerPrefabMenu;
    private GameObject playerPrefabGM0;
    private GameObject playerPrefabGM1;
    private GameObject playerPrefabGM2;
    private GameObject playerPrefabGM3;
    private GameObject playerPrefabGM4;
    private GameObject lvlUpFx;
    private GameObject impactFx;
    private GameObject bulletPrefab;
    private Vector3 replayPos;
    [SerializeField] private Transform destructionPoint; //used as a 'reset position for continue button
    //public int continueBaseAmount = 10;
    public int[] continueCost = new int[] { 250, 500, 1000, 1500, 2000 };
    public int continueCostIndex;
    public bool playerIsReady = false;
    public int gameModeIndex;
    private bool lastContinueUsed = false;
   

    void Awake() {
        if(Instance != null) {
            return;
        }
        
        Instance = this;

        cameraController = FindObjectOfType<CameraController>();
        AdsManager.Instance.OnInterstitialAdFinished += AdsManager_OnInterstitialAdFinished;

         //load playerKeys
         if(PlayerPrefs.HasKey(PlayerPrefKeys.kCurrentSkin)) {
            PlayerPrefKeys.kCurrentSkinInt = PlayerPrefs.GetInt(PlayerPrefKeys.kCurrentSkin, 1);
        }
         if(PlayerPrefs.HasKey(PlayerPrefKeys.kTutorialStep)) {
            PlayerPrefKeys.kTutorialInt = PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep, 0);
        }
        
        //populate the player prefabs list according to playerkey
        AddSelectedSkinToList(); 
        //SpawnFirstPlayer();


    }
    //  void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //      SpawnFirstPlayer();
    // }

    void OnDestroy() {
        AdsManager.Instance.OnInterstitialAdFinished -= AdsManager_OnInterstitialAdFinished;
    }
          
    void Start() {

        storeSaveLoad = FindObjectOfType<StoreSaveLoad>();  
        gameOverUI = FindObjectOfType<GameOverUI>();
        menuManager = FindObjectOfType<MenuManager>();
        player = GameObject.FindWithTag("Player");  

        replayPos = player.transform.position;
        continueCostIndex = 0;
          
        UpdatePlayState(PlayState.MENU); 

    }

    void Update() {
        player = GameObject.FindWithTag("Player"); 

     //  Debug.Log(Screen.width);

    }


     public void UpdatePlayState(PlayState newState) {  
        playState = newState;
        UpdateGameModeIndex();

        switch (newState) {
            case PlayState.MENU:
                SetPlayerPrefab_GM(2);
                player.transform.position = replayPos; //to replace the player when we get back to menu
            break;
            case PlayState.REGULAR:
                SetPlayerPrefab_GM(0);                
            break;
            case PlayState.GAMEMODE1:
                SetPlayerPrefab_GM(1);
            break;
            case PlayState.GAMEMODE2:
                SetPlayerPrefab_GM(6);
            break;
            case PlayState.GAMEMODE3:
                SetPlayerPrefab_GM(7);
            break;
            case PlayState.GAMEMODE4:
                SetPlayerPrefab_GM(8);
            break;
        
        } 
        OnPlayStateChanged?.Invoke(newState);
     }

      private void UpdateGameModeIndex() {  // assign an int to each gamemode. tutorial key is assigned to this value to show tips again on what should I do button, menuManager      


        switch (playState) {
            // case PlayState.REGULAR:
            //     gameModeIndex = 0;              
            // break;
            case PlayState.GAMEMODE1:
                gameModeIndex = 1; 
            break;
            case PlayState.GAMEMODE2:
                gameModeIndex = 2; 
            break;
            case PlayState.GAMEMODE3:
                gameModeIndex = 3; 
            break;
             case PlayState.GAMEMODE4:
                gameModeIndex = 4; 
            break;
        
        } 

        if(gameModeIndex != 0)
        PlayerPrefKeys.kTutorialInt = gameModeIndex ;

        PlayerPrefs.SetInt(PlayerPrefKeys.kTutorialStep, PlayerPrefKeys.kTutorialInt);
        PlayerPrefs.Save();
     }

    public void SpawnFirstPlayer() {

        player = LeanPool.Spawn(playerPrefabList[2], new Vector3 (0f, -0.45f, 0f), Quaternion.identity);
    }


     public void SetPlayerPrefab_GM(int gameModeIndex) {

       player = GameObject.FindWithTag("Player");          
       GameObject[] inGamePlayers = GameObject.FindGameObjectsWithTag("Player");
      
       player = LeanPool.Spawn(playerPrefabList[gameModeIndex], player.transform.position, player.transform.rotation);
     
         if ( inGamePlayers.Length != 0 ) {
             foreach (GameObject item in inGamePlayers) {
                 LeanPool.Despawn(item);
             }
         } 
         
        //fire event to get the new player and grab it in fx, cam, level gen
        OnNewPlayerSpawned?.Invoke(this, EventArgs.Empty);
     }


     public void ContinueButton() { 

       // if(ScoreManager.Instance.TrySpendStarAmount(continueCost[continueCostIndex])) {
            
             gameOverUI = FindObjectOfType<GameOverUI>();
             player = GameObject.FindWithTag("Player");

             if(continueCostIndex >= 5/*continueCost.Length - 1*/) {
                    lastContinueUsed = true;

                    gameOverUI.DarkenButtonText(lastContinueUsed);     
             }
            if(continueCostIndex < 5/*continueCost.Length -1*/ ) {
                 continueCostIndex++;
           } 
           Analytics.CustomEvent("Continue click" + continueCostIndex);

            switch (playState)  {
                case PlayState.REGULAR:
                playerController_GM0 = player.GetComponent<PlayerController_GM0>();
                player.transform.position = destructionPoint.position;
                DeactivatePlanetsBelow();
                GameManager.Instance.UpdateGameState(GameState.PLAYMODE);
                playerController_GM0.regularState = RegularState.AUTOPILOT;
                break;

            }
        
        //}
       
     }

    private void AdsManager_OnInterstitialAdFinished(object sender, EventArgs e) {
        ContinueButton();
    }

     private void DeactivatePlanetsBelow() { //for continue button
         GameObject[] candidates = GameObject.FindGameObjectsWithTag("Planet");

        if(candidates.Length != 0) {
                foreach (GameObject planet in candidates)
            {
                if(planet.transform.position.y < cameraController.transform.position.y - 5f)
                    //LeanPool.Despawn(planet);
                    planet.SetActive(false);
            }
        }
        
     }
     public void ResetContinueCost() {
         continueCostIndex = 0;
         lastContinueUsed = false;
     }
    public void PlayerToReplayPos() { //when clicking the replay button
                player.transform.position = replayPos;
    }

    public void PlayerIsReady() { //when player clicks the 'im ready button' in menuManager
                playerIsReady = true;
    }

     public void AddSelectedSkinToList() {

         if(playerPrefabList.Count != 0) playerPrefabList.Clear();

        switch (PlayerPrefKeys.kCurrentSkinInt) //Do not change the item order : gm0, gm1, MenuPf, lvlupFx, impactFx, bulletFx, gm++
        {
            case 1: //basic set 
            playerPrefabGM0 = StoreItem.GetPlayerPrefabGM0(StoreItem.ItemType.BasicBall);
            playerPrefabList.Add(playerPrefabGM0);

            playerPrefabGM1 = StoreItem.GetPlayerPrefabGM1(StoreItem.ItemType.BasicBall);
            playerPrefabList.Add(playerPrefabGM1);

            playerPrefabMenu = StoreItem.GetPlayerPrefabMenu(StoreItem.ItemType.BasicBall);
            playerPrefabList.Add(playerPrefabMenu);

            lvlUpFx = StoreItem.GetLvlUpFX(StoreItem.ItemType.BasicBall);
            playerPrefabList.Add(lvlUpFx);

            impactFx = StoreItem.GetImpactFX(StoreItem.ItemType.BasicBall);
            playerPrefabList.Add(impactFx);
            
            bulletPrefab = StoreItem.GetBulletPrefab(StoreItem.ItemType.BasicBall);
            playerPrefabList.Add(bulletPrefab);

            playerPrefabGM2 = StoreItem.GetPlayerPrefabGM2(StoreItem.ItemType.BasicBall);
            playerPrefabList.Add(playerPrefabGM2);

            playerPrefabGM3 = StoreItem.GetPlayerPrefabGM3(StoreItem.ItemType.BasicBall);
            playerPrefabList.Add(playerPrefabGM3);

            playerPrefabGM4 = StoreItem.GetPlayerPrefabGM4(StoreItem.ItemType.BasicBall);
            playerPrefabList.Add(playerPrefabGM4);
            break;

            case 2: //frost set
            playerPrefabGM0 = StoreItem.GetPlayerPrefabGM0(StoreItem.ItemType.FrostBall);
            playerPrefabList.Add(playerPrefabGM0);

            playerPrefabGM1 = StoreItem.GetPlayerPrefabGM1(StoreItem.ItemType.FrostBall);
            playerPrefabList.Add(playerPrefabGM1);

            playerPrefabMenu = StoreItem.GetPlayerPrefabMenu(StoreItem.ItemType.FrostBall);
            playerPrefabList.Add(playerPrefabMenu);

            lvlUpFx = StoreItem.GetLvlUpFX(StoreItem.ItemType.FrostBall);
            playerPrefabList.Add(lvlUpFx);

            impactFx = StoreItem.GetImpactFX(StoreItem.ItemType.FrostBall);
            playerPrefabList.Add(impactFx);

            bulletPrefab = StoreItem.GetBulletPrefab(StoreItem.ItemType.FrostBall);
            playerPrefabList.Add(bulletPrefab);

            playerPrefabGM2 = StoreItem.GetPlayerPrefabGM2(StoreItem.ItemType.FrostBall);
            playerPrefabList.Add(playerPrefabGM2);

            playerPrefabGM3 = StoreItem.GetPlayerPrefabGM3(StoreItem.ItemType.FrostBall);
            playerPrefabList.Add(playerPrefabGM3);

            playerPrefabGM4 = StoreItem.GetPlayerPrefabGM4(StoreItem.ItemType.FrostBall);
            playerPrefabList.Add(playerPrefabGM4);
            
            break;

            case 3: //Nature set
            playerPrefabGM0 = StoreItem.GetPlayerPrefabGM0(StoreItem.ItemType.NatureBall);
            playerPrefabList.Add(playerPrefabGM0);

            playerPrefabGM1 = StoreItem.GetPlayerPrefabGM1(StoreItem.ItemType.NatureBall);
            playerPrefabList.Add(playerPrefabGM1);

            playerPrefabMenu = StoreItem.GetPlayerPrefabMenu(StoreItem.ItemType.NatureBall);
            playerPrefabList.Add(playerPrefabMenu);

            lvlUpFx = StoreItem.GetLvlUpFX(StoreItem.ItemType.NatureBall);
            playerPrefabList.Add(lvlUpFx);

            impactFx = StoreItem.GetImpactFX(StoreItem.ItemType.NatureBall);
            playerPrefabList.Add(impactFx);

            bulletPrefab = StoreItem.GetBulletPrefab(StoreItem.ItemType.NatureBall);
            playerPrefabList.Add(bulletPrefab);

            playerPrefabGM2 = StoreItem.GetPlayerPrefabGM2(StoreItem.ItemType.NatureBall);
            playerPrefabList.Add(playerPrefabGM2);

            playerPrefabGM3 = StoreItem.GetPlayerPrefabGM3(StoreItem.ItemType.NatureBall);
            playerPrefabList.Add(playerPrefabGM3);

            playerPrefabGM4 = StoreItem.GetPlayerPrefabGM4(StoreItem.ItemType.NatureBall);
            playerPrefabList.Add(playerPrefabGM4);
            break;

            case 4: //xmas set
            playerPrefabGM0 = StoreItem.GetPlayerPrefabGM0(StoreItem.ItemType.XmasBall);
            playerPrefabList.Add(playerPrefabGM0);

            playerPrefabGM1 = StoreItem.GetPlayerPrefabGM1(StoreItem.ItemType.XmasBall);
            playerPrefabList.Add(playerPrefabGM1);

            playerPrefabMenu = StoreItem.GetPlayerPrefabMenu(StoreItem.ItemType.XmasBall);
            playerPrefabList.Add(playerPrefabMenu);

            lvlUpFx = StoreItem.GetLvlUpFX(StoreItem.ItemType.XmasBall);
            playerPrefabList.Add(lvlUpFx);

            impactFx = StoreItem.GetImpactFX(StoreItem.ItemType.XmasBall);
            playerPrefabList.Add(impactFx);

            bulletPrefab = StoreItem.GetBulletPrefab(StoreItem.ItemType.NatureBall); //not used anymore so left nature
            playerPrefabList.Add(bulletPrefab);

            playerPrefabGM2 = StoreItem.GetPlayerPrefabGM2(StoreItem.ItemType.XmasBall);
            playerPrefabList.Add(playerPrefabGM2);

            playerPrefabGM3 = StoreItem.GetPlayerPrefabGM3(StoreItem.ItemType.XmasBall);
            playerPrefabList.Add(playerPrefabGM3);

            playerPrefabGM4 = StoreItem.GetPlayerPrefabGM4(StoreItem.ItemType.XmasBall);
            playerPrefabList.Add(playerPrefabGM4);
            break;

            case 5: //love set
            playerPrefabGM0 = StoreItem.GetPlayerPrefabGM0(StoreItem.ItemType.LoveBall);
            playerPrefabList.Add(playerPrefabGM0);

            playerPrefabGM1 = StoreItem.GetPlayerPrefabGM1(StoreItem.ItemType.LoveBall);
            playerPrefabList.Add(playerPrefabGM1);

            playerPrefabMenu = StoreItem.GetPlayerPrefabMenu(StoreItem.ItemType.LoveBall);
            playerPrefabList.Add(playerPrefabMenu);

            lvlUpFx = StoreItem.GetLvlUpFX(StoreItem.ItemType.LoveBall);
            playerPrefabList.Add(lvlUpFx);

            impactFx = StoreItem.GetImpactFX(StoreItem.ItemType.LoveBall);
            playerPrefabList.Add(impactFx);

            bulletPrefab = StoreItem.GetBulletPrefab(StoreItem.ItemType.NatureBall); //not used anymore so left nature
            playerPrefabList.Add(bulletPrefab);

            playerPrefabGM2 = StoreItem.GetPlayerPrefabGM2(StoreItem.ItemType.LoveBall);
            playerPrefabList.Add(playerPrefabGM2);

            playerPrefabGM3 = StoreItem.GetPlayerPrefabGM3(StoreItem.ItemType.LoveBall);
            playerPrefabList.Add(playerPrefabGM3);

            playerPrefabGM4 = StoreItem.GetPlayerPrefabGM4(StoreItem.ItemType.LoveBall);
            playerPrefabList.Add(playerPrefabGM4);
            break;

            case 6: //thunder set
            playerPrefabGM0 = StoreItem.GetPlayerPrefabGM0(StoreItem.ItemType.ThunderBall);
            playerPrefabList.Add(playerPrefabGM0);

            playerPrefabGM1 = StoreItem.GetPlayerPrefabGM1(StoreItem.ItemType.ThunderBall);
            playerPrefabList.Add(playerPrefabGM1);

            playerPrefabMenu = StoreItem.GetPlayerPrefabMenu(StoreItem.ItemType.ThunderBall);
            playerPrefabList.Add(playerPrefabMenu);

            lvlUpFx = StoreItem.GetLvlUpFX(StoreItem.ItemType.ThunderBall);
            playerPrefabList.Add(lvlUpFx);

            impactFx = StoreItem.GetImpactFX(StoreItem.ItemType.ThunderBall);
            playerPrefabList.Add(impactFx);

            bulletPrefab = StoreItem.GetBulletPrefab(StoreItem.ItemType.ThunderBall); //not used anymore so left nature
            playerPrefabList.Add(bulletPrefab);

            playerPrefabGM2 = StoreItem.GetPlayerPrefabGM2(StoreItem.ItemType.ThunderBall);
            playerPrefabList.Add(playerPrefabGM2);

            playerPrefabGM3 = StoreItem.GetPlayerPrefabGM3(StoreItem.ItemType.ThunderBall);
            playerPrefabList.Add(playerPrefabGM3);

            playerPrefabGM4 = StoreItem.GetPlayerPrefabGM4(StoreItem.ItemType.ThunderBall);
            playerPrefabList.Add(playerPrefabGM4);
            break;


            case 0: //showing the boost item 
            playerPrefabMenu = StoreAssets.i.pf_boostStoreObject; //need to fill the list at least from 0 to 2 cuz store looks for the [2] item in list to display
            playerPrefabList.Add(playerPrefabMenu);

            playerPrefabMenu = StoreAssets.i.pf_boostStoreObject;
            playerPrefabList.Add(playerPrefabMenu);

            playerPrefabMenu = StoreAssets.i.pf_boostStoreObject;
            playerPrefabList.Add(playerPrefabMenu);
            break;
        }
        

     }

   
}
