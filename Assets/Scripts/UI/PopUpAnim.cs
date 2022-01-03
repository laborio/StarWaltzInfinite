using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpAnim : MonoBehaviour
{
    public Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void OnEnable()
    {
        anim.SetTrigger("pop");
    }
}
