using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LArmAnimHandler : MonoBehaviour {

    PlayerWeaponSystem wSystem;
    Player player;
    Animator anim;

    private void Start()
    {
        wSystem = GetComponentInParent<PlayerWeaponSystem>();
        player = GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!player.reset)
        {
            anim.SetBool("LShooting", wSystem.lShooting);
            anim.SetBool("SplitShootLeft", wSystem.wState == PlayerWeaponSystem.WeaponState.SplitAim && wSystem.lSplitShoot ? true : false);
            anim.SetBool("SplitAim", wSystem.wState == PlayerWeaponSystem.WeaponState.SplitAim ? true : false);
            anim.SetBool("NoAim", wSystem.wState == PlayerWeaponSystem.WeaponState.NoAim ? true : false);
            anim.SetBool("DirectionalInput", Mathf.Abs(player.directionalInput.x) > 0 ? true : false);
            anim.SetFloat("XVelocity", Mathf.Abs(player.velocity.x));
            anim.SetBool("Aiming", wSystem.wState == PlayerWeaponSystem.WeaponState.HalfAim || wSystem.wState == PlayerWeaponSystem.WeaponState.FullAim ? true : false);
        }
        

        anim.SetBool("ResetArms", player.reset);
    }

    void LShootEnd()
    {
        wSystem.lShooting = false;
        wSystem.lSplitShoot = false;
    }

    public void ResetAllAnimParams()
    {
        anim.SetBool("LShooting", false);
        anim.SetBool("SplitShootLeft", false);
        anim.SetBool("SplitAim", false);
        anim.SetBool("NoAim", false);
        anim.SetBool("DirectionalInput", false);
        anim.SetFloat("XVelocity", 0);
        anim.SetBool("Aiming", false);
    }
}
