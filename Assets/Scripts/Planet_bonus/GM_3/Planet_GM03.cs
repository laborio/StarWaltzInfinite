using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
using Lean.Pool;

public class Planet_GM03 : MonoBehaviour
{
   // public event EventHandler OnGM03Finished;
    private int unitValue = 1;
    private int xpValue;
    private int starValue;
    float timerRotation;
    float timerDuration;
    private float speed;
    Vector3 rotateDir;
    public GameObject star;
     List<GameObject> starList;

    void OnEnable() {
        
        starList = new List<GameObject>();


        xpValue = 20;

        starValue = Mathf.RoundToInt(LevelingSystem.Instance.GetLevelNumber());//5;//Random.Range(1,4); 

        SpawnStars();

    }

    void OnDisable() {
       starList.Clear(); 
    }
    void Start() {
         
        FrequencyRot();
    }

    void FixedUpdate() {
       
        rotate();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<BulletController>() != null) {
           
            ScoreManager.Instance.AddUnitCount(unitValue);
            ScoreManager.Instance.AddMiniGameStarCount(starValue);
            ScoreManager.Instance.AddMiniGameXpCount(xpValue);

            xpValue += 30;
          

        }
    }



    private void FrequencyRot() {

        speed = Random.Range(75f, 150f);
        float[] dirArray = new float[] {-1, 1};
        rotateDir = new Vector3(0,0,dirArray[Random.Range(0,dirArray.Length)]);

        timerDuration = Random.Range(3f,5f);
        timerRotation = 0f;

        
    }

    private void rotate() {
        
        timerRotation += Time.deltaTime;
     
       transform.Rotate(rotateDir * speed * Time.deltaTime);
      

       if(timerRotation >= timerDuration - 2f && timerRotation < timerDuration) {
        if(speed >= 0) speed -= 1f;
        else speed = 0f;
        
       }
      else if(timerRotation >= timerDuration)
       FrequencyRot();
       
    }

    private void SpawnStars() {
        int numberOfStars = Random.Range(2, 5);


        switch (numberOfStars)
        {
            case 2:
            GameObject obj = LeanPool.Spawn(star, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
            starList.Add(obj);
            GameObject obj1 = LeanPool.Spawn(star, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), transform.rotation);
            starList.Add(obj1);
            break;

            case 3:
            GameObject obj2 = LeanPool.Spawn(star, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
            starList.Add(obj2);
            GameObject obj3 = LeanPool.Spawn(star, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), transform.rotation);
            starList.Add(obj3);
            GameObject obj4 = LeanPool.Spawn(star, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), transform.rotation);
            starList.Add(obj4);
            break;

            case 4:
            GameObject obj5 = LeanPool.Spawn(star, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
            starList.Add(obj5);
            GameObject obj6 = LeanPool.Spawn(star, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), transform.rotation);
            starList.Add(obj6);
            GameObject obj7 = LeanPool.Spawn(star, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), transform.rotation);
            starList.Add(obj7);
            GameObject obj8 = LeanPool.Spawn(star, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), transform.rotation);
            starList.Add(obj8);
                       
            break;
        }
        foreach (GameObject star in starList)
        {
            star.transform.SetParent(gameObject.transform);
        }

    }

}
