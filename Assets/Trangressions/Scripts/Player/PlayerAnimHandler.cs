﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour {

    public Animator anim;
    Player player;
    PlayerWeaponSystem wSystem;
    Controller2D controller;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        wSystem = GetComponent<PlayerWeaponSystem>();
        controller = GetComponent<Controller2D>();
    }

    private void Update()
    {
        anim.SetFloat("XVelocity", Mathf.Abs(player.velocity.x));
        anim.SetBool("DirectionalInput", Mathf.Abs(player.directionalInput.x) > 0 ? true : false);
        anim.SetBool("Grounded", player.controller.collisions.below);
        anim.SetBool("Shooting", wSystem.lShooting || wSystem.rShooting ? true : false);

        anim.SetBool("NoAim", wSystem.wState == PlayerWeaponSystem.WeaponState.NoAim ? true : false);
        anim.SetBool("HalfAim", wSystem.wState == PlayerWeaponSystem.WeaponState.HalfAim ? true : false);
        anim.SetBool("FullAim", wSystem.wState == PlayerWeaponSystem.WeaponState.FullAim ? true : false);
        anim.SetBool("SplitAim", wSystem.wState == PlayerWeaponSystem.WeaponState.SplitAim ? true : false);
        anim.SetBool("WalkBackwards", player.state == Player.pState.BackwardsWalk ? true : false);

        if (!player.wallSliding && player.state != Player.pState.BackwardsWalk)
        {
            if (player.velocity.x < 0 && player.dir == Player.Direction.Right)
            {
               
                Flip(false);

                print(wSystem.hasBackwardsWalked);
            }

            if (player.velocity.x > 0 && player.dir == Player.Direction.Left)
            {
                Flip(false);
                //print(player.state);
            }
            
        }
        
    }

    public void Flip(bool dontFlipChildren)
    {
        if (player.dir == Player.Direction.Left)
            player.dir = Player.Direction.Right;

        else if (player.dir == Player.Direction.Right)
            player.dir = Player.Direction.Left;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        if (dontFlipChildren)
        {
            foreach (Transform child in transform)
            {
                Vector3 childScale = child.transform.localScale;
                childScale.x *= -1;
                child.transform.localScale = childScale;
            }
        }
        
        print("Player flip" + " were children not flipped?" + dontFlipChildren);
    }
}
