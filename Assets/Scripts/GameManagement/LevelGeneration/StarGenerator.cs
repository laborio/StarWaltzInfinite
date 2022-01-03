using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{

    public ObjectPool starPool;
    LevelGenerator levelGenerator;
    

    void Start() {
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    
    void Update()
    {
        
        
    }

    public void Spawn3Stars(Vector3 startPos, Vector3 lowerPos, Vector3 upperPos) {
       
        GameObject star1 = starPool.GetPooledObject();
        star1.transform.position = startPos;
        star1.SetActive(true);

        GameObject star2 = starPool.GetPooledObject();
        star2.transform.position = lowerPos;
        star2.SetActive(true);

        GameObject star3 = starPool.GetPooledObject();
        star3.transform.position = upperPos;
        star3.SetActive(true);
    }

     public void Spawn2Stars(Vector3 lowerPos, Vector3 upperPos) {
       

        GameObject star1 = starPool.GetPooledObject();
        star1.transform.position = lowerPos;
        star1.SetActive(true);

        GameObject star2 = starPool.GetPooledObject();
        star2.transform.position = upperPos;
        star2.SetActive(true);
    }

     public void Spawn1Star(Vector3 startPos) {
       
        GameObject star1 = starPool.GetPooledObject();
        star1.transform.position = startPos;
        star1.SetActive(true);
    }
    
       
}
