using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour {

    public Animator anim;
    Player player;
    PlayerWeaponSystem wSystem;
    Controller2D controller;

    public RArmAnimHandler rArmAnim;
    public LArmAnimHandler lArmAnim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
        wSystem = GetComponent<PlayerWeaponSystem>();
        controller = GetComponent<Controller2D>();

        rArmAnim = transform.GetChild(2).GetComponent<RArmAnimHandler>();
        lArmAnim = transform.GetChild(3).GetComponent<LArmAnimHandler>();
    }

    private void Update()
    {
        if (!player.reset)
        {
            anim.SetFloat("XVelocity", Mathf.Abs(player.velocity.x));
            anim.SetFloat("YVelocity", player.velocity.y);
            anim.SetBool("DirectionalInput", Mathf.Abs(player.directionalInput.x) > 0 ? true : false);
            anim.SetBool("Grounded", player.controller.collisions.below);
            anim.SetBool("Jumped", controller.state == Controller2D.pState.Jumping ? true : false);

            anim.SetBool("NoAim", wSystem.wState == PlayerWeaponSystem.WeaponState.NoAim ? true : false);
            anim.SetBool("HalfAim", wSystem.wState == PlayerWeaponSystem.WeaponState.HalfAim ? true : false);
            anim.SetBool("FullAim", wSystem.wState == PlayerWeaponSystem.WeaponState.FullAim ? true : false);
            anim.SetBool("SplitAim", wSystem.wState == PlayerWeaponSystem.WeaponState.SplitAim ? true : false);
            anim.SetBool("Shooting", wSystem.lShooting || wSystem.rShooting ? true : false);
            anim.SetBool("WalkBackwards", controller.state == Controller2D.pState.BackwardsWalk ? true : false);
            anim.SetBool("Sliding", controller.state == Controller2D.pState.Sliding ? true : false);
            anim.SetBool("SlideDownSlope", controller.collisions.slidingDownMaxSlope);
            anim.SetBool("Wallslide", player.wallSliding);
        }        

        anim.SetBool("Reset", player.reset);

        if (!player.wallSliding && controller.state != Controller2D.pState.BackwardsWalk && !wSystem.hasBackwardsWalked && !wSystem.inAirShooting)
        {
            
            if (player.velocity.x < 0 && player.dir == Player.Direction.Right)
            {
                //print(wSystem.inAirShooting);
                Flip(false);
            }

            if (player.velocity.x > 0 && player.dir == Player.Direction.Left)
            {
                Flip(false);
                //print(player.state);
            }
            
        }

        if (lArmAnim == null)
        {
            print("This shit equal null");
        }
        
    }

    public void Flip(bool dontFlipChildren)
    {
        if (player.dir == Player.Direction.Left)
            player.dir = Player.Direction.Right;

        else if (player.dir == Player.Direction.Right)
            player.dir = Player.Direction.Left;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        if (dontFlipChildren)
        {
            foreach (Transform child in transform)
            {
                Vector3 childScale = child.transform.localScale;
                childScale.x *= -1;
                child.transform.localScale = childScale;
            }
        }
        
        //print("Player flip" + " were children not flipped?" + dontFlipChildren);
    }

    public void ResetAllAnimParams()
    {
        anim.SetFloat("XVelocity", 0);
        anim.SetFloat("YVelocity", 0);
        anim.SetBool("DirectionalInput", false);
        anim.SetBool("Grounded", false);
        anim.SetBool("Jumped", false);

        anim.SetBool("NoAim", false);
        anim.SetBool("HalfAim", false);
        anim.SetBool("FullAim", false);
        anim.SetBool("SplitAim", false);
        anim.SetBool("Shooting", false);
        anim.SetBool("WalkBackwards", false);

        lArmAnim.ResetAllAnimParams();
        rArmAnim.ResetAllAnimParams();
    }

    public IEnumerator DustLandFX()
    {
        GameObject dustFX = ObjectPooler.sharedInstance.GetPooledObject("DustCloud");
        dustFX.SetActive(true);
        dustFX.transform.position = new Vector3(transform.position.x, transform.position.y - 0.99f, 0);
        dustFX.transform.eulerAngles = new Vector3(-90, 0, 0);
        yield return new WaitForSeconds(2f);
        dustFX.SetActive(false);
    }

    public IEnumerator WallDustFX(int wallDirX)
    {
        GameObject dustFX = ObjectPooler.sharedInstance.GetPooledObject("DustCloud");
        dustFX.SetActive(true);
        dustFX.transform.position = new Vector3(transform.position.x - 0.177f, transform.position.y - 0.618f, 0);
        if (wallDirX == -1)
        {
            dustFX.transform.eulerAngles = new Vector3(-180, -90, 90);
        }
        else
        {
            dustFX.transform.eulerAngles = new Vector3(180, 90, -90);
        }
        yield return new WaitForSeconds(2f);
        dustFX.SetActive(false);
    }
}
