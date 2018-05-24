using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [Header("Attributes")]
    public float fireRate;
    public float timeToFire;
    public float range;
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

    [HideInInspector]
    public bool canFire, targetInRange, targetInSight;
    private Quaternion goalDir;

    float refFloat;

    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        firePoint = transform.GetChild(0).transform;

        timeToFire = fireRate;
        timeToReturnToNormalPos = aggroTime;
        targetInSight = true;
    }

    public void Update()
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

        //Get player direction
        Vector3 dir = target.position - transform.position;
        angleToTarget = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angleToTarget = AngleClamp(angleToTarget, angleClamp.x, angleClamp.y);

        //If target is within the turret bounds
        if (targetInSight)
        {
            angle = angleToTarget;
        }
        else
        {
            angle = idleAngle;
        }
        


        //Desired lerp angle
        goalDir = Quaternion.AngleAxis(angle, Vector3.forward);


        //Check target is in range
        float distanceToTarget = Vector3.Distance(firePoint.position, target.position);
        if (distanceToTarget > range || !targetInSight)
        {
            targetInRange = false;
        }
        else
            targetInRange = true;

        //Rotate accordingly
        transform.rotation = Quaternion.Lerp(transform.rotation, goalDir, Time.deltaTime);
        

    }

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
            //print("In turret sight");
            timeToReturnToNormalPos = aggroTime;
            targetInSight = true;
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
