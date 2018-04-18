using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RArmAnimHandler : MonoBehaviour {

    PlayerWeaponSystem wSystem;
    Animator anim;
    Player player;

    private void Start()
    {
        wSystem = GetComponentInParent<PlayerWeaponSystem>();
        anim = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        anim.SetBool("RShooting", wSystem.rShooting);
        anim.SetBool("NoAim", wSystem.wState == PlayerWeaponSystem.WeaponState.NoAim ? true : false);
        anim.SetBool("Aiming", wSystem.wState == PlayerWeaponSystem.WeaponState.HalfAim || wSystem.wState == PlayerWeaponSystem.WeaponState.FullAim ? true : false);
        anim.SetBool("ResetArms", player.resetArms);
    }

    void RShootEnd()
    {
        wSystem.rShooting = false;

        //if (!wHandler.LShooting)
        //    wHandler.shooting = false;
    }
}
