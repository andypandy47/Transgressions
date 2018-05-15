using UnityEngine;
using System.Collections;

public class Controller2D : RaycastController
{
    Player player;
    PlayerWeaponSystem wSystem;
    PlayerAnimHandler pAnimHandler;

    public enum pState
    {
        Stationary,
        Running,
        BackwardsWalk,
        Jumping,
        Faling,
        Sliding
    }
    public pState state;

    public float maxSlopeAngle = 80; //TODO: set to zero so player will gradually slide on all slopes
    public float slopeSpeed = 1; //add on to moveDistance variables in sliding functions allowing for control over speed on the slopes
    public float slopeSpeedIncrease;
    public float slopeSpeedDivisor;
    public float slopeIdleSlideSpeed = 1;
    float slopeDescentXVelocitySmoothing;
    float slopeDescentYVelocitySmoothing;
    public Vector2 slopeDescentVelocity;
    public Vector2 slopeAscentVelocity;
    public Vector2 slopeLaunchAmount;
    public Vector2 moveAmount;

    public CollisionInfo collisions;
    [HideInInspector]
    public Vector2 playerInput;

    bool canLand;
    bool grounded;
    bool canLaunchOffSlope;

    public IEnumerator FSM()
    {
        while (player.alive)
        {
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

    public override void Start()
    {
        base.Start();
        collisions.faceDir = 1;
        player = GetComponent<Player>();
        wSystem = GetComponent<PlayerWeaponSystem>();
        pAnimHandler = GetComponent<PlayerAnimHandler>();
        slopeDescentVelocity = Vector2.zero;
        state = pState.Stationary;

    }

    public void Move(Vector2 moveAmount, bool standingOnPlatform)
    {
        if (!PauseMenu.isPaused)
            Move(moveAmount, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false)
    {
        grounded = collisions.below;
        UpdateRaycastOrigins();
        HandleSlopeVelocityEffect(ref moveAmount, input);

        collisions.Reset(input.x);
        collisions.moveAmountOld = moveAmount;
        playerInput = input;

        if (moveAmount.y < 0)
        {
            DescendSlope(ref moveAmount, input.x);
        }

        if (moveAmount.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
        }

        HorizontalCollisions(ref moveAmount, input);
        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }
        HandleStateLogic(input, moveAmount);

        transform.Translate(moveAmount);
        this.moveAmount = moveAmount;

        if (standingOnPlatform)
        {
            collisions.below = true;
        }

        

        if (collisions.climbingSlope)
        {

        }
    }

    void HandleStateLogic(Vector2 input, Vector2 moveAmount)
    {
        if (grounded && Mathf.Abs(input.x) == 0 && moveAmount.x == 0 && !collisions.slidingDownMaxSlope)
        {
            state = pState.Stationary;
        }
        else if (grounded && moveAmount.x > 0 && player.dir == Player.Direction.Right && input.x > 0 || grounded && moveAmount.x < 0 && player.dir == Player.Direction.Left && input.x < 0)
        {
            state = pState.Running;
        }
        else if (grounded && moveAmount.x > 0 && player.dir == Player.Direction.Right && input.x < 1 && !collisions.climbingSlope 
            || grounded && moveAmount.x < 0 && player.dir == Player.Direction.Left && input.x > -1 && !collisions.climbingSlope || collisions.slidingDownMaxSlope)
        {
            state = pState.Sliding;
        }
        else if (!grounded && moveAmount.y > 0 && !collisions.climbingSlope)
        {
            state = pState.Jumping;
        }
        else if (moveAmount.y < 0 && !grounded && !collisions.slidingDownMaxSlope && !collisions.climbingSlope && !collisions.descendingSlope)
        {
            state = pState.Faling;

            canLand = true;
        }

        if (canLand && grounded)
        {
            StartCoroutine(pAnimHandler.DustLandFX());
            wSystem.inAirShooting = false;
            canLand = false;
            
        }
    }

    void HorizontalCollisions(ref Vector2 moveAmount, Vector2 input)
    {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if (Mathf.Abs(moveAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {

                if (hit.distance == 0)
                {
                    print(hit.collider.name);
                    continue;
                }

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }

                    ClimbSlope(ref moveAmount, slopeAngle, hit.normal, input);
                    moveAmount.x += distanceToSlopeStart * directionX;
                    //print("Climb slope please");
                }

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;
                    if (collisions.climbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
            //else if (hit == false)
               // print("Hit = false");
        }
    }

    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                if (hit.collider.tag == "Through")
                {
                    if (directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if (collisions.fallingThroughPlatform)
                    {
                        continue;
                    }
                    if (playerInput.y == -1)
                    {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .5f);
                        continue;
                    }
                }

                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
                
            }
        }
    }

    void ClimbSlope(ref Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal, Vector2 input)
    {
        float moveDistance = Mathf.Abs(moveAmount.x) * slopeSpeed;
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= climbmoveAmountY)
        {
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);

            slopeAscentVelocity = new Vector2(moveAmount.x, moveAmount.y);
            //print("Climbing a slope");

            collisions.below = true;
            if (input.x != 0)
                collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
            collisions.slopeNormal = slopeNormal;
        }
    }

    void DescendSlope(ref Vector2 moveAmount, float xInput)
    {
        RaycastHit2D maxSlopeHitLeft = Physics2D.Raycast(raycastOrigins.bottomLeft, Vector2.down, Mathf.Abs(moveAmount.y) + skinWidth, collisionMask);
        RaycastHit2D maxSlopeHitRight = Physics2D.Raycast(raycastOrigins.bottomRight, Vector2.down, Mathf.Abs(moveAmount.y) + skinWidth, collisionMask);
        if (maxSlopeHitLeft ^ maxSlopeHitRight && Mathf.Abs(xInput) == 0)
        {
            SlideDownMaxSlope(maxSlopeHitLeft, ref moveAmount);
            SlideDownMaxSlope(maxSlopeHitRight, ref moveAmount);
        }
        else
            collisions.slidingDownMaxSlope = false;

        if (!collisions.slidingDownMaxSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                slopeSpeedIncrease = slopeAngle / slopeSpeedDivisor;

                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {
                        if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                        {
                            //slopeSpeed += slopeSpeedIncrease * Time.deltaTime;
                            float moveDistance = Mathf.Abs(moveAmount.x) * slopeSpeed;
                            float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                            moveAmount.y -= descendmoveAmountY;

                            slopeDescentVelocity = new Vector3(moveAmount.x, moveAmount.y);
                           // print(Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x));

                            collisions.slopeAngle = slopeAngle;
                            collisions.descendingSlope = true;
                            collisions.below = true;
                            collisions.slopeNormal = hit.normal;
                        }
                    }
                }
            }
        }
    }

    void SlideDownMaxSlope(RaycastHit2D hit, ref Vector2 moveAmount)
    {
        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            //slopeIdleSlideSpeed = slopeAngle / 30;
            if (slopeAngle > maxSlopeAngle)
            {
                moveAmount.x = Mathf.Sign(hit.normal.x) * (Mathf.Abs(moveAmount.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                collisions.slopeAngle = slopeAngle;
                collisions.slidingDownMaxSlope = true;
                collisions.slopeNormal = hit.normal;
            }
        }

    }

    void HandleSlopeVelocityEffect(ref Vector2 moveAmount, Vector2 input)
    {
        int dirX = collisions.faceDir;

        slopeSpeed = Mathf.Clamp(slopeSpeed, 1, 10);
        
    }

    void ResetFallingThroughPlatform()
    {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public bool slidingDownMaxSlope;

        public float slopeAngle, slopeAngleOld;
        public Vector2 slopeNormal;
        public Vector2 moveAmountOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset(float xInput)
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            //slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    public void ResetVariables()
    {
        slopeSpeed = 1;
        slopeSpeedIncrease = 0;
        slopeDescentVelocity = Vector2.zero;
        slopeAscentVelocity = Vector2.zero;
        canLaunchOffSlope = false;
    }

}