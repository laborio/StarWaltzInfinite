using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Lean.Pool;

public class PlanetController : MonoBehaviour
{
    public event EventHandler OnColliderActivated;
    Light2D objectLight;
    new CircleCollider2D collider;
    CircleCollider2D centerCollider;
    public new GameObject light;
    private GameObject center;
    public new GameObject animation;
     private GameObject planetDestructionPoint;
    LevelingSystem levelingSystem;
    LevelGenerator levelGenerator;
    //for GM2
    private float timerLight;
    private float localtime;
	private bool canGlow = true;
	private float glowTime = 1f; //must be the timer between each spawn *2
    SpriteRenderer spriteRenderer;
    public GameObject planet_explosion_pieces;
    public GameObject planet_explosion_firework;
    new private ParticleSystem particleSystem;
    [SerializeField] GameObject star;
    public int unitValue = 1;
    public int xpValue;
    private int additionalXpValue;
    private int additionalStarValue;
    public int starValue;
    private float timerStar; 
   
   
    void Awake()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
       
        light = this.gameObject.transform.GetChild(0).gameObject;
        center = this.gameObject.transform.GetChild(2).gameObject;
        animation = this.gameObject.transform.GetChild(1).gameObject;
        planetDestructionPoint = GameObject.FindWithTag("DestructionPoint");

        collider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        centerCollider = center.GetComponent<CircleCollider2D>();
        objectLight = light.GetComponent<Light2D>();

        
    }

    void OnEnable() {
        
         switch (PlayerController2.Instance.playState)  {
            case PlayState.REGULAR:
            InitGM_0();
            break;
            
            case PlayState.GAMEMODE2:
            InitGM_2();
            break;
       }
       
    }
   

    void Update()
    {
        if (transform.position.y < planetDestructionPoint.transform.position.y) {
            gameObject.SetActive(false);
        }

         switch (PlayerController2.Instance.playState)  {
            case PlayState.REGULAR:
            
            break;
            
            case PlayState.GAMEMODE2:
            GM02_Behaviour();
            break;
       }
    }

  
   
    

    void InitGM_0() {
        
        transform.gameObject.tag = "Planet"; 
        animation.SetActive(false);
    	objectLight.intensity = 0f;
        collider.enabled = true;
        centerCollider.enabled = true;
     

        //to have an efficient orbit speed control when checking the ray cast
         if(LevelingSystem.Instance.GetLevelNumber() < 3) {
            gameObject.layer = 2;
        }
        else {
            gameObject.layer = 6;
        }

    }
     void InitGM_2() {
        // levelGenerator.OnTimerOver += LevelGenerator_OnTimerOver;

        if(LevelingSystem.Instance.GetLevelNumber() < 10)
        xpValue = LevelingSystem.Instance.GetLevelNumber() * 30;

        else
        xpValue = LevelingSystem.Instance.GetLevelNumber() * 20;

        starValue = LevelingSystem.Instance.GetLevelNumber() * 2; 

        transform.gameObject.tag = "Planet"; 
        animation.SetActive(false);
    	objectLight.intensity = 0f;
        collider.enabled = false; //is reactivated in levelgenerator mg_02 when player has to find the planet
        centerCollider.enabled = false;
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        canGlow = true;
        localtime = 0f;
        timerLight = 0f;

    }
    
    void GM02_Behaviour() { //handles glowing when spawned disable sprite renderer
        localtime += Time.deltaTime;
    	timerLight += Time.deltaTime;

        Color tmp = spriteRenderer.color;
        tmp.a = 1f;

        if(canGlow == true) {
    		if(timerLight < glowTime/2)
    		objectLight.intensity += .08f;
    		
    		if(timerLight >= glowTime/2) 
    		objectLight.intensity -= .08f;
    	
    		if(objectLight.intensity < 0)
    		objectLight.intensity = 0;

    		if(timerLight > glowTime) {
                canGlow = false;
               spriteRenderer.color -= tmp;
               // spriteRenderer.enabled = !spriteRenderer.enabled;
            }
    		

        }
       
    }

    // private void LevelGenerator_OnTimerOver(float remainingTime) {  //moved to scoremanager
    
    //        additionalXpValue = Mathf.RoundToInt(((float)LevelingSystem.Instance.GetLevelNumber() * remainingTime));
    //        ScoreManager.Instance.AddMiniGameXpCount(additionalXpValue);
           
    //        additionalStarValue = Mathf.RoundToInt(((float)LevelingSystem.Instance.GetLevelNumber() * remainingTime) / 3f);
    //        ScoreManager.Instance.AddMiniGameStarCount(additionalStarValue);

    //     Debug.Log("unsub event");
    //      levelGenerator.OnTimerOver -= LevelGenerator_OnTimerOver;
    
    // }

    public void ExplodeAndDespawn() {

       
        LeanPool.Spawn(star, transform.position, transform.rotation);
               
        GameObject sw = LeanPool.Spawn(planet_explosion_firework, transform.position, Quaternion.identity);
        LeanPool.Despawn(sw, 2);

        GameObject imp = LeanPool.Spawn(planet_explosion_pieces, transform.position, transform.rotation);
        LeanPool.Despawn(imp, 2);
        particleSystem = imp.GetComponent<ParticleSystem>();
        var main = particleSystem.main;

          switch (gameObject.name)
        {
            case "planete blue":
            main.startColor = Color.blue;
            break;
            case "planete green":
            main.startColor = Color.green;
            break;
            case "planete red":
            main.startColor = Color.red;
            break;
            case "planete yellow":
            main.startColor = Color.yellow;
            break;
        }

        LeanPool.Despawn(gameObject);
    }

     void OnTriggerEnter2D(Collider2D other) { 

         
        if(other.CompareTag("Player")) {
            //any game mode
            animation.SetActive(true);
            objectLight.intensity = 1.5f;
            collider.enabled = !collider.enabled;
            centerCollider.enabled = !centerCollider.enabled;

            
            
            if(PlayerController2.Instance.playState == PlayState.GAMEMODE2)  {

            Debug.Log(gameObject.name);
            spriteRenderer.color = new Color32(255, 255, 255, 255);
            ScoreManager.Instance.AddUnitCount(unitValue);
            ScoreManager.Instance.AddMiniGameStarCount(starValue);
            ScoreManager.Instance.AddMiniGameXpCount(xpValue);
            //Debug.Log(ScoreManager.Instance.GetMiniGameXpCount());

            OnColliderActivated?.Invoke(this, EventArgs.Empty);
           
            }
       
        }
    }
    


}
