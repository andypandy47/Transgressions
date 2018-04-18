using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RArmAnimHandler : MonoBehaviour {

    PlayerWeaponSystem wSystem;
    Animator anim;

    private void Start()
    {
        wSystem = GetComponentInParent<PlayerWeaponSystem>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("RShooting", wSystem.rShooting);
        anim.SetBool("NoAim", wSystem.wState == PlayerWeaponSystem.WeaponState.NoAim ? true : false);
    }

    void RShootEnd()
    {
        wSystem.rShooting = false;

        //if (!wHandler.LShooting)
        //    wHandler.shooting = false;
    }
}
