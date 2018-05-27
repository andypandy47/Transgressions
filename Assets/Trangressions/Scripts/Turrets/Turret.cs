using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [Header("Attributes")]
    public float fireRate;
    public float timeToFire;
    public float range;
    [Range (0.5f, 5) ]
    public float turnSpeed;
    public float aggroTime;
    float timeToReturnToNormalPos;
    float angle;
    float angleToTarget;
    float angleToPlayer;

    [Header("Unity Setup")]
    [HideInInspector]public Transform player;
    public Transform firePoint;
    public float idleAngle;
    public Vector2 angleClamp;
    public LayerMask whatToDetect;
    public LayerMask whatToHit;

    [HideInInspector]
    public bool canFire, playerInRange, playerInSight, playerBehindWall;
    private Quaternion goalDir;

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

        CheckPlayerNotBehindWall(dirToPlayer, distanceToPlayer, .3f);
        CheckPlayerWithinAngleBounds(angleToPlayer, angleClamp.x, angleClamp.y);
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
            angle = idleAngle;
        }
        
        //Desired lerp angle
        goalDir = Quaternion.AngleAxis(angle, Vector3.forward);

        //Rotate accordingly
        transform.rotation = Quaternion.Lerp(transform.rotation, goalDir, Time.deltaTime * turnSpeed);
        

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
                    //print("Player is behind wall, dont fire");
                }
            }

            timeToUpdate = updateRate;
        }
        
        //Debug.DrawRay(firePoint.position, dir, Color.green);
        return playerBehindWall;

    }

    //Function clamps angle but also checks if player is out of angle bounds e.g not behind turret wall
    float AngleClamp(float angle, float min, float max)
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
            //print("player is not in sight");
        }
        else
        {
            if (!playerBehindWall)
            {
                playerInSight = true;
                //print("player is in sight, follow");
            }
            else
                playerInSight = false;
        }

        return playerInSight;
    }

    void CoolDownTime()
    {
        //If turret cannot fire and target is in range and target is in sight
        if (!canFire && playerInRange && playerInSight)
        {
            //Reduce time until it can fire next
            timeToFire -= Time.deltaTime;
            if (timeToFire <= 0)
                canFire = true;
        }
        else if (!playerInRange || !playerInSight)
        {
            timeToFire = fireRate;
            canFire = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.transform.position, range);

        //Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + Vector3.left * 2, transform.position + Vector3.right * 2);
    }
}
