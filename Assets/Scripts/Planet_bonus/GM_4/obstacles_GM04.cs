using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacles_GM04 : MonoBehaviour
{
    private GameObject galaxyRef;
    private float speed;
    private float rotateSpeed;
    //private float[] rowSpeed = new float[] {9, 8, 7, 6, 5, 4, 3};
    void OnEnable()
    {

        galaxyRef = GameObject.Find("Galaxy_pf");

        speed = 3f;
        rotateSpeed = Random.Range(1,3);
      
    }

   
    void FixedUpdate()
    {
        //transform.RotateAround(galaxyRef.transform.position, -Vector3.forward, 200f * Time.deltaTime);
        transform.position += Vector3.right * speed * Time.deltaTime;
        transform.Rotate (0,0, rotateSpeed);

        if(transform.position.x > 5f)
        transform.position = new Vector3(-5f, transform.position.y, transform.position.z);
    }
}
