using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret {

    Animator anim;

    private void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        base.Update();
        AnimatorBools();

        if (canFire)
        {
            print("Laser turret fired");
            Shoot();
        }
    }

    void Shoot()
    {
        ResetTurret();
    }

    void AnimatorBools()
    {
        anim.SetBool("canFire", canFire);
        if (timeToFire < .5f)
        {
            anim.SetBool("charging", true);
        }
        else
        {
            anim.SetBool("charging", false);
        }

    }
}
