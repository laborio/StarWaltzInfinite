using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
using Lean.Pool;


public class PlayerFX : MonoBehaviour
{
    [Header("Level Up prefabs")]
    [SerializeField] private GameObject WorldSpaceLevelUpPrefab;
    [SerializeField] private GameObject LevelUpFlashEffectPrefab;
    private GameObject lvlUpFlashEffectRef; //just the reference to animate it in update 

    [Header("Impact prefab")]
    [SerializeField] private GameObject impactParticle;

    [Header("XP prefab")]
    [SerializeField] private GameObject xpAnimationEffect;
    private GameObject xpAnimationEffectRef;
    GameObject worldSpaceCanvas;
    LevelingSystem levelingSystem;
    scoreBounce scoreBounce;
    GameObject player;

    void Awake() {
        
        worldSpaceCanvas = GameObject.FindWithTag("WorldCanvas");
    
    }

    void OnEnable() {
        LevelSystemAnimated.Instance.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
        LevelingSystem.Instance.OnExperienceChanged += LevelingSystem_OnExperienceChanged;
    }

    void OnDisable() {
        LevelSystemAnimated.Instance.OnLevelChanged -= LevelSystemAnimated_OnLevelChanged;
        LevelingSystem.Instance.OnExperienceChanged -= LevelingSystem_OnExperienceChanged;
    }
    

    private void LevelSystemAnimated_OnLevelChanged(object sender, System.EventArgs e) {

        if(LevelingSystem.Instance.GetLevelNumber() > 1) {
            levelUpPlayerAnim();
            levelUpWorldSpaceAnim();
            AudioManager.Instance.Play("levelup");
        }
           
            
    }
    private void LevelingSystem_OnExperienceChanged(object sender, System.EventArgs e) {
           
            SpawnXpAnimationTxt();
    }
  
    
    void Update() {
        
        if(lvlUpFlashEffectRef != null) {
            lvlUpFlashEffectRef.transform.position = player.transform.position;
        }


    }

     void levelUpPlayerAnim() {
        player = PlayerController2.Instance.player;
         //index 3 is the 4th row of the list = the fx lvl up row
        lvlUpFlashEffectRef = LeanPool.Spawn(PlayerController2.Instance.playerPrefabList[3], player.transform.position, Quaternion.identity);
        LeanPool.Despawn(lvlUpFlashEffectRef, 2);   

        if(this.GetComponent<PlayerController_GM0>().regularState != RegularState.BOOST)
        ScreenShakeController.instance.StartShake(.2f, .2f);

     }

     void levelUpWorldSpaceAnim() {
        player = PlayerController2.Instance.player;
        GameObject obj = LeanPool.Spawn(WorldSpaceLevelUpPrefab, new Vector3(0, player.transform.position.y +2.5f, player.transform.position.z), Quaternion.identity);
        obj.transform.SetParent(worldSpaceCanvas.transform);
        LeanPool.Despawn(obj, 3); 
                      
     }

     void SpawnXpAnimationTxt() {
        player = PlayerController2.Instance.player;
        GameObject xpAnimObj = LeanPool.Spawn(xpAnimationEffect, WorldSpaceXPposition()/* player.transform.position*/, Quaternion.identity);
       // GameObject xpAnimObj = _pool.Get();
      //  xpAnimObj.transform.position = WorldSpaceXPposition();
        xpAnimObj.transform.SetParent(worldSpaceCanvas.transform);
        
        LeanPool.Despawn(xpAnimObj, 3);                 

    }

    public void PlayCollisionImpactFX() {
        player = PlayerController2.Instance.player;
        // index 4 cuz the prefab is in the 4th row
        GameObject obj = LeanPool.Spawn(PlayerController2.Instance.playerPrefabList[4], player.transform.position, Quaternion.identity);
        LeanPool.Despawn(obj, 2);  
    }

    private Vector3 WorldSpaceXPposition() {
        
        float xPos;
        if(transform.position.x > 0) {
            xPos = Random.Range(transform.position.x - 1f, transform.position.x - 1.3f);
        }
        else {
            xPos = Random.Range(transform.position.x + 1f, transform.position.x + 1.3f);
        }

        float yPos = Random.Range(transform.position.y +0.5f, transform.position.y + 1f);
        
        Vector3 pos = new Vector3(xPos, yPos, transform.position.z);
        return pos;
    }

}
