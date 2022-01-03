using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class AsteroidController : MonoBehaviour
{
    LevelGenerator levelGenerator;
    PlayerController_GM01 playerController_GM01;
    private GameObject destructionPoint;
    private float flyingSpeed;
    private float rotateSpeed;
    private Vector3 moveDirection;
    private float randomSize;
    [SerializeField] private GameObject asteroid_ImpactParticle;
    [SerializeField] private GameObject shockwaveParticle;
    [SerializeField] GameObject star;
    GameObject worldSpaceCanvas;
    public int unitValue = 1;
    public int xpValue;
    public int starValue;


    void Awake() {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        worldSpaceCanvas = GameObject.FindWithTag("WorldCanvas");
    }

    void Start() {
        
     }
    void OnEnable() {
        

        destructionPoint = GameObject.FindWithTag("DestructionPoint");

        if(PlayerController2.Instance.playState == PlayState.GAMEMODE4) {
            xpValue = Mathf.RoundToInt(Mathf.Log((float) LevelingSystem.Instance.GetLevelNumber(), 3) * 9);//LevelingSystem.Instance.GetLevelNumber() * 7;
            starValue = Mathf.RoundToInt(LevelingSystem.Instance.GetLevelNumber() / 3); //Random.Range(1,3); 
        }

        else {
            // playerController_GM01 = FindObjectOfType<PlayerController_GM01>();
            // playerController_GM01.OnAsteroidTouched += GM01_OnAsteroidTouched;

            xpValue = Mathf.RoundToInt(Mathf.Log((float) LevelingSystem.Instance.GetLevelNumber(), 3) * 12);
            starValue = Mathf.RoundToInt(LevelingSystem.Instance.GetLevelNumber() / 3); //Random.Range(1,4);
        }
       

       

        // randomSize = Random.Range(0.25f, 0.45f);
        // transform.localScale = new Vector3(randomSize, randomSize, randomSize);
        
        moveDirection = levelGenerator.asteroidMoveDir;
        
        rotateSpeed = Random.Range(1,2);

        if(LevelingSystem.Instance.GetLevelNumber() >= 10)
        flyingSpeed = .20f + (0.5f / (float) LevelingSystem.Instance.GetLevelNumber());

        else flyingSpeed = .20f;

     //   LeanPool.Despawn(gameObject, 5f);
        
    }

    void OnDisable() {
        // if(PlayerController2.Instance.playState == PlayState.GAMEMODE1)
        //     playerController_GM01.OnAsteroidTouched -= GM01_OnAsteroidTouched;
    }

    void FixedUpdate() {

       

        switch (PlayerController2.Instance.playState)
        {
                        
            case PlayState.GAMEMODE4:
            transform.Rotate (0,0, rotateSpeed); 
            
            if(transform.position.y < destructionPoint.transform.position.y) {
                ScoreManager.Instance.AddUnitCount(unitValue);
                ScoreManager.Instance.AddMiniGameStarCount(starValue);
                ScoreManager.Instance.AddMiniGameXpCount(xpValue);
                LeanPool.Despawn(gameObject);
            }
            
            break;

            default:
            transform.position += moveDirection * flyingSpeed * Time.deltaTime;
            transform.Rotate (0,0, rotateSpeed);
            break;
        }

        
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        if(PlayerController2.Instance.playState == PlayState.GAMEMODE4) {
            if(other.GetComponent<PlayerController_GM04>() != null) {

                if(gameObject.name == "Bonus_Rocket") 
                FullExplosion();
            }
        }

        // else { //if gamemode 1
        //     if(other.GetComponent<BulletController>() != null) {

        //         if(gameObject.name == "Bonus_Rocket") {
                    
        //         FullExplosion();
        //         }

        //         else {

        //         SingleExplosion();
        //         }
        //     }
        // }
            
        
    }

    public void ExplodeAndDespawn() {
            GameObject sw = LeanPool.Spawn(shockwaveParticle, transform.position, Quaternion.identity);
            LeanPool.Despawn(sw, 2);

            GameObject imp = LeanPool.Spawn(asteroid_ImpactParticle, transform.position, transform.rotation);
            LeanPool.Despawn(imp, 2);

            LeanPool.Despawn(gameObject);
    }

    private void FullExplosion() {
         GameObject[] asteroids = GameObject.FindGameObjectsWithTag("rock");

                foreach (GameObject item in asteroids){
                    SpawnStarSet(starValue, item.transform.position);
                    ScoreManager.Instance.AddUnitCount(unitValue);
                    ScoreManager.Instance.AddMiniGameStarCount(starValue);
                    ScoreManager.Instance.AddMiniGameXpCount(xpValue);
                    item.GetComponent<AsteroidController>().ExplodeAndDespawn();
                    AudioManager.Instance.Play("asteroid_touch");
                }
                
                GameObject sw = LeanPool.Spawn(shockwaveParticle, transform.position, Quaternion.identity);
                LeanPool.Despawn(sw, 2);

                GameObject imp = LeanPool.Spawn(asteroid_ImpactParticle, transform.position, transform.rotation);
                LeanPool.Despawn(imp, 2);
                ScreenShakeController.instance.StartShake(.07f, .07f);
                LeanPool.Despawn(gameObject);
    }

    private void SingleExplosion() {
         GameObject sw = LeanPool.Spawn(shockwaveParticle, transform.position, Quaternion.identity);
                LeanPool.Despawn(sw, 2);

                GameObject imp = LeanPool.Spawn(asteroid_ImpactParticle, transform.position, transform.rotation);
                LeanPool.Despawn(imp, 2);

                //spawn stars 
                SpawnStarSet(starValue, transform.position);

                ScreenShakeController.instance.StartShake(.07f, .07f);

                ScoreManager.Instance.AddUnitCount(unitValue);
                ScoreManager.Instance.AddMiniGameStarCount(starValue);
                ScoreManager.Instance.AddMiniGameXpCount(xpValue);

                LeanPool.Despawn(gameObject);
    }

    //  void OnMouseEnter() {
    //     if(PlayerController2.Instance.playState == PlayState.GAMEMODE1) {
    //         if(gameObject.name == "Bonus_Rocket") {
    //             AudioManager.Instance.Play("satellite_touch");   
    //             FullExplosion();
    //             }

    //             else {
    //             AudioManager.Instance.Play("asteroid_touch");
    //             SingleExplosion();
    //             }
    //     }

      // void GM01_OnAsteroidTouched(object sender, EventArgs e) {
        public void OnAsteroidTouched() {
        
            if(gameObject.name == "Bonus_Rocket") {
                AudioManager.Instance.Play("satellite_touch");   
                FullExplosion();
                }

                else {
                AudioManager.Instance.Play("asteroid_touch");
                SingleExplosion();
                }
        }
          
            
    // }

    // private void SpawnStarSet(int randomIndex, Vector3 position) {
    //     switch (randomIndex)
    //     {
    //         case 1:
    //         LeanPool.Spawn(star, transform.position, transform.rotation);
    //         break;

    //         case 2:
    //         LeanPool.Spawn(star, new Vector3(transform.position.x - 0.15f, transform.position.y, transform.position.z), Quaternion.identity);
    //         LeanPool.Spawn(star, new Vector3(transform.position.x + 0.15f, transform.position.y, transform.position.z), Quaternion.identity);
    //         break;

    //         case 3:
    //         LeanPool.Spawn(star, new Vector3(transform.position.x - 0.15f, transform.position.y - 0.17f, transform.position.z), Quaternion.identity);
    //         LeanPool.Spawn(star, new Vector3(transform.position.x + 0.15f, transform.position.y - 0.17f, transform.position.z), Quaternion.identity);
    //         LeanPool.Spawn(star, new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z), Quaternion.identity);
    //         break;
    //     }
    // }

    private void SpawnStarSet(int randomIndex, Vector3 position) {
        switch (randomIndex)
        {
            case 1:
            LeanPool.Spawn(star, position, Quaternion.identity);
            break;

            case 2:
            LeanPool.Spawn(star, new Vector3(position.x - 0.15f, position.y, position.z), Quaternion.identity);
            LeanPool.Spawn(star, new Vector3(position.x + 0.15f, position.y, position.z), Quaternion.identity);
            break;

            case 3:
            LeanPool.Spawn(star, new Vector3(position.x - 0.15f, position.y - 0.17f, position.z), Quaternion.identity);
            LeanPool.Spawn(star, new Vector3(position.x + 0.15f, position.y - 0.17f, position.z), Quaternion.identity);
            LeanPool.Spawn(star, new Vector3(position.x, position.y + 0.15f, position.z), Quaternion.identity);
            break;
        }
    }
}
