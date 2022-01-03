using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
using Lean.Pool;

public class LevelGenerator : MonoBehaviour
{
   public event EventHandler OnMinigameOver;
   public event Action<float> OnTimerChanged;
   public event Action<float> OnTimerOver;
   GameObject player;
   PlayerController2 playerController2;
   PlayerController_GM0 playerController_GM0;
   PlayerController_GM playerController_GM;
   private StarGenerator starGenerator;

   [Header("Menu")]
   public GameObject planetMenu;

   [Header("Gamemode 0")]
   public float planetSpriteWidth;
   private GameObject generationPoint;
   CircleCollider2D planetCollider;
   private float distanceBetween;
   [SerializeField] private float distanceBetweenMin;
   [SerializeField] private float distanceBetweenMax;
   private int planetSelectorIndex;
   private Vector3 posCurrentPlanet;
   private Vector3 posPreviousPlanet;
   [SerializeField] private float starSpawnRateThreshold;
   public ObjectPool[] objectPools;
   private int updatedPoolLength;

   [Header("Gamemode 1")]
   public float eventDuration;
   public GameObject asteroidPrefab;
   public GameObject bonusRocketPrefab; //is actually a satellite but name needs to be rocket as it is checked in its Asteroid Controller class
   [SerializeField] private Transform asteroidTargetPointPrefab;
   [SerializeField] private Transform asteroidTargetPoint;
   [SerializeField] private Transform asteroidSpawnPoint;
   [SerializeField] private Transform asteroidSpawner;
   public Camera cam;
   private Vector3 gm01StartPos;
   public Vector3 asteroidMoveDir;
   private float gm01_timer;
   private float gm01_spawnTimer;
   private float gm01_spawnRate;
  
   [Header("Gamemode 2")]
   public Transform[] planetPrefabs;
   private float gm02_timer;
   private float gm02_spawnTimer;
   private float gm02_spawnRate;
   private Vector3 spawnPos;
   private int numberOfPlanet;
   private List<Transform> planetList = new List<Transform>();
   private Vector3 planetSize;
   float planetSizeDelta;
   private Transform planetToBeFoundNow;
   PlanetController planetController;
   private bool listFull = false;
   float timerCleanUp;

  // [Header("Gamemode 4")]
   private GameObject galaxyRef;
   private GameObject obstacle;
   private float spawnTimer;
   private float[] rawCounter = new float[] {1,3,4,6,7};
   private int rawIndex;
   private List<GameObject> obstaclesList = new List<GameObject>();
   PlayerController_GM04 playerController_GM04;


 
    
    void Awake() {
     
        PlayerController2.OnPlayStateChanged += PlayerController2_OnPlayStateChanged;

        player = GameObject.FindWithTag("Player");
//        playerController_GM0 = player.GetComponent<PlayerController_GM0>();
        playerController_GM = FindObjectOfType<PlayerController_GM>();

        starGenerator = FindObjectOfType<StarGenerator>();
        generationPoint = GameObject.FindWithTag("GenerationPoint");

        PlayerController2.Instance.SpawnFirstPlayer();

    }

    void OnDestroy() {
        PlayerController2.OnPlayStateChanged -= PlayerController2_OnPlayStateChanged;
      
    }

    void Start() {
      
        updatedPoolLength = objectPools.Length - 1;
    }
  
    private void PlayerController2_OnPlayStateChanged(PlayState state) {
        
       
        switch (state) {

            case PlayState.REGULAR:
            player = PlayerController2.Instance.player;
            playerController_GM0 = player.GetComponent<PlayerController_GM0>();
            //removing the last item of the pool (sattelite) to planet selector index so it never spawns first 
            updatedPoolLength = objectPools.Length - 1;

            if(generationPoint != null) {
            Vector3 replacePos = new Vector3(generationPoint.transform.position.x, generationPoint.transform.position.y - .1f, 0); //cuz generation point is on z = -10 so needs to be 0  
            transform.position = replacePos;
            }
            break;

            case PlayState.MENU:
            
            break;

            case PlayState.GAMEMODE4:
            playerController_GM = FindObjectOfType<PlayerController_GM>();
            if(generationPoint != null) {
            Vector3 replacePos = new Vector3(generationPoint.transform.position.x, generationPoint.transform.position.y + 5f, 0); //cuz generation point is on z = -10 so needs to be 0  
            transform.position = replacePos;
            }
            break;

            default:
            //get the base class controller ref only in GAMEMODES for mini game cuz it doesnt exist if we're not in minigame mode
            playerController_GM = FindObjectOfType<PlayerController_GM>();
            break;

        }
       
    }
  
    void Update()
    {
        

            switch (PlayerController2.Instance.playState) {
                case PlayState.MENU:
                DeactivatePlanets();
                break;
                case PlayState.REGULAR:
                HandleRegularGeneration();
                break;
                case PlayState.GAMEMODE1:
                Handle_GM1_Generation();
                break;
                case PlayState.GAMEMODE2:
                Handle_GM2_Generation();
                break;
                case PlayState.GAMEMODE4:
                Handle_GM4_Generation();
                break;

            }

    }

    
    void planetSizeRandomizer(GameObject planet) {
        float planetSize = Random.Range(0.3f, 0.6f);
        planet.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
    }

    void SpawnPlanet() {


            planetSelectorIndex = Random.Range(0, updatedPoolLength);
            //re adding the last item of the pool (sattelite) to planet selector index
            updatedPoolLength = objectPools.Length; 

            //instantiate the inactive object give him size and position
            GameObject newPlanet = objectPools[planetSelectorIndex].GetPooledObject(); 
           // planetSizeRandomizer(newPlanet);
            newPlanet.transform.position = transform.position;
            newPlanet.transform.rotation = transform.rotation;
           
            //get radius and a distance to avoid placing Bonuss too close to each other
            planetSpriteWidth = newPlanet.transform.localScale.x * 2f;
            distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax);

            //get the random x position 
            float minXPos = (-2.3f + planetSpriteWidth);
            float maxXPos = (2.3f - planetSpriteWidth);
            float randomXPos = Random.Range(minXPos, maxXPos);

            //place the levelGenertor
            transform.position = new Vector3 (randomXPos, transform.position.y + planetSpriteWidth + distanceBetween, transform.position.z); //place it 
            newPlanet.SetActive(true);

            //Instantiate stars 5%chance each planet spawn
            if(Random.Range(0f, 100f) < starSpawnRateThreshold) {
               
                posCurrentPlanet = newPlanet.transform.position;
                posPreviousPlanet = FindPreviousPlanet().position;
             
                SpawnStarsBetween2Planets(posCurrentPlanet, posPreviousPlanet);
            }
             
            
    }

    public void DeactivatePlanets() {
        GameObject[] activePlanets = GameObject.FindGameObjectsWithTag("Planet");
      
         if ( activePlanets.Length != 0 ) {
             foreach (GameObject item in activePlanets) {
                 item.SetActive(false);
             }
         }
    }

    public void HandleRegularGeneration() {  //pc gm0 does not inherit from PC_GM
 
        if(playerController_GM0 != null) {
                if (playerController_GM0.regularState == RegularState.EXIT ||
                playerController_GM0.regularState == RegularState.WAITINGFORTUTORIAL) {
                 
                    return;
                }
                else if( playerController_GM0.regularState == RegularState.BOOST) {
                   
                       // transform.position = new Vector3(generationPoint.transform.position.x, generationPoint.transform.position.y - .1f, 0);
                        // if(transform.position.y < generationPoint.transform.position.y)
                        // SpawnPlanet();
                        transform.position = new Vector3(0, generationPoint.transform.position.y - 0.1f, 0f);
                    
                }

                else {
                    if(transform.position.y < generationPoint.transform.position.y)
                    SpawnPlanet();
                   
                }
        }
    }

    void Handle_GM1_Generation() {       
       
            if (playerController_GM.gM_State == GM_State.WAITINGFORTUTORIAL) {
                 return;
            }
            else if (playerController_GM.gM_State == GM_State.STARTING) {
                SpawnAsteroids();
            }
             else if (playerController_GM.gM_State == GM_State.COUNTING) {
                return;
            }
        
    }

    void Handle_GM2_Generation() {

         if (playerController_GM.gM_State == GM_State.WAITINGFORTUTORIAL) {
                  //set up game duration
                    if(LevelingSystem.Instance.GetLevelNumber() >= 6) {
                        eventDuration = (1/(float)LevelingSystem.Instance.GetLevelNumber()) * 160f;
                        
                        if(eventDuration < 10f)
                        eventDuration = 10f;
                    }
                    else if(LevelingSystem.Instance.GetLevelNumber() < 6) {
                        eventDuration = 20f;
                    }
            }
            else if (playerController_GM.gM_State == GM_State.STARTING) {
                SpawnPlanets_GM2();
            }
             else if (playerController_GM.gM_State == GM_State.COUNTING) {
                return;
            }

    }

   private void Handle_GM4_Generation() {
        if (playerController_GM.gM_State == GM_State.WAITINGFORTUTORIAL) {
                  //set up game duration
                    if(LevelingSystem.Instance.GetLevelNumber() >= 6) {
                        eventDuration = (1/(float)LevelingSystem.Instance.GetLevelNumber()) * 160f;
                        
                        if(eventDuration < 5f)
                        eventDuration = 5f;
                    }
                    else if(LevelingSystem.Instance.GetLevelNumber() < 6) {
                        eventDuration = 20f;
                    }

            }
            else if (playerController_GM.gM_State == GM_State.STARTING) {
                if(transform.position.y < generationPoint.transform.position.y)
               
                Spawn_GM04();
            }
             else if (playerController_GM.gM_State == GM_State.COUNTING) {
                return;
            }

    }

    void Spawn_GM04() {


            //planetSelectorIndex = Random.Range(0, updatedPoolLength);
    
            int spawnIndex = Random.Range(0,100);

            if(spawnIndex <= 5) {
                GameObject newBonus = LeanPool.Spawn(bonusRocketPrefab, transform.position, transform.rotation);
                planetSpriteWidth = newBonus.transform.localScale.x * 2f;

                if(newBonus != null)
                LeanPool.Despawn(newBonus, 4f);
            }

            else {
                GameObject newPlanet = LeanPool.Spawn(StoreAssets.i.pf_asteroids[Random.Range(0, StoreAssets.i.pf_asteroids.Length)], transform.position, transform.rotation);
                float planetSize = Random.Range(0.3f, 0.5f);
                newPlanet.transform.localScale = new Vector3(planetSize, planetSize, planetSize);
                planetSpriteWidth = newPlanet.transform.localScale.x * 2f;
                //LeanPool.Despawn(newPlanet, 4f);
            }
            
           
            //get radius and a distance to avoid placing Bonuss too close to each other
           
            distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax);

            //get the random x position 
            // float minXPos = (-2.3f + planetSpriteWidth);
            // float maxXPos = (2.3f - planetSpriteWidth);
            float minXPos = (PlayerController2.Instance.player.transform.position.x -1f);
            float maxXPos = (PlayerController2.Instance.player.transform.position.x +1f);
            float randomXPos = Random.Range(minXPos, maxXPos);

            //place the levelGenertor
            transform.position = new Vector3 (randomXPos, transform.position.y + planetSpriteWidth + distanceBetween, transform.position.z); //place it 
            

            // //Instantiate stars 5%chance each planet spawn
            // if(Random.Range(0f, 100f) < starSpawnRateThreshold) {
               
            //     posCurrentPlanet = newPlanet.transform.position;
            //     posPreviousPlanet = FindPreviousPlanet().position;
             
            //     SpawnStarsBetween2Planets(posCurrentPlanet, posPreviousPlanet);
            // }
             
             
            
    }


     private void Handle_GM5_Generation() {
        if (playerController_GM.gM_State == GM_State.WAITINGFORTUTORIAL) {
                  //set up game duration
                    if(LevelingSystem.Instance.GetLevelNumber() >= 6) {
                        eventDuration = (1/(float)LevelingSystem.Instance.GetLevelNumber()) * 160f;
                        
                        if(eventDuration < 5f)
                        eventDuration = 5f;
                    }
                    else if(LevelingSystem.Instance.GetLevelNumber() < 6) {
                        eventDuration = 20f;
                    }

                   //get galaxy ref
                   galaxyRef = GameObject.Find("Galaxy_pf");

                   SpawnObstacles_GM5();
            }
            else if (playerController_GM.gM_State == GM_State.STARTING) {
                
            }
             else if (playerController_GM.gM_State == GM_State.COUNTING) {
                return;
            }

    }

    void SpawnObstacles_GM5() {  


        spawnTimer += Time.deltaTime;
        float spawnRate = Random.Range(.5f, 1.5f);
    
            if(rawIndex < rawCounter.Length) {
             
                if(obstaclesList.Count < 5) {
                
                        for (int i = 0 ; i < 5; i++) {
                            
                            if(spawnTimer > spawnRate) {
                        
                                //set up spawn position
                                float xOffset = 6f;//Random.Range (-6f, 6f);
                                    
                                spawnPos = new Vector3 (transform.position.x + xOffset, PlayerController2.Instance.player.transform.position.y + rawCounter[rawIndex], 0f);
                            
                                GameObject obstacle = LeanPool.Spawn(StoreAssets.i.pf_asteroids[Random.Range(0, StoreAssets.i.pf_asteroids.Length)], 
                                spawnPos, Quaternion.Euler(0,0,Random.Range(0, 360f)));
                                planetSizeRandomizer(obstacle);
                                obstaclesList.Add(obstacle);
                                spawnTimer = 0f;
                                            
                            }       
                    
                        }
                    } 

                else if(obstaclesList.Count >= 5) {
                    rawIndex++;
                    obstaclesList.Clear();
                }
               
            } 
  
    }

   

    // private void GM04_OnGalaxySpawned(object sender, EventArgs e) {
    //     Debug.Log("galaxy spawned");
    //     LeanPool.Spawn(StoreAssets.i.pf_gm4Objects[0], new Vector3(galaxyRef.transform.position.x, galaxyRef.transform.position.y + 7f,0f), Quaternion.identity);
    // }

   
    
    void SpawnAsteroids() {
        transform.position = new Vector3 (0f, cam.transform.position.y, 0f);
        eventDuration = 30f;
        //start minigame timers and set spawn rate according to lvl
        gm01_spawnRate = (1.3f/Mathf.Log((float) LevelingSystem.Instance.GetLevelNumber(), 2)); //(0.5f / (float) LevelingSystem.Instance.GetLevelNumber()) * 5f;
        gm01_timer += Time.deltaTime;
        gm01_spawnTimer += Time.deltaTime;

        OnTimerChanged?.Invoke(eventDuration - gm01_timer); 

      if(gm01_timer < eventDuration) {

          if(gm01_spawnTimer > gm01_spawnRate) {
            
            int randomizeRotationIndex = Random.Range(0, 10);

            // if(randomizeRotationIndex < 6) {
            //     asteroidSpawner.eulerAngles = new Vector3(0,0,Random.Range(-15, 15));
            //     transform.position = new Vector3(Random.Range(-2f, 2f), cam.transform.position.y, 0f );
                
            // }
            
                asteroidSpawner.eulerAngles = new Vector3(0,0,0);//Random.Range(-90, 90));
                transform.position = new Vector3(Random.Range(-2f, 2f), cam.transform.position.y, 0f );
            
            
             
              //get direction from spawn pos to targetpos 
              asteroidMoveDir = asteroidTargetPoint.position - asteroidSpawnPoint.position;

                int goldenAsteroidSpawnThresHold = Random.Range(0, 100);
                //asteroid movement is handled in its own class
                if(goldenAsteroidSpawnThresHold <= 3) {
                    GameObject newAsteroid = LeanPool.Spawn(bonusRocketPrefab, asteroidSpawnPoint.position, Quaternion.identity);
                    LeanPool.Despawn(newAsteroid, 5f);
                }
                else {
                    GameObject newAsteroid = LeanPool.Spawn(asteroidPrefab, asteroidSpawnPoint.position, Quaternion.identity); 
                    float randomSize = Random.Range(0.25f, 0.45f);
                    newAsteroid.transform.localScale = new Vector3(randomSize, randomSize, randomSize);
                    LeanPool.Despawn(newAsteroid, 5f);
                }

                gm01_spawnTimer = 0f; 
        
        }
       
      }
     
      else if (gm01_timer > eventDuration) {

          GameObject[] asteroids = GameObject.FindGameObjectsWithTag("rock");

            foreach (GameObject item in asteroids){
                    item.GetComponent<AsteroidController>().ExplodeAndDespawn();
                    AudioManager.Instance.Play("asteroid_touch");
                }
            gm01_timer = 0;
            OnMinigameOver?.Invoke(this, EventArgs.Empty);
        
        }
    }

    private void SpawnPlanets_GM2() {
         //set generator to center of screen
        transform.position = new Vector3 (0f, Camera.main.transform.position.y, 0f);

      
    

        //decide how many we spawn 
        if(LevelingSystem.Instance.GetLevelNumber() >= 6 && LevelingSystem.Instance.GetLevelNumber() <= 10) {
            numberOfPlanet = 6;
        }
        else if(LevelingSystem.Instance.GetLevelNumber() < 6) {
            numberOfPlanet = LevelingSystem.Instance.GetLevelNumber();
        }
        else if(LevelingSystem.Instance.GetLevelNumber() > 10) {
            numberOfPlanet = 6;
        }
        

        //set planet size rules according to how many we'll spawn
        if(LevelingSystem.Instance.GetLevelNumber() >= 7) {
            planetSizeDelta = .275f;
        }
        else if(LevelingSystem.Instance.GetLevelNumber() < 7) {
            // planetSizeDelta = .2f;
           planetSizeDelta = (1/(float)LevelingSystem.Instance.GetLevelNumber()) * 1.5f;
        }
        planetSize = new Vector3(planetSizeDelta, planetSizeDelta, planetSizeDelta);

        //set up spawn frequency
        gm02_spawnRate = .8f;
        gm02_spawnTimer += Time.deltaTime;
     
        if(listFull == false/*planetList.Count < numberOfPlanet*/) { //spawn planets if list hasnt reach its full size 
             for (int i = 0 ; i < numberOfPlanet; i++) {
                 
                    if(gm02_spawnTimer > gm02_spawnRate) {

                    //set up spawn position
                    float xOffset = Random.Range (-1.85f, 1.85f);
                    float yOffset = Random.Range (-3.5f, 3.5f);
                    spawnPos = new Vector3 (transform.position.x + xOffset, transform.position.y + yOffset, 0f);
                    int planetSelector = Random.Range(0, planetPrefabs.Length); 

                    if(planetList.Count != 0) {
                         foreach (Transform spawnedPlanet in planetList)
                        {
                            while((spawnPos - spawnedPlanet.position).magnitude < (planetSizeDelta * 4f))
                            return;
                        }
                    }
                       
                    Transform planet = LeanPool.Spawn(planetPrefabs[planetSelector], spawnPos, Quaternion.identity);
                    planet.localScale = planetSize;

                    planetList.Add(planet);
                    gm02_spawnTimer = 0f;  
                    }
         
            }
             if(planetList.Count == numberOfPlanet) listFull = true;
    
        }

        else if(listFull == true) { //the list has been populated correctly and is getting empty as the player finds plantets
           
            //start timer
            gm02_timer += Time.deltaTime;
            OnTimerChanged?.Invoke(eventDuration - gm02_timer); 
            
            if(planetList.Count != 0) {
                //get the first planet of the list and activate its collider
               
                planetToBeFoundNow = planetList[0];
                planetToBeFoundNow.gameObject.GetComponent<CircleCollider2D>().enabled = true;

                planetController = planetToBeFoundNow.GetComponent<PlanetController>();
                planetController.OnColliderActivated += PlanetController_OnColliderActivated;
            }
            if(planetList.Count == 0 || gm02_timer > eventDuration) { //end of the game

                OnTimerOver?.Invoke(eventDuration - gm02_timer); 
                
                timerCleanUp += Time.deltaTime;

                GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");

                foreach (GameObject item in planets) {

                        if(timerCleanUp > .15f) {
                            item.GetComponent<PlanetController>().ExplodeAndDespawn();
                            timerCleanUp = 0f;
                        }
                        
                }

                if(planets.Length == 0) {
                    planetList.Clear();
                    gm02_timer = 0;
                    listFull = false;
                    OnMinigameOver?.Invoke(this, EventArgs.Empty);
                }
                
            
            }
        }
             

    }

    private void PlanetController_OnColliderActivated(object sender, EventArgs e) {
        //remove the planet we just found from the list
        planetList.Remove(planetToBeFoundNow);
        planetController.OnColliderActivated -= PlanetController_OnColliderActivated;
    }
        
    

    //  private Vector3 GetAsteroidSpawnPos() {


    //      float posX = Random.Range(-5f, 5f);
    //      float posY = 0;
    //      float randomYIndex = 0;
        
    //      if(posX <= -4f || posX >= 4f) {
    //          randomYIndex = Random.Range(3f, 12f);
    //          posY = PlayerController2.Instance.player.transform.position.y + randomYIndex;
    //      }
    //      else if(posX > -4f && posX < 4f) {

          
    //         // float[] yPosArray = new float[] {-3, 12};
    //         // randomYIndex = yPosArray[Random.Range(0, yPosArray.Length)]; //Random.Range(yPosArray[0], yPosArray[1]);
             

    //          posY = PlayerController2.Instance.player.transform.position.y + 12f;
            
    //      }

    //      return new Vector3(posX, posY, 0);
    //  }

    // private Vector3 GetAsteroidTargetPointPos() {

    //     float posX = 0;
    //     float posY = 0;

    //     if(transform.position.y == PlayerController2.Instance.player.transform.position.y + 12f) {
    //          posY =  PlayerController2.Instance.player.transform.position.y - 1f;
    //     }
         

    //     else if(transform.position.y < PlayerController2.Instance.player.transform.position.y + 12f) {
    //         float Yindex = Random.Range(-1f, 7f);
    //         posY =  PlayerController2.Instance.player.transform.position.y + Yindex;
    //     }
    //       posX = transform.position.x;

    //     return new Vector3(-posX, posY, 0);
    //  }


     public Transform FindPreviousPlanet() {

             GameObject[] candidates = GameObject.FindGameObjectsWithTag("Planet");
             float minDistance = Mathf.Infinity;
             Transform closest;
         
             if ( candidates.Length == 0 )
                 return null;
            
             closest = candidates[0].transform;
             for ( int i = 0 ; i < candidates.Length ; ++i )
             {  
                if(candidates[i].transform.position.y < posCurrentPlanet.y)
                  {

                    float distance = (candidates[i].transform.position - transform.position).sqrMagnitude;
 
                    if ( distance < minDistance )
                    {
                        closest = candidates[i].transform;
                        minDistance = distance;
                    }

                  }
                  
                 
             }    
             return closest;
         }

    void SpawnStarsBetween2Planets(Vector3 pos1, Vector3 pos2) {
                    
            if(pos2 != Vector3.zero && pos2 != pos1) {
                Vector3 middlePos = Vector3.Lerp(pos1, pos2, 0.5f);
                Vector3 upperPos =  Vector3.Lerp(pos1, pos2, 0.65f);
                Vector3 lowerPos =  Vector3.Lerp(pos1, pos2, 0.35f);

                float starSpawnIndex = Random.Range(0f, 100f);

                if(starSpawnIndex <= 15f) {
                    starGenerator.Spawn3Stars(middlePos, lowerPos, upperPos);
                } 
                else if(starSpawnIndex > 15f && starSpawnIndex <= 45f) {
                    starGenerator.Spawn2Stars(lowerPos, upperPos);
                } 
                else if(starSpawnIndex > 45f) {
                    starGenerator.Spawn1Star(middlePos);
                } 

            }

    }
 
}
