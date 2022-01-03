using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;


public class StarController : MonoBehaviour
{
    PlayerController_GM0 playerController_GM0;
    [SerializeField] private GameObject star_ImpactParticle;
    private GameObject planetDestructionPoint;
    private GameObject player;
    [SerializeField] private int rotationSpeed;
    private Vector3 scaleSize;
    [SerializeField] private float scaleDelta;
    public int starScore = 1;
    
   
    void Start()
    {
        planetDestructionPoint = GameObject.FindWithTag("DestructionPoint");
        player = GameObject.FindWithTag("Player");
        playerController_GM0 = player.GetComponent<PlayerController_GM0>();
        
        //LeanTween.moveY(gameObject, transform.position.y - 0.2f, 1f).setEaseInOutSine().setLoopPingPong();

        scaleSize = new Vector3(transform.localScale.x * scaleDelta, transform.localScale.y * scaleDelta, transform.localScale.y * scaleDelta);
        LeanTween.scale(gameObject, scaleSize, 1f).setEaseInOutSine().setLoopPingPong();
        

    }

  
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);

        // if(playerController_GM0.regularState == RegularState.AUTOPILOT) {
        //     transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 4f * Time.deltaTime);
        // }
        

         if (transform.position.y < planetDestructionPoint.transform.position.y) {
            gameObject.SetActive(false);
        }
    }

      void OnTriggerEnter2D(Collider2D other)
    {
     	   if (other.CompareTag("Player"))
     	   	{	
     	   		
                GameObject obj1 = LeanPool.Spawn(star_ImpactParticle, transform.position, Quaternion.identity);

                LeanPool.Despawn(obj1, 2);
                gameObject.SetActive(false);

    		}

            else if (other.CompareTag("Bullet")) {
                AudioManager.Instance.Play("coin_touch");   
                GameObject obj1 = LeanPool.Spawn(star_ImpactParticle, transform.position, Quaternion.identity);
                ScoreManager.Instance.AddMiniGameStarCount(LevelingSystem.Instance.GetLevelNumber() * 10);

                LeanPool.Despawn(obj1, 2);
                LeanPool.Despawn(gameObject);
            }
    }
}
