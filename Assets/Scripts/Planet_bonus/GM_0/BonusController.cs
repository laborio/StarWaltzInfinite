using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BonusController : MonoBehaviour
{
    public float speed;
	private float localtime = 0;
	private float rotateSpeed;

    SpriteRenderer spriteRenderer;
	new GameObject light;
	Light2D light2D;
    new BoxCollider2D collider;
    public GameObject particleEffect;
    public GameObject shockwaveEffect;
    private GameObject planetDestructionPoint;

    void Awake()
    {	
        //get components
        planetDestructionPoint = GameObject.FindWithTag("DestructionPoint"); //satellite is pooled from the same pool as planets
        light = this.gameObject.transform.GetChild(0).gameObject;
    	light2D = light.GetComponent<Light2D>();
        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //init values
    	light2D.intensity = 0.7f;
        rotateSpeed = Random.Range(0.3f, 1.2f);
        speed = Random.Range (0.5f, 1f);
      
    }

    void OnEnable() {
        transform.gameObject.tag = "Planet";
        transform.localScale = new Vector3(.35f, .35f, .35f);
        spriteRenderer.enabled = true;
        collider.enabled = true;
    }

    void OnDisable() {
        transform.gameObject.tag = "Untagged"; 
    }

  
    void Update()
    {   
         if (transform.position.y < planetDestructionPoint.transform.position.y) {
            gameObject.SetActive(false);
        }

        localtime += Time.deltaTime;
        transform.Rotate (0,0, rotateSpeed);

            int index = Random.Range(1, 8);

            if (index >= 8)
            transform.position = new Vector3(Mathf.PingPong(localtime * speed, 4f) - 2f, transform.position.y, transform.position.z);
            
        
		
    }


     void OnTriggerEnter2D(Collider2D other)
    {
     	   if (other.CompareTag("Player"))
     	   	{	
               
     	   		ScreenShakeController.instance.StartShake(.2f, .2f);
     	   		
                GameObject obj1 = Instantiate(particleEffect, transform.position, Quaternion.identity);
                GameObject obj2 = Instantiate(shockwaveEffect, transform.position, Quaternion.identity);

                Destroy(obj1, 2);
                Destroy(obj2, 2);

                collider.enabled = !collider.enabled;
                spriteRenderer.enabled = !spriteRenderer.enabled;
     	   		

    		}
    }
}
