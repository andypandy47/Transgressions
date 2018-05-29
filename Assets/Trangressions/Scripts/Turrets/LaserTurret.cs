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

    public enum State
    {
        Inactive,
        Idle,
        Charging,
        Firing
    }

    public State state;
    
    public override void Start()
    {
        state = State.Inactive;
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

        if (activated && state == State.Inactive)
            StartCoroutine(ActivateTurret());
        
        if (state != State.Inactive)
        {
            RotateTurret(reticule);

            if (timeToFire < .7f && !firing)
            {
                state = State.Charging;
                if (timeToFire < .2f)
                    locked = true;
            }


            shootDir = firePoint.position - transform.position;

            if (playerInRange)
                rFollow.MoveReticule(player, .1f, locked);

            if (canFire)
            {
                canFire = false;
                firing = true;
                StartCoroutine(Shoot());
            }

            Debug.DrawRay(firePoint.position, (shootDir * range) * rangeCompensation, Color.white);
        }
        
        if (reset)
        {
            reset = false;
            StartCoroutine(ResetThisTurret());
        }
            
    }

    IEnumerator ActivateTurret()
    {
        yield return new WaitForSeconds(0.1f);
        state = State.Idle;
    }

    IEnumerator Shoot()
    {
        state = State.Firing;

        if (!reset)
            yield return StartCoroutine(laserBeam.ShootLaser(player, lineR, shootDir, firePoint.position, laserLength, laserDuration, unitMask, wallMask, reset));
        
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

    void ResetFiring()
    {
        print("Laser reset");
        timeToFire = fireRate;
        firing = false;
        locked = false;
        state = State.Idle;
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
        state = State.Inactive;
        yield return null;
    }
}
