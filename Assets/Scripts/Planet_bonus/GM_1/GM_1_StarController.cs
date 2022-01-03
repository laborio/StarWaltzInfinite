using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class GM_1_StarController : MonoBehaviour
{
    //[SerializeField] private GameObject star_ImpactParticle;
    private float timer;
    PlayerController_GM04 playerController_GM04;

    void Awake() {
    
    }

    void OnEnable() {
       
     //  LeanTween.scale(gameObject, new Vector3(0.01f, 0.01f, 0.01f), 3f).setEaseInOutSine();
     if(PlayerController2.Instance.playState == PlayState.GAMEMODE4)
      playerController_GM04 = PlayerController2.Instance.player.GetComponent<PlayerController_GM04>();
      
    }
    
    void Update() {
        timer += Time.deltaTime;

        transform.RotateAround(transform.position, Vector3.forward, 150 * Time.deltaTime);



        if(timer >= .3f) {
          if(PlayerController2.Instance.playState == PlayState.GAMEMODE4)
          transform.position = Vector3.MoveTowards(transform.position, PlayerController2.Instance.player.transform.position, ((playerController_GM04.GetCurrentSpeed() + 3f) * Time.deltaTime));

          else
          transform.position = Vector3.Lerp(transform.position, PlayerController2.Instance.player.transform.position, 2 * Time.deltaTime);
        }
        
     
    }

    
    void OnDisable() {
      // transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      timer = 0f;
    }

      void OnTriggerEnter2D(Collider2D other)
    {
     	   if (other.CompareTag("Player"))
     	   	{	
     
                LeanPool.Despawn(gameObject);
                

    		}
    }
}
