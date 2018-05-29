using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurretAnimHandler : MonoBehaviour {

    LaserTurret turret;
    Animator anim;

    private void Start()
    {
        turret = GetComponent<LaserTurret>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("Activated", turret.state == LaserTurret.State.Inactive ? false : true);
        anim.SetBool("Charging", turret.state == LaserTurret.State.Charging ? true : false);
        anim.SetBool("Firing", turret.state == LaserTurret.State.Firing ? true : false);
    }
}
