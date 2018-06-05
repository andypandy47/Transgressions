using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurretAnimHandler : MonoBehaviour {

    LaserTurret turret;
    Animator anim;
    LaserTurretAudio lAudio;

    private void Start()
    {
        turret = GetComponent<LaserTurret>();
        anim = GetComponent<Animator>();
        lAudio = GetComponent<LaserTurretAudio>();
    }

    private void Update()
    {
        anim.SetBool("Charging", turret.state == LaserTurret.State.Charging ? true : false);
        anim.SetBool("Firing", turret.state == LaserTurret.State.Firing ? true : false);
        anim.SetBool("Idle", turret.state == LaserTurret.State.Idle ? true : false);
    }
}
