using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [Header("Attributes")]
    public float fireRate;
    public float timeToFire;
    public float range;
    [Range (0.5f, 20) ]
    public float turnSpeed;
    public float aggroTime;
    float timeToReturnToNormalPos;
    [HideInInspector]
    public float angle, angleToTarget;
    float angleToPlayer;

    [Header("Unity Setup")]
    [HideInInspector]public Transform player;
    public Transform firePoint;
    public float idleAngle;
    public Vector2 angleClamp;
    public LayerMask whatToDetect;
    public LayerMask whatToHit;

    [HideInInspector]
    public bool canFire, firing, playerInRange, playerInSight, playerBehindWall, reset;

    [HideInInspector]
    public Vector3 dirToTarget, dirToPlayer;

    float refFloat;
    float timeToUpdate = 0;
    float tRange;
    float distanceToPlayer;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        firePoint = transform.GetChild(0).transform;

        timeToFire = fireRate;
        timeToReturnToNormalPos = aggroTime;
        playerInSight = true;
        canFire = false;
        tRange = 9999;
    }

    public virtual void Update()
    {
        if (player == null)
        {
            print("target is null");
            return;
        }
        CheckForAgrro();
        CoolDownTime();
        
    }

    void CheckForAgrro()
    {
        //Check target is in range
        distanceToPlayer = Vector3.Distance(firePoint.position, player.position);
        if (distanceToPlayer > range || !playerInSight)
        {
            playerInRange = false;
        }
        else
            playerInRange = true;

        dirToPlayer = player.position - transform.position;
        angleToPlayer = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Deg2Rad;

        //CheckPlayerNotBehindWall(dirToPlayer, distanceToPlayer, .3f);
        CheckPlayerWithinAngleBounds(angleToPlayer, angleClamp.x, angleClamp.y);
    }

    

    bool CheckPlayerNotBehindWall(Vector3 dir, float distance, float updateRate)
    {
        timeToUpdate -= Time.deltaTime;
        if (timeToUpdate < 0)
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir, distance, whatToDetect);
            Debug.DrawRay(firePoint.position, dir * distance, Color.blue);

            if (hit)
            {
                if (hit.collider.tag == "PlayerCollider")
                {
                    playerBehindWall = false;
                    //tRange = hit.distance;
                    //print("Player is visible, follow");
                }
                else if (hit.collider.tag == "Floor")
                {
                    playerBehindWall = true;
                    //tRange = hit.distance;
                    print("Player is behind wall, dont fire");
                }
            }

            timeToUpdate = updateRate;
        }
        
        //Debug.DrawRay(firePoint.position, dir, Color.green);
        return playerBehindWall;

    }

    //Function clamps angle
    public float AngleClamp(float angle, float min, float max)
    {
        if (angle <= min || angle >= max)
        {
            if (angle <= min)
            {
                angle = min;
            }
            if (angle >= max)
            {
                angle = max;
            }
        }

        return angle;
    }

    bool CheckPlayerWithinAngleBounds(float angle, float min, float max)
    {
        if (angle <= min || angle >= max)
        {
            if (angle <= min)
            {
                angle = min;
            }
            if (angle >= max)
            {
                angle = max;
            }
            playerInSight = false;
            print("player is not in sight " + angleToPlayer );
        }
        else
        {
            playerInSight = true;
        }

        return playerInSight;
    }

    void CoolDownTime()
    {
        //If turret cannot fire and target is in range and target is in sight
        if (playerInRange && playerInSight && !firing)
        {
            //Reduce time until it can fire next
            timeToFire -= Time.deltaTime;
        }
        else if (!playerInRange || !playerInSight)
        {
            timeToFire = fireRate;
            canFire = false;
        }
    }

    public void ResetTurret()
    {
        reset = true;
        transform.rotation = Quaternion.Euler(0, 0, idleAngle);
    }

    
}
