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

    [Header("Unity Setup")]
    private Transform target;
    public Transform firePoint;
    public float idleAngle;
    public Vector2 angleClamp;
    public LayerMask whatToDetect;
    public LayerMask whatToHit;

    [HideInInspector]
    public bool canFire, targetInRange, targetInSight, targetBehindWall;
    private Quaternion goalDir;

    [HideInInspector]
    public Vector3 dir;

    float refFloat;
    float timeToUpdate = 0;
    float tRange;
    float distanceToTarget;

    public virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        firePoint = transform.GetChild(0).transform;

        timeToFire = fireRate;
        timeToReturnToNormalPos = aggroTime;
        targetInSight = true;
        tRange = 9999;
    }

    public virtual void Update()
    {
        if (target == null)
        {
            print("target is null");
            return;
        }

        LookAtTarget();
        CoolDownTime();
    }

    void LookAtTarget()
    {
        if (target == null)
        {
            print("target is null");
            return;
        }

        //Check target is in range
        distanceToTarget = Vector3.Distance(firePoint.position, target.position);
        if (distanceToTarget > range || !targetInSight)
        {
            targetInRange = false;
        }
        else
            targetInRange = true;

        //Get player direction
        dir = target.position - transform.position;
        angleToTarget = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        CheckPlayerNotBehindWall(dir, distanceToTarget, .3f);
        angleToTarget = AngleClamp(angleToTarget, angleClamp.x, angleClamp.y);

        //If target is within the turret bounds
        if (targetInSight && targetInRange)
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

            if (hit)
            {
                if (hit.collider.tag == "Player")
                {
                    targetBehindWall = false;
                    //tRange = hit.distance;
                    //print("Player is visible, fire");
                }
                else if (hit.collider.tag == "Floor")
                {
                    targetBehindWall = true;
                    //tRange = hit.distance;
                    print("Player is behind wall, dont fire");
                }
            }

            timeToUpdate = updateRate;
        }
        
        //Debug.DrawRay(firePoint.position, dir, Color.green);
        return targetBehindWall;

    }

    //Function clamps angle but also checks if player is out of angle bounds e.g not behind turret wall
    float AngleClamp(float angle, float min, float max)
    {
        if (angle <= min || angle >= max)
        {
            targetInSight = false;

            if (angle <= min)
            {
                angle = min;
            }
            if (angle >= max)
            {
                angle = max;
            }
        }
        else
        {
            //timeToReturnToNormalPos = aggroTime;
            //player is within angle bounds
            if (!targetBehindWall)
                targetInSight = true;
            else
                targetInSight = false;
        }

        return angle;
    }

    void CoolDownTime()
    {
        //If turret cannot fire and target is in range and target is in sight
        if (!canFire && targetInRange && targetInSight)
        {
            //Reduce time until it can fire next
            timeToFire -= Time.deltaTime;
            if (timeToFire <= 0)
                canFire = true;
        }
        else if (!targetInRange || !targetInSight)
        {
            timeToFire = fireRate;
            canFire = false;
        }
    }

    public void ResetTurret()
    {
        canFire = false;
        timeToFire = fireRate;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.transform.position, range);

        //Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + Vector3.left * 2, transform.position + Vector3.right * 2);
    }
}
