using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret {

    Animator anim;
    LineRenderer lineR;
    ReticuleFollow rFollow;

    Transform reticule;
    Vector3 reticuleIdle = new Vector3(5, 0, 0);
    GameObject shootDirection;

    Vector3 shootDir;
    const float rangeCompensation = 2.629f;

    bool locked;

    public override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        lineR = GetComponent<LineRenderer>();
        rFollow = GetComponent<ReticuleFollow>();

        reticule = transform.GetChild(1).transform;
        shootDirection = transform.GetChild(2).gameObject;

        locked = false;
    }

    public override void Update()
    {
        base.Update();

        RotateTurret(reticule);

        if (timeToFire < .5f)
            locked = true;

        AnimatorBools();

        shootDir = firePoint.position - transform.position;

        if (playerInSight && playerInRange)
            rFollow.MoveReticule(player, .2f, locked);

        if (canFire)
        {
            print("Laser turret fired");
            Shoot();
        }
        Debug.DrawRay(firePoint.position, (shootDir * range) * rangeCompensation, Color.white);
    }

    void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, shootDir, Mathf.Infinity, whatToHit);
        
        if (hit)
        {
            if (hit.collider.tag == "PlayerCollider")
            {
                print("Player hit by laser");
            }
        }
        
        ResetFiring();
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

    void ResetFiring()
    {
        timeToFire = fireRate;
        canFire = false;
        locked = false;
    }

    void ReticuleControl()
    {

    }
}
