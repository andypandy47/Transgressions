using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret {

    Animator anim;
    LineRenderer lineR;
    GameObject reticule;
    GameObject shootDirection;

    Vector3 shootDir;
    const float rangeCompensation = 2.629f;

    public override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        lineR = GetComponent<LineRenderer>();

        reticule = transform.GetChild(1).gameObject;
        shootDirection = transform.GetChild(2).gameObject;
    }

    public override void Update()
    {
        base.Update();
        AnimatorBools();

        shootDir = firePoint.position - transform.position;

        ReticuleControl();
        if (canFire)
        {
            print("Laser turret fired");
            Shoot();
        }
        Debug.DrawRay(firePoint.position, (shootDir * range) * rangeCompensation, Color.white);
        //print(firePoint.position);
    }

    void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, shootDir,  range * rangeCompensation, whatToHit);
        if (hit)
        {
            if (hit.collider.tag == "player")
            {
                print("Player hit by laser");
            }
        }
        
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

    void ReticuleControl()
    {

    }
}
