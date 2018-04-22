using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    PlayerWeaponSystem wSystem;
    PlayerAnimHandler pAnimHandler;
    PlayerMovementIncrease moveIncrease;

    public enum Direction
    {
        Right,
        Left,
        Split
    }

    public enum pState
    {
        Stationary,
        Running,
        BackwardsWalk,
        Jumping,
        Faling
    }
    public Direction dir;
    public pState state;

    [HideInInspector] public bool alive;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    public float normalMoveSpeed = 6;
    public float backwardsWalkMoveSpeed = 3;
    float moveSpeed = 6;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 5;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    public float velocityXSmoothing;

    [HideInInspector] public Controller2D controller;

    public Vector3 velocity;
    [HideInInspector] public Vector2 directionalInput;
    [HideInInspector] public bool wallSliding, running, hasHorInput, grounded, reset;
    bool canLand;
    int wallDirX;

    public float test;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        wSystem = GetComponent<PlayerWeaponSystem>();
        pAnimHandler = GetComponent<PlayerAnimHandler>();
        moveIncrease = GetComponent<PlayerMovementIncrease>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        dir = Direction.Right;
        state = pState.Stationary;

        alive = true;
    }

    public IEnumerator FSM()
    {
        while (alive)
        {
            switch (dir)
            {
                case Direction.Right:
                    break;
                case Direction.Left:
                    break;
                case Direction.Split:
                    break;
            }

            switch (state)
            {
                case pState.Stationary:
                    break;
                case pState.Running:
                    break;
                case pState.BackwardsWalk:
                    break;
                case pState.Jumping:
                    break;
                case pState.Faling:
                    break;
            }
            yield return null;
        }
    }

    void Update()
    {

        if (Mathf.Abs(directionalInput.x) > 0)
        {
            hasHorInput = true;
        }
        else
            hasHorInput = false;

        moveIncrease.MoveIncrease(ref accelerationTimeGrounded, ref accelerationTimeAirborne, ref moveSpeed, directionalInput.x);
        CalculateVelocity();
        //HandleMoveSpeed();
        HandleWallSliding();
        SetBools();
        HandleStateLogic();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }


        test = Mathf.Abs(directionalInput.x);

        grounded = controller.collisions.below;

    }

    void HandleStateLogic()
    {
        #region Direction Logic

        if (dir == Direction.Split)
        {
            if (transform.localScale.x == -1)
            {
                pAnimHandler.Flip(false);
            }

            if (directionalInput.x > 0)
            {
                dir = Direction.Right;
            }
            else if (directionalInput.x < 0)
            {
                pAnimHandler.Flip(false);
                dir = Direction.Left;
            }
        }
        else if (dir != Direction.Split && wSystem.wState == PlayerWeaponSystem.WeaponState.SplitAim && !hasHorInput)
        {
            dir = Direction.Split;
        }

        #endregion

        if (grounded && !hasHorInput)
        {
            state = pState.Stationary;
        }
        else if (grounded && velocity.x > 0 && dir == Direction.Right || grounded && velocity.x < 0 && dir == Direction.Left)
        {
            state = pState.Running;
        }
        else if (!grounded && velocity.y > 0)
        {
            state = pState.Jumping;
        }
        else if (velocity.y < 0 && !grounded)
        {
            state = pState.Faling;
           // pAnimHandler.lArmAnim.ResetAllAnimParams();
            //pAnimHandler.rArmAnim.ResetAllAnimParams();
            canLand = true;
        }



        //LANDED
        if (canLand && grounded)
        {
            wSystem.inAirShooting = false;

            canLand = false;
            //print("Landed");
        }
        
    }

    void HandleMoveSpeed()
    {
        if (state == pState.BackwardsWalk)
        {
            moveSpeed = backwardsWalkMoveSpeed;
        }
        else
        {
            moveSpeed = normalMoveSpeed;
        }
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    void SetBools()
    {
        if (controller.collisions.below && Mathf.Abs(directionalInput.x) > 0)
        {
            running = true;
        }
        else
            running = false;

    }

    public void OnJumpInputDown()
    {
        wSystem.wState = PlayerWeaponSystem.WeaponState.NoAim;

        if (dir == Direction.Split)
        {
            dir = Direction.Right;

            if (wSystem.lArmFlipped)
                wSystem.FlipArm(wSystem.LArm, wSystem.normalLeftArmPos);
        }

        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                velocity.y = maxJumpVelocity;
                state = pState.Jumping;
            }
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }


    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        if (Mathf.Abs(velocity.x) < 0.2f)
        {
            velocity.x = 0;
        }
        velocity.y += gravity * Time.deltaTime;
    }

    public void ResetToSpawnPosition(Transform spawnPointPos)
    {
        velocity = Vector3.zero;
        transform.position = spawnPointPos.position;
        moveSpeed = 6;
        accelerationTimeGrounded = 0.1f;

        wSystem.lShooting = false;
        wSystem.rShooting = false;
        wSystem.wState = PlayerWeaponSystem.WeaponState.NoAim;
        pAnimHandler.ResetAllAnimParams();

        reset = true;

        if (!alive)
            alive = true;

        if (dir == Direction.Left)
        {
            pAnimHandler.Flip(false);
        }
        else
        {
            dir = Direction.Right;
        }
        state = pState.Stationary;
    }

}