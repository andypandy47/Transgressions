using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret {

    Animator anim;
    LineRenderer lineR;
    ReticuleFollow rFollow;
    Beam laserBeam;

    Transform reticule;
    Vector3 reticuleIdle = new Vector3(5, 0, 0);
    GameObject shootDirection;

    Vector3 shootDir;
    private Quaternion goalDir;
    const float rangeCompensation = 2.629f;

    bool locked;

    public float laserLength;
    public float laserDuration;

    public LayerMask unitMask;
    public LayerMask wallMask;

    public override void Start()
    {
        base.Start();

        anim = GetComponent<Animator>();
        lineR = GetComponentInChildren<LineRenderer>();
        rFollow = GetComponent<ReticuleFollow>();
        laserBeam = GetComponent<Beam>();

        reticule = transform.GetChild(1).transform;
        shootDirection = transform.GetChild(2).gameObject;

        locked = false;
    }

    public override void Update()
    {
        base.Update();

        RotateTurret(reticule);

        if (timeToFire < .2f)
            locked = true;

        AnimatorBools();

        shootDir = firePoint.position - transform.position;

        if (playerInRange)
            rFollow.MoveReticule(player, .1f, locked);

        if (canFire)
        {
            print("Laser turret fired");
            canFire = false;
            firing = true;
            StartCoroutine(Shoot());
        }
        Debug.DrawRay(firePoint.position, (shootDir * range) * rangeCompensation, Color.white);

        if (reset)
        {
            reset = false;
            StartCoroutine(ResetThisTurret());
        }
            
    }

    IEnumerator Shoot()
    {
        if (!reset)
            yield return StartCoroutine(laserBeam.ShootLaser(player, lineR, shootDir, firePoint.position, laserLength, laserDuration, unitMask, wallMask, reset));
        print("Laser should stop now");
        
        ResetFiring();
        yield return false;
    }

    public void RotateTurret(Transform target)
    {
        if (target == null)
        {
            print("target is null");
            return;
        }

        //Get player direction
        dirToTarget = target.position - transform.position;
        angleToTarget = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;

        angleToTarget = AngleClamp(angleToTarget, angleClamp.x, angleClamp.y);

        //If target is within the turret bounds
        if (playerInSight && playerInRange)
        {
            //angle to rotate to = angle to the player
            angle = angleToTarget;
        }
        else
        {
            //else angle to rotate to = idleangle
            if (!locked)
                angle = idleAngle;
        }

        //Desired lerp angle
        goalDir = Quaternion.AngleAxis(angle, Vector3.forward);

        //Rotate accordingly
        transform.rotation = Quaternion.Lerp(transform.rotation, goalDir, Time.deltaTime * turnSpeed);
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
        print("Laser reset");
        timeToFire = fireRate;
        firing = false;
        locked = false;
    }

    IEnumerator ResetThisTurret()
    {
        print("Reset whole turret for level reset");
        StopCoroutine(laserBeam.ShootLaser(player, lineR, shootDir, firePoint.position, laserLength, laserDuration, unitMask, wallMask, reset));
        StopCoroutine(Shoot());
        laserBeam.ResetBeam(lineR, firePoint.position);
        rFollow.ResetReticule();
        locked = false;
        firing = false;
        timeToFire = fireRate;
        reset = false;
        yield return null;
    }
}
