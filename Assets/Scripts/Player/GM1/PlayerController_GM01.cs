using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class PlayerController_GM01 : PlayerController_GM
{
   // public event EventHandler OnAsteroidTouched;
   [Header("Settings")]
    public float rotateSpeed = 5f;
    public float flyingSpeed = 6f;
    public float fireRate;
    private float shootTimer;

    [Header("Prefabs")]
    public GameObject bulletPrefab;
    private Transform cannonAxis;
    private Transform cannonTip;
    private GameObject cannonRef;

    protected override void OnEnable() {
        cannonRef = LeanPool.Spawn(StoreAssets.i.pf_cannon_GM1, new Vector3(0, transform.position.y +5f, transform.position.z), Quaternion.identity);
        cannonAxis = GameObject.Find("cannonAxis").transform;
        cannonTip = GameObject.Find("cannonTip").transform;

        base.OnEnable();
    }
    protected override void OnDisable() {
        base.OnDisable();
        Destroy(cannonRef);
    }

    protected override void FixedUpdate() {
        shootTimer += Time.deltaTime;
        base.FixedUpdate();
        
    }

    protected override void HandleTutorialMovement() {
        
        transform.position = Vector3.MoveTowards(transform.position, cannonAxis.position, flyingSpeed * Time.deltaTime);
        
        //bool goes to true so the event can be send from parent class
        if(transform.position == cannonAxis.position)
        playerInPosition = true;

        base.HandleTutorialMovement();
    }

    protected override void HandleStartingMovement() {
        if(InputManager.getTouch(TouchPhase.Moved)) {
           rotateCannon();
           RayCastMousePos();
       }

    //    if(shootTimer > GetFireRate()) {
    //        if(InputManager.getTouch(TouchPhase.Moved)) {

    //            //raycast mouse pos
    //           // RayCastMousePos();

    //        //fireBullet();
    //        shootTimer = 0f;
    //       }
    //    }
       
    }

    
    void rotateCannon() {
        //get mouse position
         Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         mouseWorldPosition.z = 0f;

        //get angle
        Vector3 dir = mouseWorldPosition - cannonAxis.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        cannonAxis.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));

    }


    void fireBullet() {

        // Spawn the bullet
        GameObject bullet = LeanPool.Spawn(PlayerController2.Instance.playerPrefabList[5]);
        bullet.transform.position = cannonTip.position;
        bullet.transform.rotation = cannonTip.rotation;

        //play muzzle fx 
        GameObject obj = LeanPool.Spawn(PlayerController2.Instance.playerPrefabList[4], cannonTip.position, cannonTip.rotation);
        LeanPool.Despawn(obj, 2);  

    }

    private float GetFireRate() {
        if(LevelingSystem.Instance.GetLevelNumber() <= 5) {
            return 0.5f/Mathf.Log((float) LevelingSystem.Instance.GetLevelNumber(), 3);
        }
        else if(LevelingSystem.Instance.GetLevelNumber() > 5 && LevelingSystem.Instance.GetLevelNumber() < 10) {
            return 0.4f/Mathf.Log((float) LevelingSystem.Instance.GetLevelNumber(), 3);
        }
        else /*if(LevelingSystem.Instance.GetLevelNumber() > 10 && LevelingSystem.Instance.GetLevelNumber() <= 15) */{
            return 0.25f;
        }
    }

    private void RayCastMousePos() {

       Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

         RaycastHit2D hitInfo = Physics2D.Raycast(mousePos, Vector3.forward, 10f);
         //Debug.DrawLine(transform.position,-transform.up, Color.green);
           
                    if (hitInfo.collider != null) {
                        if(hitInfo.transform.gameObject.tag == "rock" || hitInfo.transform.gameObject.tag == "Planet") {
                            hitInfo.transform.gameObject.GetComponent<AsteroidController>().OnAsteroidTouched();
                        }
                            //LeanPool.Despawn(hitInfo.transform.gameObject);
                           // OnAsteroidTouched?.Invoke(this, EventArgs.Empty);
                                       
                    }
    }

}
