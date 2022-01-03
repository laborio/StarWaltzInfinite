using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCollisions_GM0 : MonoBehaviour
{
   
    [SerializeField] private GameObject xpAnimationEffect;
    GameObject worldSpaceCanvas;

    private LevelingSystem levelingSystem;
    private PlayerController2 playerController;
    PlayerController_GM0 playerController_GM0;
    ScoreManager scoreManager;
    PlayerFX playerFX;
       

    void Awake() {

        levelingSystem = FindObjectOfType<LevelingSystem>();
        worldSpaceCanvas = GameObject.FindWithTag("WorldCanvas");

        playerController = FindObjectOfType<PlayerController2>();
        playerController_GM0 = GetComponent<PlayerController_GM0>();
        playerFX = GetComponent<PlayerFX>();
        
        
    }

   
    void Update()
    {
       
    }
   
       
    void OnTriggerEnter2D(Collider2D other) {  

        if(other.transform.GetComponent<PlanetController>() != null) 
        IfCollisionWithPlanet(other); 

        else if(other.transform.GetComponent<BonusController>() != null)              
        IfCollisionWithBonus(other);
       // IfCollisionWithPlanet(other);
        
        else if(other.transform.GetComponent<StarController>() != null)
        IfCollisionWithStar(other);
 
    }

   
   void IfCollisionWithPlanet(Collider2D other) {

        PlanetController planetController = other.GetComponent<PlanetController>();
       // if(planetController != null) {

            //get impact position for player movement 
            playerController_GM0.collidingWithPlanet = true;
            playerController_GM0.impactPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);

             // FX play the player impact particle effect 
             playerFX.PlayCollisionImpactFX();

             //play sound
             AudioManager.Instance.Play("planet_touch");

            levelingSystem.baseXP = levelingSystem.GetPlanetXp(); //necessary to display the right amount on worldspace UI. used by scoreBounce
             //send xp recieved from the collisions to levelingsystem
            levelingSystem.AddExperience(levelingSystem.XpGain(levelingSystem.baseXP));

       // }

   }

   void IfCollisionWithBonus(Collider2D other) {
        BonusController bonusController = other.GetComponent<BonusController>();
       //if(bonusController != null) {

            // FX play the player impact particle effect 
             playerFX.PlayCollisionImpactFX();

              //play sound
             AudioManager.Instance.Play("satellite_touch");

            levelingSystem.baseXP = levelingSystem.GetBonusXp();
            levelingSystem.AddExperience(levelingSystem.XpGain(levelingSystem.baseXP));

            //set autopilot state in playermovement GM0
            playerController_GM0.collidingWithBonus = true;
            //playerController_GM0.regularState = RegularState.AUTOPILOT;
     //   }
   }

   void IfCollisionWithStar(Collider2D other) {
        StarController starController = other.GetComponent<StarController>();
      //  if(starController != null) {
            AudioManager.Instance.Play("coin_touch");
            ScoreManager.Instance.AddStar(starController.starScore);
           
      //  }
   }
}
