using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMenu : MonoBehaviour
{
    private Transform target;
    void Start() {
        target = GameObject.FindWithTag("StartPlanet").transform;
        transform.position = new Vector3(0f, 1.44f, 0f);
       // transform.position = new Vector3 (target.position.x,target.position.y - 1f, target.position.z);
    }

    
    void OnEnable()
    {
        target = GameObject.FindWithTag("StartPlanet").transform;
        transform.position = new Vector3(0f, 1.44f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.RotateAround(target.position, -Vector3.forward, 200f * Time.deltaTime);
    }
}
