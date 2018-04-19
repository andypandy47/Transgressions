using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBAnimHandler : MonoBehaviour {

    NBController controller;
    Animator anim;

    private void Start()
    {
        controller = GetComponent<NBController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("Dead", controller.dead);
    }
}
