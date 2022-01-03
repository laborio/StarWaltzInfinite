using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.Rendering.Universal;
using Random=UnityEngine.Random;


public class PlayerController_GM05 : PlayerController_GM
{
    //public event EventHandler OnGalaxySpawned;
    private GameObject galaxyRef;
    private float localtime;
    Light2D light2D;
    private float speed;
    private float rotateSpeed;
    private Vector3 targetPos;
    private Vector3 spawnPos;
    private bool isMoving;
    private float galaxyYoffset;

    protected override void OnEnable() {
        
        galaxyRef = LeanPool.Spawn(StoreAssets.i.pf_galaxy,
                          new Vector3(0, transform.position.y + 8f, transform.position.z), Quaternion.identity);
        //OnGalaxySpawned?.Invoke(this, EventArgs.Empty);    
        

        rotateSpeed = .5f;
        speed = 0.4f;
        light2D = galaxyRef.GetComponent<Light2D>();
    	light2D.intensity = 1.75f;
        isMoving = false;

        
        //planet_GM03 = planetTargetRef.GetComponent<Planet_GM03>();
        // planet_GM03.OnGM03Finished += PlanetGM03_OnGM03Finished;

     
        base.OnEnable();
    }
    protected override void OnDisable() {
        base.OnDisable();
        //for each arrow in collection destroy
        LeanPool.Despawn(galaxyRef);
    }

    protected override void FixedUpdate() {
        AnimateGalaxy();
        base.FixedUpdate();
    }

    protected override void HandleTutorialMovement() {

        targetPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        
         playerInPosition = true;
         base.HandleTutorialMovement();
    }

    protected override void HandleStartingMovement() {
         transform.rotation = Quaternion.identity;
         
         if(InputManager.getTouch(TouchPhase.Began)) 
         isMoving = true;

         if(isMoving == true)
         MoveUp();
        
           
       
    }

    private void AnimateGalaxy() {
        localtime += Time.deltaTime;

        galaxyRef.transform.localScale = new Vector3(Mathf.PingPong(localtime * speed, 0.5f) + 1f, Mathf.PingPong(localtime * speed, 0.5f) + 1f, transform.localScale.z);
        galaxyRef.transform.Rotate (0,0, rotateSpeed);

        light2D.intensity = Mathf.PingPong(localtime * 2, 3.25f) + 1.75f;
    }

    private void MoveUp() {

        
            if(transform.position.y < targetPos.y - 0.3f)
            transform.position = Vector3.Lerp(transform.position, targetPos, 8f * Time.deltaTime);

            else {
                
                targetPos = new Vector3(transform.position.x, transform.position.y + 1.3f, transform.position.z);
                isMoving = false;
            }
            
        
    }

    // private void SpawnObstacles() {
    //     Debug.Break();

    //     spawnTimer += Time.deltaTime;
    //     spawnRate = Random.Range(.5f, 1.5f);
    
    //         foreach(float raw in rawCounter) {
                
    //             for (int i = 0 ; i < 5; i++) {
                    
                
                
                        
    //                         //set up spawn position
    //                         float xOffset = 6f;//Random.Range (-6f, 6f);
                            
    //                         spawnPos = new Vector3 (transform.position.x + xOffset, transform.position.y + raw, 0f);
    //                     // int planetSelector = Random.Range(0, StoreAssets.i.pf_gm4Objects.Length); 

    //                         // if(obstaclesList.Count != 0) {
    //                         //     foreach (GameObject spawnedObstacle in obstaclesList)
    //                         //     {
    //                         //         while((spawnPos - spawnedObstacle.transform.position).magnitude < 2f) {
    //                         //             xOffset = Random.Range (-6f, 6f);
    //                         //             spawnPos = new Vector3 (transform.position.x + xOffset, transform.position.y + raw, 0f);
    //                         //         }
                                    
    //                         //     }
    //                         // }

                        
    //                             GameObject obstacle = LeanPool.Spawn(StoreAssets.i.pf_gm4Objects[0], spawnPos, Quaternion.Euler(0,0,Random.Range(0, 360f)));
    //                             //obstaclesList.Add(obstacle);
                                
                            
                            
                    
                        
    //                     //  Debug.Log(obstaclesList.Count);
                    
    //             }
                
    //             //obstaclesList.Clear();
    //         }
        
        
      
    // }

    // private void SpawnObstacles2() {
    //    spawnTimer += Time.deltaTime;
    //    spawnRate = Random.Range(.5f, 1.5f);
     
    //     if(obstaclesList.Count < 5) { //spawn planets if list hasnt reach its full size 
    //          for (int i = 0 ; i < 5; i++) {
                 
    //                 if(spawnTimer > spawnRate) {

    //                 //set up spawn position
    //                 float xOffset = 6f;//Random.Range (-6f, 6f);

    //                 spawnPos = new Vector3 (transform.position.x + xOffset, transform.position.y + 1f, 0f);
                       
    //                 GameObject obstacle = LeanPool.Spawn(StoreAssets.i.pf_gm4Objects[0], spawnPos, Quaternion.Euler(0,0,Random.Range(0, 360f)));
    //                // planet.localScale = planetSize;

    //                 obstaclesList.Add(obstacle);
    //                 spawnTimer = 0f;  
    //                 }
         
    //         }
    //          //if(planetList.Count == numberOfPlanet) listFull = true;
    
    //     }
    // }
}
