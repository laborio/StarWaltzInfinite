using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using System;
using Random=UnityEngine.Random;

public class BulletController : MonoBehaviour
{
    private float timer;
    public bool canMove = false;
    public GameObject lightSaber_particle;
    PlayerController_GM03 playerController_GM03;
  

    void Awake() {
        if(PlayerController2.Instance.playState == PlayState.GAMEMODE3) {
            playerController_GM03 = FindObjectOfType<PlayerController_GM03>();
        }
    }
    void OnEnable()
    {
        canMove = false;

       
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;

        switch (PlayerController2.Instance.playState)
        {
            case PlayState.GAMEMODE1:
            Handle_GM01_Update();
            break;

            case PlayState.GAMEMODE3:
            Handle_GM03_Update();
            break;
        }

        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("rock"))
        LeanPool.Despawn(gameObject);

        else if (other.CompareTag("Planet")) {
           // particles.SetActive(false);
            gameObject.transform.SetParent(other.gameObject.transform);
            canMove = false;
            ScreenShakeController.instance.StartShake(.07f, .07f);
        }

        else if (other.CompareTag("Bullet")) {
          GameObject obj1 = LeanPool.Spawn(lightSaber_particle, transform.position, Quaternion.identity);
          GameObject obj2 = LeanPool.Spawn(StoreAssets.i.pf_shockwave, transform.position, Quaternion.identity);
          ScreenShakeController.instance.StartShake(.07f, .07f);

                LeanPool.Despawn(obj1, 2);
                LeanPool.Despawn(obj2, 2);
                LeanPool.Despawn(gameObject);
                
        }

        //for MG_03 : stars spawning around the planet
        StarController starController = other.GetComponent<StarController>();
        if(starController != null)
        ScoreManager.Instance.AddMiniGameXpCount(Random.Range(3,6));
        
    }

    void Handle_GM01_Update() {
        // transform.position += Vector3.up * 10f * Time.deltaTime;
       transform.position = transform.position + transform.up * 10f * Time.deltaTime;

       LeanPool.Despawn(gameObject, 3f);
    }
    void Handle_GM03_Update() {

        if(canMove == true) {
            transform.position = transform.position + transform.up * 15f * Time.deltaTime;
        }
        


        else
        return;
    }

   
}
