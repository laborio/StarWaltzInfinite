using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public enum RegularState {WAITINGFORTUTORIAL, BOOST, STARTING, ROTATING, FLYING, AUTOPILOT, EXIT, INACTIVE }

public class PlayerController_GM0 : MonoBehaviour
{
    public RegularState regularState;  //les etats de la regularStatemachine du mode REGULAR only
    
    [Header("Objects References")]
    CircleCollider2D playerCollider;
    PlayerController2 playerController2;
    PlayerCollisions_GM0 playerCollisions_GM0;
    GameManager gameManager;
    LevelingSystem levelingSystem;
    GameObject destructionPoint;
    MenuManager menuManager;

    [Header("Player Targets")]
    public Transform currentTarget;
    public Transform nextTarget;
    [SerializeField] private float distanceToTarget;

    [Header("Movement")]
    public float orbiteDistance;
    private float rotationSpeed;
    [SerializeField]private float fastRotationSpeed;
    [SerializeField]private float slowRotationSpeed;
    public float flyingSpeed = 6f;
    [SerializeField] private float startingMovementSpeed;
    private Vector3 moveDirection;
    private float angle; 
    private float timerJump;
    public float bounceWindow = 0.3f;
    private float timerExit;
    private Vector3 resetPos;
    private int lastSelectedGame;
    //private float flyingTimer;
    private float z;
    Vector3 currentEulerAngles;
    Quaternion currentRotation;
    
    [Header("Collision")]
    public bool collidingWithPlanet = false; // is handled on collisions in the CollisionController class
    public bool collidingWithBonus = false;
    private LayerMask nextPlanetMask;
    public Vector3 impactPosition;
    private Vector3 forecastImpactPos;
    Rigidbody2D rb;
    


    void Awake() {

        playerCollisions_GM0 = GetComponent<PlayerCollisions_GM0>();
        playerCollider = GetComponent<CircleCollider2D>();
        menuManager = FindObjectOfType<MenuManager>();
        nextPlanetMask = LayerMask.GetMask("NextPlanet");
        destructionPoint = GameObject.FindWithTag("DestructionPoint");
        rb = GetComponent<Rigidbody2D>();
        
    }

    void OnEnable() {

       
        
        LevelingSystem.Instance.OnMiniGameLevelChanged += levelingSystem_OnMiniGameLevelChanged;

        regularState = RegularState.WAITINGFORTUTORIAL;
        GetTargetsData();
        collidingWithBonus = false;
        collidingWithPlanet = false;
    }

    void OnDisable() {
        LevelingSystem.Instance.OnMiniGameLevelChanged -= levelingSystem_OnMiniGameLevelChanged;
        //flyingTimer = 0f; 
        

    }

    void levelingSystem_OnMiniGameLevelChanged(object sender, System.EventArgs e) { //called in levelingsystem when the current lvl is a minigame lvl
        regularState = RegularState.EXIT;
    }

    void Start() {

        GetTargetsData();
        regularState = RegularState.WAITINGFORTUTORIAL;
        
    }

    
    void FixedUpdate() {

       // Debug.Log(MouseOverUI.IsPointerOverUIObject());
       GetDistanceToTarget();
       
         if(GameManager.Instance.State == GameState.PLAYMODE) {

            switch(regularState) {
                case RegularState.WAITINGFORTUTORIAL:
                HandleTutorialMovement();
                break;
                case RegularState.BOOST:
                HandleBoostMovement();
                break;
                case RegularState.STARTING:
                HandleStartingMovement();
                break;
                case RegularState.ROTATING:
                HandleRotatingMovement();
                break;
                case RegularState.FLYING:
                HandleFlyingMovement();
                break;
                case RegularState.AUTOPILOT:
                HandleAutopilotMovement();
                break;
                case RegularState.EXIT:
                HandleExitMovement();
                break;
                case RegularState.INACTIVE:
                break;

            }
         }    
        
    }
    //  void FixedUpdate() {
       
    //      if(GameManager.Instance.State == GameState.PLAYMODE) {

    //         switch(regularState) {
               
    //             case RegularState.ROTATING:
    //             HandleRotatingMovement();
    //             break;
    //             case RegularState.FLYING:
    //             HandleFlyingMovement();
    //             break;
    //             case RegularState.AUTOPILOT:
    //             HandleAutopilotMovement();
    //             break;
                
    //         }
    //      }    
        
    // }

    
    public Transform FindCurrentTarget() {
             GameObject[] candidates = GameObject.FindGameObjectsWithTag("Planet");
             float minDistance = Mathf.Infinity;
             Transform closest;
         
             if ( candidates.Length == 0 )
                 return transform;
            
             closest = candidates[0].transform;
             for ( int i = 0 ; i < candidates.Length ; ++i )
             {  
                
                    float distance = (candidates[i].transform.position - transform.position).sqrMagnitude;
 
                    if ( distance < minDistance )
                    {
                        closest = candidates[i].transform;
                        minDistance = distance;
                    }

                  
                 
             }    
             return closest;
         } 

    public Transform FindNextTarget() {

             GameObject[] candidates = GameObject.FindGameObjectsWithTag("Planet");
             float minDistance = Mathf.Infinity;
             Transform closest;
         
             if ( candidates.Length == 0 )
                 return null;
            
             closest = candidates[0].transform;
             for ( int i = 0 ; i < candidates.Length ; ++i )
             {  
                if(candidates[i].transform.position.y > currentTarget.position.y)
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


    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
         //return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg ;
      }

    void GetTargetsData() {

        currentTarget = FindCurrentTarget();
        nextTarget = FindNextTarget();

        if(currentTarget != null)
        angle = AngleBetweenTwoPoints(currentTarget.position, transform.position);
    }

    void GetDistanceToTarget() {
        if(nextTarget != null)
        distanceToTarget = Vector3.Distance(nextTarget.position, transform.position);
    }


    void RegulateOrbitSpeed() {   

         //align player rotation with orbit path     
        // float zRot = transform.eulerAngles.z;
       //Vector2 moveDirection2 = (transform.position - currentTarget.position);
        transform.rotation =  Quaternion.Euler (new Vector3(0f,0f,angle + 90)); 
    
         RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -transform.up, (distanceToTarget + 1f), nextPlanetMask);
         //Debug.DrawLine(transform.position,-transform.up, Color.green);
           
                    if (hitInfo.collider != null) {
                        
                        //Debug.Log("object: " + hitInfo.transform.gameObject + "," + "pos: " + hitInfo.point);
                        rotationSpeed = GetSlowRotationSpeed();  
                        forecastImpactPos = hitInfo.point;
                                       
                    }

                    else {
                        rotationSpeed = GetFastRotationSpeed();
                    }
                 

    }

    float GetSlowRotationSpeed() {
        if(LevelingSystem.Instance.GetLevelNumber() < 3) {
            return 0;
        }
        else {
            return LevelingSystem.Instance.GetLevelNumber() * 10;
        }

    }

    float GetFastRotationSpeed() {
         if(LevelingSystem.Instance.GetLevelNumber() < 10) {
            return 200;
        }
        else {
            return (LevelingSystem.Instance.GetLevelNumber() * 2) + 200;
        }
    }

     float GetFlyingSpeed() {
         if(LevelingSystem.Instance.GetLevelNumber() < 10) {
            return 6;
        }
        else {
            return Mathf.Log((float) LevelingSystem.Instance.GetLevelNumber(), 3) + 5f; //(LevelingSystem.Instance.GetLevelNumber() / 2) + 1;
           
        }
    }

    // public Vector3 GetResetPos() {
    //     return resetPos;
    // }

     void HandleTutorialMovement() {

         //nextTarget = FindNextTarget();
         GetTargetsData();
         currentTarget = GameObject.FindWithTag("StartPlanet").transform;
        
        if(PlayerPrefs.GetInt(PlayerPrefKeys.kTutorialStep) != 0) {
            
            if(PlayerPrefs.GetInt(PlayerPrefKeys.kBoostCount) != 0 && LevelingSystem.Instance.GetLevelNumber() == 1) {
                menuManager.boostPanel.SetActive(true);
                RegulateOrbitSpeed();
                transform.RotateAround(currentTarget.position, -Vector3.forward, rotationSpeed * Time.deltaTime);
            }
            

            else
            regularState = RegularState.STARTING; 
        }  

        else {
            RegulateOrbitSpeed();
            transform.RotateAround(currentTarget.position, -Vector3.forward, rotationSpeed * Time.deltaTime);

        }
        

    }

    void HandleBoostMovement() {
        //transform.position += Vector3.up * 12f * Time.deltaTime;
        
        Vector3 targetPos = new Vector3(0f, 10f, 0f);
       // transform.position = Vector3.Lerp(transform.position, targetPos, 3f * Time.deltaTime);
       transform.position += Vector3.up * 13f * Time.deltaTime;
       
        if(LevelSystemAnimated.Instance.GetLevelNumber() >= 15){
                     
            regularState = RegularState.STARTING;
            GetTargetsData();
            
        }

        
    }

    void HandleStartingMovement() {
        
       
       
       nextTarget = FindNextTarget();
       currentTarget = this.transform;

       float angle = AngleBetweenTwoPoints(currentTarget.position, transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0,0, angle + 90f));

        timerExit = 0f; 

         if(nextTarget != null)
                transform.position = Vector3.MoveTowards(transform.position, nextTarget.position, GetFlyingSpeed() * Time.deltaTime);
                //Debug.Break();
                if (collidingWithPlanet == true) {
                    regularState = RegularState.ROTATING;
                }

    }

    void HandleRotatingMovement() {

         GetTargetsData(); 
         RegulateOrbitSpeed();

         timerJump += Time.fixedDeltaTime; //set up timer qui permet de jump direct a la next planete
         collidingWithPlanet = false; // turns to true in PlayerCollision class

        //set le sens de rotation
        if(impactPosition.x < currentTarget.transform.position.x) 
        transform.RotateAround(currentTarget.position, -Vector3.forward, rotationSpeed * Time.fixedDeltaTime);
        else if (impactPosition.x >= currentTarget.transform.position.x)
        transform.RotateAround(currentTarget.position, Vector3.forward, rotationSpeed * Time.fixedDeltaTime);


        //check si le player tape au bon moment pour activer le bounce
        if (timerJump <= bounceWindow) {

            if (InputManager.getTouch(TouchPhase.Began)) {
                    LevelingSystem.Instance.jumpCount ++; //is used to multiply XP in leveling system
                    regularState = RegularState.AUTOPILOT;
                }
            
        }
        else if (timerJump > bounceWindow) {

            LevelingSystem.Instance.jumpCount = 1;

            if(timerJump > bounceWindow + .25f) { //the time window that prevents the player to jump away immediatlely after failing the bounce
                
                if (InputManager.getTouch(TouchPhase.Began)) {
                    moveDirection = (currentTarget.position - transform.position);//.normalized;
                    timerJump = 0f;

                    //resetPos = transform.position; //save this pos for respawn function
                    
                    regularState = RegularState.FLYING;
                }
            } 
            
        }

    }

    void HandleFlyingMovement() {
  
    //transform.position -= moveDirection * GetFlyingSpeed() * Time.deltaTime;
    transform.Translate(-Vector3.up * GetFlyingSpeed() * Time.deltaTime);
     //transform.position += -transform.up * GetFlyingSpeed() * Time.deltaTime;
   // rb.velocity = -moveDirection * GetFlyingSpeed();// * Time.deltaTime;
   // rb.MovePosition(transform.position - (moveDirection.normalized * GetFlyingSpeed() * Time.fixedDeltaTime));


    //trigger game over state
     if(transform.position.x <= -3.8f || transform.position.x >= 3.8f || transform.position.y < destructionPoint.transform.position.y) {
         GameManager.Instance.UpdateGameState(GameState.LOST);
         regularState = RegularState.INACTIVE;

         Analytics.CustomEvent("Player Died" + LevelingSystem.Instance.GetLevelNumber());
     }
    

      if (collidingWithPlanet == true) {
        //    flyingTimer = 0f;                   
            transform.position = impactPosition; // this will determine rotate around direction clockwise or not 
            regularState = RegularState.ROTATING;

        } 
      else if(collidingWithBonus == true) {
          //flyingTimer = 0f; 
          regularState = RegularState.AUTOPILOT;

      }

    }

    void HandleAutopilotMovement() {

        float angle = AngleBetweenTwoPoints(currentTarget.position, transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0,0, angle + 90f));

        if (collidingWithPlanet == true) { //only true when collidingWithPlanet with planets, not satellites
        
            transform.position = impactPosition; // this will determine rotate around direction clockwise or not 
            regularState = RegularState.ROTATING;
        }
        else if (collidingWithBonus == true) { //only true when collidingWithPlanet with planets, not satellites
        
            currentTarget = FindCurrentTarget(); //refresh current target in case we skipped a lot of planets at once before entering autopilot mode
        }
        collidingWithBonus = false;
        nextTarget = FindNextTarget();  //refresh next target
        transform.position = Vector3.MoveTowards(transform.position, nextTarget.position, GetFlyingSpeed() * Time.deltaTime);

    }

    void HandleExitMovement() {


        timerExit += Time.deltaTime; //is reset to 0 in starting movement, when reentering the gm0 state from another gm

        if(timerExit <= 1.5f) {
            //move up and get to center of screen
            transform.position += Vector3.up * 10f * Time.deltaTime;
            if(transform.position.x <= 0)
            transform.position = new Vector3(transform.position.x + 0.01f, transform.position.y, transform.position.z);
            if(transform.position.x > 0)
            transform.position = new Vector3(transform.position.x - 0.01f, transform.position.y, transform.position.z);
        }
        else {
            
            PickMiniGame();
            //PlayerController2.Instance.UpdatePlayState(PlayState.GAMEMODE1);
        }
       
    }

    private void PickMiniGame() {
        
        int gameSelector = Random.Range(1, 5);

        if(gameSelector == lastSelectedGame) return;

        else {
            switch (gameSelector)
            {
                case 1:
                PlayerController2.Instance.UpdatePlayState(PlayState.GAMEMODE1);
                break;
                case 2:
                PlayerController2.Instance.UpdatePlayState(PlayState.GAMEMODE2);
                break;
                case 3:
                PlayerController2.Instance.UpdatePlayState(PlayState.GAMEMODE3);
                break;
                case 4:
                PlayerController2.Instance.UpdatePlayState(PlayState.GAMEMODE4);
                break;
            }
            lastSelectedGame = gameSelector;
        }
    }




    bool isBetween (float number, float min, float max) {
        if(number > min && number <= max) {
            return true;
        }
        else {
            return false;
        }
    }

}
