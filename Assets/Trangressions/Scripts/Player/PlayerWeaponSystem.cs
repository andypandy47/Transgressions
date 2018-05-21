using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour {

    Controller2D controller;
    Player player;
    PlayerAnimHandler pAnimHandler;
    AirKickBack airKickBack;
    CamShake camShake;

    public GameObject testPrefab;

    public enum WeaponState
    {
        NoAim,
        HalfAim,
        FullAim,
        SplitAim
    }

    public WeaponState wState;

    public float fireRate, timeToIdle, range;
    float rArmCoolDown, lArmCoolDown, idleTimer;

    [HideInInspector] public bool rShooting, lShooting, lSplitShoot, inAirShooting, lArmFlipped;
    bool flashFlipped;
    public bool hasBackwardsWalked;

    public List<string> altShoot;
    string right = "Right";
    string left = "Left";

    [HideInInspector] public Transform RArm, LArm, muzzlePoint, muzzlePointSplit;
    GameObject muzzleFlashPrefab;

    [HideInInspector]
    public Vector3 splitShootLeftArmPos = new Vector3(-.302f, -0.278f, 0), normalLeftArmPos = new Vector3(0.186f, -0.261f, 0),
        rightArmJumpShootPos = new Vector3(0.178f, -0.331f, 0), leftArmJumpShootPos = new Vector3(0.157f, -0.356f, 0),
        normalRightPos = new Vector3(0.311f, -0.277f, 0);
    Vector3 pScale;

    // public RaycastHit2D lHit;
    // public RaycastHit2D rHit;
    public RaycastHit2D hit;
    public LayerMask whatToHit;

    Enemy enemy;

    [FMODUnity.EventRef]
    public string gunShotFmodEvent;

    private void Start()
    {
        player = GetComponent<Player>();
        pAnimHandler = GetComponent<PlayerAnimHandler>();
        airKickBack = GetComponent<AirKickBack>();
        controller = GetComponent<Controller2D>();
        camShake = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CamShake>();

        muzzlePoint = transform.GetChild(0).transform;
        muzzlePointSplit = transform.GetChild(1).transform;
        RArm = transform.GetChild(2).transform;
        LArm = transform.GetChild(3).transform;

        flashFlipped = false;
    }

    private void Update()
    {
        rArmCoolDown -= Time.deltaTime;
        lArmCoolDown -= Time.deltaTime;
        idleTimer -= Time.deltaTime;

        pScale = player.transform.localScale;

        if (idleTimer <= 0)
        {
            wState = WeaponState.NoAim;

            //When returning to idle from the player direction split state, make sure player knows which direction theyre facing
            if (pScale.x == 1)
            {
                player.dir = Player.Direction.Right;
            }
            else
            {
                player.dir = Player.Direction.Left;
            }
            rShooting = false;
            lShooting = false;
            hasBackwardsWalked = false;
            altShoot.Clear();
        }

        if (player.wallSliding)
            wState = WeaponState.NoAim;

        if (wState == WeaponState.SplitAim && player.hasHorInput || wState == WeaponState.HalfAim && player.hasHorInput)
        {
            wState = WeaponState.FullAim;

            //Check if arm is flipped from splitshoot and flip back to normal pos
            if (lArmFlipped && wState != WeaponState.SplitAim)
            {
                FlipArm(LArm, normalLeftArmPos);
            }
        }

        if (player.hasHorInput && hasBackwardsWalked && controller.state != Controller2D.pState.BackwardsWalk)
        {
            hasBackwardsWalked = false;
        }

        if (player.grounded && wState != WeaponState.SplitAim)
        {
            RArm.gameObject.transform.localPosition = normalRightPos;
            LArm.gameObject.transform.localPosition = normalLeftArmPos;
        }

    }

    public void RightWeaponInput()
    {
        rShooting = false;

        if (!player.wallSliding)
        {
            //Check if player is not moving
            if (!player.hasHorInput && player.grounded)
            {
                //If state is idle and player is facing the wrong way, flip to face right
                if (wState == WeaponState.NoAim && player.dir == Player.Direction.Left)
                {
                    pAnimHandler.Flip(false);
                }

                //Check if arm is flipped from splitshoot and flip back to normal pos
                if (lArmFlipped && wState != WeaponState.SplitAim)
                {
                    FlipArm(LArm, normalLeftArmPos);
                }

                //If weapon state is idle, then fire and switch to state.HalfAim
                if (wState == WeaponState.NoAim)
                {
                    if (controller.state != Controller2D.pState.Sliding)
                    {
                        wState = WeaponState.HalfAim;
                    }
                    else
                        wState = WeaponState.FullAim;

                    RightShoot(false, false);
                }
                //Else if state equals half aim already then shoot with the other weapon
                else if (wState == WeaponState.HalfAim && player.dir == Player.Direction.Right)
                {
                    wState = WeaponState.FullAim;
                    AlternateShooting();
                }
                //Else if state is in full aim already and player is facing right then continue to altShoot
                else if (wState == WeaponState.FullAim && player.dir == Player.Direction.Right)
                {
                    AlternateShooting();
                }
                //Initial change to split aim
                else if (player.dir == Player.Direction.Left && wState == WeaponState.HalfAim || player.dir == Player.Direction.Left && wState == WeaponState.FullAim)
                {
                    wState = WeaponState.SplitAim;
                    pAnimHandler.Flip(false);
                    player.dir = Player.Direction.Split;

                    //Flip arm to splitshoot pos
                    if (!lArmFlipped)
                    {
                        FlipArm(LArm, splitShootLeftArmPos);
                    }

                    rShooting = false;

                    RightShoot(true, true);
                }
                //Split aim shot
                else if (wState == WeaponState.SplitAim)
                {
                    if (rArmCoolDown <= 0)
                        RightShoot(true, false);
                    else
                    {
                        wState = WeaponState.FullAim;

                        if (pScale.x == -1)
                            pAnimHandler.Flip(false);
                        if (lArmFlipped)
                            FlipArm(LArm, normalLeftArmPos);

                        player.dir = Player.Direction.Right;
                        LeftShoot(false, false);
                    }
                }
            }
            //If the player is running
            else if (player.hasHorInput && player.grounded)
            {
                //If player is weapon idle 
                if (wState == WeaponState.NoAim)
                {
                    //Shoot with right and switch to full aim mode
                    if (player.dir == Player.Direction.Right)
                    {
                        wState = WeaponState.FullAim;
                        RightShoot(false, false);
                    }
                    //else if player is running left then go into backwards walk mode
                    else if (player.dir == Player.Direction.Left)
                    {
                        wState = WeaponState.FullAim;
                        pAnimHandler.Flip(false);
                        controller.state = Controller2D.pState.BackwardsWalk;
                        player.dir = Player.Direction.Right;
                        hasBackwardsWalked = true;

                        RightShoot(false, false);
                    }
                }
                //TODO: PLAYER CANT HAVE SPLIT AIM WHEN RUNNING SO GET RID OF THIS SHIT
                else if (wState == WeaponState.SplitAim)
                {
                    if (rArmCoolDown <= 0)
                        RightShoot(true, false);
                    else
                    {
                        wState = WeaponState.FullAim;

                        if (lArmFlipped)
                            FlipArm(LArm, normalLeftArmPos);

                        LeftShoot(false, false);
                    }
                }
                //If player is in full aim mode when running to the right, do the usual alt fire
                else if (wState == WeaponState.FullAim && player.dir == Player.Direction.Right)
                {
                    AlternateShooting();
                }
                //If player is in full aim mdoe when running to the left, switch to backwards walk
                else if (wState == WeaponState.FullAim && player.dir == Player.Direction.Left)
                {
                    wState = WeaponState.FullAim;
                    pAnimHandler.Flip(false);
                    controller.state = Controller2D.pState.BackwardsWalk;
                    player.dir = Player.Direction.Right;
                    hasBackwardsWalked = true;

                    RightShoot(false, false);
                }
            }
            else if (!player.grounded)
            {
                inAirShooting = true;
                if (wState == WeaponState.NoAim)
                {
                    //Shoot with right and switch to full aim mode
                    if (player.dir == Player.Direction.Right)
                    {
                        wState = WeaponState.FullAim;
                        LeftShoot(false, false);
                    }
                    else if (player.dir == Player.Direction.Left)
                    {
                        pAnimHandler.Flip(false);
                        wState = WeaponState.FullAim;
                        LeftShoot(false, false);
                    }
                    RArm.gameObject.transform.localPosition = rightArmJumpShootPos;
                    LArm.gameObject.transform.localPosition = leftArmJumpShootPos;
                }
                else if (wState == WeaponState.FullAim && player.dir == Player.Direction.Right)
                {
                    AlternateShooting();
                }
                else if (wState == WeaponState.FullAim && player.dir == Player.Direction.Left)
                {
                    //AlternateShooting();
                }
            }
        }
    }

    public void LeftWeaponInput()
    {
        lShooting = false;

        if (!player.wallSliding)
        {
            if (!player.hasHorInput && player.grounded)
            {
                //If state is idle and player is facing the wrong way, flip to face left
                if (wState == WeaponState.NoAim && player.dir == Player.Direction.Right)
                {
                    pAnimHandler.Flip(false);
                }

                //Check if arm is flipped from splitshoot and flip back to normal pos
                if (lArmFlipped && wState != WeaponState.SplitAim)
                {
                    FlipArm(LArm, normalLeftArmPos);
                }

                //If weapon state is idle, then fire and switch to state.HalfAim
                if (wState == WeaponState.NoAim)
                {
                    if (controller.state != Controller2D.pState.Sliding)
                    {
                        wState = WeaponState.HalfAim;
                    }
                    else
                        wState = WeaponState.FullAim;

                    RightShoot(false, false);
                }
                //Else if state equals half aim already then shoot with the other weapon
                else if (wState == WeaponState.HalfAim && player.dir == Player.Direction.Left)
                {
                    wState = WeaponState.FullAim;
                    AlternateShooting();
                }
                //Else if state is in full aim already and player is facing left then continue to altShoot
                else if (wState == WeaponState.FullAim && player.dir == Player.Direction.Left)
                {
                    AlternateShooting();
                }
                //Initial change to split aim
                else if (player.dir == Player.Direction.Right && wState == WeaponState.HalfAim || player.dir == Player.Direction.Right && wState == WeaponState.FullAim)
                {
                    wState = WeaponState.SplitAim;
                    player.dir = Player.Direction.Split;

                    //Flip left arm to splitshoot pos
                    if (!lArmFlipped)
                    {
                        FlipArm(LArm, splitShootLeftArmPos);
                    }

                    LeftShoot(true, false);
                }
                //Split aim shot
                else if (wState == WeaponState.SplitAim)
                {
                    //If cooldown is done split shoot left again, else go into full left alternate shooting
                    if (lArmCoolDown <= 0)
                        LeftShoot(true, false);
                    else
                    {
                        wState = WeaponState.FullAim;

                        if (pScale.x == 1)
                            pAnimHandler.Flip(false);
                        if (lArmFlipped)
                            FlipArm(LArm, normalLeftArmPos);

                        player.dir = Player.Direction.Left;

                        LeftShoot(false, true);
                    }
                }
            }
            else if (player.hasHorInput && player.grounded)
            {
                //If player is weapon idle 
                if (wState == WeaponState.NoAim)
                {
                    //Shoot with left and switch to full aim mode
                    if (player.dir == Player.Direction.Left)
                    {
                        wState = WeaponState.FullAim;
                        LeftShoot(false, false);
                    }
                    //else if player is running right then go into backwards walk mode
                    else if (player.dir == Player.Direction.Right)
                    {
                        wState = WeaponState.FullAim;
                        pAnimHandler.Flip(false);
                        controller.state = Controller2D.pState.BackwardsWalk;
                        player.dir = Player.Direction.Left;
                        hasBackwardsWalked = true;

                        LeftShoot(false, false);
                    }
                }
                //TODO: PLAYER CANT HAVE SPLIT AIM WHEN RUNNING SO GET RID OF THIS SHIT
                else if (wState == WeaponState.SplitAim)
                {
                    if (lArmCoolDown <= 0)
                        LeftShoot(true, false);
                    else
                    {
                        wState = WeaponState.FullAim;

                        if (lArmFlipped)
                            FlipArm(LArm, normalLeftArmPos);

                        LeftShoot(false, false);
                    }
                }
                //If player is in full aim mode when running to the left, do the usual alt fire
                else if (wState == WeaponState.FullAim && player.dir == Player.Direction.Left)
                {
                    AlternateShooting();
                }
                //If player is in full aim mdoe when running to the left, switch to backwards walk
                else if (wState == WeaponState.FullAim && player.dir == Player.Direction.Right)
                {
                    wState = WeaponState.FullAim;
                    pAnimHandler.Flip(false);
                    controller.state = Controller2D.pState.BackwardsWalk;
                    player.dir = Player.Direction.Left;
                    hasBackwardsWalked = true;

                    LeftShoot(false, false);
                }
            }
            //Player is in the air
            else if (!player.grounded)
            {
                inAirShooting = true;
                if (wState == WeaponState.NoAim)
                {
                    //Shoot with left and switch to full aim mode
                    if (player.dir == Player.Direction.Left)
                    {
                        wState = WeaponState.FullAim;
                        LeftShoot(false, false);
                    }
                    else if (player.dir == Player.Direction.Right)
                    {
                        pAnimHandler.Flip(false);
                        wState = WeaponState.FullAim;
                        LeftShoot(false, false);
                    }

                    RArm.gameObject.transform.localPosition = rightArmJumpShootPos;
                    LArm.gameObject.transform.localPosition = leftArmJumpShootPos;
                }
                else if (wState == WeaponState.FullAim && player.dir == Player.Direction.Left)
                {
                    AlternateShooting();
                }
                else if (wState == WeaponState.FullAim && player.dir == Player.Direction.Right)
                {
                    //AlternateShooting();
                }
            }
        }
    }

    void RightShoot(bool splitShot, bool ignoreCoolDown)
    {
        
        //Only fire if cool down is okay
        if (rArmCoolDown <= 0 && !ignoreCoolDown)
        {
            rShooting = true;

            rArmCoolDown = fireRate;
            idleTimer = timeToIdle;

            //Add to shot list
            altShoot.Add(right);
            ShotEffect(false);
            ShotRaycast(right, splitShot);
        }
        else if (ignoreCoolDown)
        {
            rShooting = true;

            rArmCoolDown = fireRate;
            idleTimer = timeToIdle;

            //Add to shot list
            altShoot.Add(right);
            ShotEffect(false);
            ShotRaycast(right, splitShot);
        }

        
        
    }

    void LeftShoot(bool splitShot, bool ignoreCoolDown)
    {
        if (!splitShot)
        {
            //Only fire if cooldown is okay
            if (lArmCoolDown <= 0 && !ignoreCoolDown)
            {
                lShooting = true;

                lArmCoolDown = fireRate;
                idleTimer = timeToIdle;

                //Add to shot list
                altShoot.Add(left);
                ShotEffect(false);
                ShotRaycast(left, splitShot);
            }
            else if (ignoreCoolDown)
            {
                lShooting = true;

                lArmCoolDown = fireRate;
                idleTimer = timeToIdle;

                //Add to shot list
                altShoot.Add(left);
                ShotEffect(false);
                ShotRaycast(left, splitShot);
            }
            
        }
        else
        {
            lShooting = false;
            lSplitShoot = true;

            lArmCoolDown = fireRate;
            idleTimer = timeToIdle;

            //Add to shot list
            altShoot.Add(left);
            ShotEffect(true);
            ShotRaycast(left, splitShot);
        }
    }

    void AlternateShooting()
    {
        //If previous shot is with the right weapon then shoot with the left, this code ensures shooting will
        //always alternate between the two pistols
        if (altShoot[altShoot.Count - 1] == right)
        {
            LeftShoot(false, false);
        }
        else
        {
            RightShoot(false, false);
        }
    }

    void ShotRaycast(string weapon, bool splitShot)
    {
        if (weapon == right)
        {
            hit = Physics2D.Raycast(muzzlePoint.position, player.dir == Player.Direction.Right || player.dir == Player.Direction.Split ? Vector2.right : Vector2.left, range, whatToHit);
            Debug.DrawRay(muzzlePoint.position, player.dir == Player.Direction.Right || player.dir == Player.Direction.Split ? Vector2.right * range : Vector2.left * range, Color.cyan);
        }
        else if (weapon == left && !splitShot)
        {
            hit = Physics2D.Raycast(muzzlePoint.position, player.dir == Player.Direction.Left && player.dir != Player.Direction.Split ? Vector2.left : Vector2.right, range, whatToHit);
            Debug.DrawRay(muzzlePoint.position, player.dir == Player.Direction.Left && player.dir != Player.Direction.Split ? Vector2.left * range : Vector2.right * range, Color.magenta);
        }
        else if (weapon == left && splitShot)
        {
            hit = Physics2D.Raycast(muzzlePointSplit.position, Vector2.left, range, whatToHit);
            Debug.DrawRay(muzzlePointSplit.position,Vector2.left * range, Color.magenta);
        }

        if (hit)
        {
            print("Hit " + hit.collider.name);
            if (hit.collider.tag == "Enemy")
            {
                /*if (!GotEnemy(hit.collider))
                {
                    enemy = hit.collider.GetComponent<Enemy>();
                }*/
                enemy = hit.collider.GetComponent<Enemy>();
                enemy.DamageEnemy(100f, player);

            }
        }
        else
            return;
    }

    bool GotEnemy(Collider2D collider)
    {
        if (enemy == null)
            return false;
        else if (enemy.gameObject.name == collider.name)
            return true;
        else
            return false;
    }

    void ShotEffect(bool leftSplitShot)
    {
        //Handle muzzleflash effect
        GameObject mFlash = ObjectPooler.sharedInstance.GetPooledObject("PlayerMuzzleFlash");

        if (mFlash != null)
        {
            mFlash.SetActive(true);

            if (!leftSplitShot)
            {
                mFlash.transform.parent = player.transform;
                mFlash.transform.position = muzzlePoint.position;
            }
            else
            {
                mFlash.transform.parent = player.transform;
                mFlash.transform.position = muzzlePointSplit.position;

                FlipFlash(mFlash);
            }
        }

        //Camshake
        StartCoroutine(camShake.VirutalCameraShake(10, .25f));

        if (!player.grounded)
            airKickBack.KickBack();

        FMODUnity.RuntimeManager.PlayOneShot(gunShotFmodEvent, transform.position);
    }

    public void FlipArm(Transform arm, Vector3 newPos)
    {
        lArmFlipped = !lArmFlipped;

        arm.transform.localPosition = newPos;

        Vector3 theScale = arm.transform.localScale;
        theScale.x *= -1;
        arm.transform.localScale = theScale;
        print("Arm flipped");
    }

    void FlipFlash(GameObject flash)
    {
        Vector3 theScale = flash.transform.localScale;
        theScale.x *= -1;
        flash.transform.localScale = theScale;
    }
}
