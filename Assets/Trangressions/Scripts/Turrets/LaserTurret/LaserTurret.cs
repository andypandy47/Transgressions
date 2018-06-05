using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : Turret {

    Animator anim;
    LineRenderer lineR;
    ReticuleFollow rFollow;
    Beam laserBeam;
    LaserTurretAudio lAudio;

    Transform reticule;
    Vector3 reticuleIdle = new Vector3(5, 0, 0);
    GameObject shootDirection;

    Vector3 shootDir;
    private Quaternion goalDir;
    const float rangeCompensation = 2.629f;

    bool locked;

    public float laserDuration;

    public LayerMask unitMask;
    public LayerMask wallMask;
    private IEnumerator shootLaser;

    public enum State
    {
        Idle,
        Charging,
        Firing
    }

    public State state;
    
    public override void Start()
    {
        state = State.Idle;
        base.Start();

        anim = GetComponent<Animator>();
        lineR = GetComponentInChildren<LineRenderer>();
        rFollow = GetComponent<ReticuleFollow>();
        laserBeam = GetComponent<Beam>();
        lAudio = GetComponent<LaserTurretAudio>();

        reticule = transform.GetChild(1).transform;
        shootDirection = transform.GetChild(2).gameObject;

        locked = false;
    }

    public override void Update()
    {
        base.Update();
        
        if (state != State.Firing)
        {
            if (playerInRange)
                rFollow.MoveReticule(player, .1f, locked);

            RotateTurret(reticule);

            if (!lAudio.isPlaying && timeToFire < 1.4)
                lAudio.StartLaserSound();

            if (timeToFire < .7f && !firing)
            {
                state = State.Charging;
                if (timeToFire < .2f)
                    locked = true;
            }

            shootDir = firePoint.position - transform.position;

            if (timeToFire <= 0)
            {
                firing = true;
                timeToFire = fireRate;
                StartCoroutine(Shoot());
            }

            Debug.DrawRay(firePoint.position, (shootDir * range) * rangeCompensation, Color.white);
        }
        
        if (reset)
        {
            StartCoroutine(ResetThisTurret());
        }
            
    }

    IEnumerator Shoot()
    {
        state = State.Firing;
        shootLaser = laserBeam.ShootLaser(player, lineR, shootDir, firePoint.position, laserDuration, unitMask, wallMask, reset);
        if (!reset)
            yield return StartCoroutine(shootLaser);
        
        ResetFiring();
        lAudio.StopLaser();
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
        firing = false;
        locked = false;
        state = State.Idle;
    }

    IEnumerator ResetThisTurret()
    {
        print("Reset whole turret for level reset");
        StopCoroutine(shootLaser);
        StopAllCoroutines();
        laserBeam.ResetBeam(lineR, firePoint.position);
        rFollow.ResetReticule();
        lAudio.ResetTurretAudio();
        locked = false;
        firing = false;
        timeToFire = fireRate;
        reset = false;
        state = State.Idle;
        yield return null;
    }
}
