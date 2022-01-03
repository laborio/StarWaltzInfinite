using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostItemController : MonoBehaviour
{
    GameObject planet;

    void Start() {
        planet = GameObject.FindWithTag("StartPlanet");
        transform.position = planet.transform.position;
        transform.rotation = Quaternion.identity;
    }

    void OnEnable()
    {
        //hide planet
        planet = GameObject.FindWithTag("StartPlanet");
        transform.position = planet.transform.position;
        //transform.rotation = Quaternion.identity;

        planet.GetComponent<SpriteRenderer>().enabled = false;
        // Vector3 scaleSize = new Vector3(0.3f, 0.3f, 0.3f);
        // LeanTween.scale(gameObject, scaleSize, 2f).setEaseInOutSine().setLoopPingPong();
    }

    void OnDisable() {
        
        transform.position = new Vector3(transform.position.x, 1.4f, transform.position.z);

        if(planet.GetComponent<SpriteRenderer>() != null)
        planet.GetComponent<SpriteRenderer>().enabled = true;
    }

   
    void Update()
    {
        transform.position = planet.transform.position;
        transform.RotateAround(transform.position, -Vector3.forward, 30f * Time.deltaTime);
    }
}
