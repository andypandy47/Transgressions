using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirKickBack : MonoBehaviour {

    PlayerWeaponSystem wSystem;
    Player player;

    public Vector3 kickBack;
    int kickBackDir;

    private void Start()
    {
        wSystem = GetComponent<PlayerWeaponSystem>();
        player = GetComponent<Player>();
    }

    public void KickBack()
    {
        kickBackDir = player.dir == Player.Direction.Right ? -1 : 1;

        player.velocity.x = kickBackDir * kickBack.x;
        player.velocity.y = kickBack.y;

        //print("Kickback");


    }

}
