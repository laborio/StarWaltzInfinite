using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using Random=UnityEngine.Random;

public class PlayerController_GM03 : PlayerController_GM
{
    public event Action<int> OnThrowCountChanged;
    [Header("Settings")]
    public float flyingSpeed = 10f;
    private GameObject planetTargetRef;
    private GameObject lightSaber;
    private int numberOfThrow;
    private int throwCounter;
    private float timerCounter;
    private float timerThrow;
    Planet_GM03 planet_GM03;
    BulletController bulletController;
    private List<GameObject> saberList;

    private float startPosX;

    protected override void OnEnable() {
        
        planetTargetRef = LeanPool.Spawn(StoreAssets.i.pf_planetTargets_GM3[Random.Range(0,StoreAssets.i.pf_planetTargets_GM3.Length)],
                          new Vector3(0, transform.position.y + 6f, transform.position.z), Quaternion.identity);

        
        planet_GM03 = planetTargetRef.GetComponent<Planet_GM03>();
        // planet_GM03.OnGM03Finished += PlanetGM03_OnGM03Finished;

        numberOfThrow = GetNumberOfThrow();
        throwCounter = 0;
        timerCounter = 0f;
        timerThrow = 0f;
        startPosX = 2f;

        saberList = new List<GameObject>();
       
        base.OnEnable();
    }
    protected override void OnDisable() {
     
        //for each arrow in collection destroy

        GameObject[] lightsabers = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in lightsabers)
        {
            
            LeanPool.Despawn(bullet);
        }
        if(planetTargetRef != null)
        LeanPool.Despawn(planetTargetRef);

           base.OnDisable();
    }

    private void Update() {
        timerThrow += Time.deltaTime;
    }


    protected override void HandleTutorialMovement() {
        
        if(playerInPosition == false)
         SpawnListObjects();

         playerInPosition = true;
         base.HandleTutorialMovement();
    }

     protected override void HandleStartingMovement() {
         transform.rotation = Quaternion.identity;

        if(throwCounter < numberOfThrow) {
            if(InputManager.getTouch(TouchPhase.Began)) {
                 if(timerThrow > 0.2f) 
                     FireArrow(); 
                     timerThrow = 0f;       
                } 
        }
        else if(throwCounter >= numberOfThrow) {
            timerCounter += Time.deltaTime;

         if(timerCounter >= 1f)
         UpdateGMState(GM_State.COUNTING);   
        }
           
    }

    private void SpawnListObjects() {

        for (int i = 0; i < numberOfThrow; i++)
        {
            GameObject lightSaber2 = LeanPool.Spawn(StoreAssets.i.pf_lightSabers[Random.Range(0,StoreAssets.i.pf_planetTargets_GM3.Length)]);
            Vector3 pos = new Vector3 (startPosX - 2f, transform.position.y + 1f, transform.position.z);
            lightSaber2.transform.position = pos;
            startPosX = pos.x;
            LeanTween.scale(lightSaber2, new Vector3(0.7f, 0.7f, 0f), .8f).setEase( LeanTweenType.easeOutQuad );
            saberList.Add(lightSaber2);
        }
    }

    private void FireArrow() {
         //fire it
         saberList[0].GetComponent<BulletController>().canMove = true;
         saberList.Remove(saberList[0]);

         if(saberList.Count != 0) {
              foreach (GameObject saber in saberList)
            {
                LeanTween.moveX(saber, saber.transform.position.x + 2f, .2f).setEase( LeanTweenType.easeOutQuad );
            }
         }
        

         throwCounter++;
         OnThrowCountChanged?.Invoke(throwCounter); //for UI update

    }

     public int GetNumberOfThrow() {

        if(LevelingSystem.Instance.GetLevelNumber() <= 5)
        return 5;

        else if(LevelingSystem.Instance.GetLevelNumber() > 5 && LevelingSystem.Instance.GetLevelNumber() < 10)
        return LevelingSystem.Instance.GetLevelNumber();

        else if(LevelingSystem.Instance.GetLevelNumber() >= 10 && LevelingSystem.Instance.GetLevelNumber() < 20)
        return 10;

        else 
        return 12;
    }

    public int GetThrowCounter() {
        return throwCounter;
    }
}
