using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementIncrease : MonoBehaviour {

    Controller2D controller;
    Player player;

    public float moveSpeedIncreaseAmount = 1f;
    public float groundAccelIncreaseAmount = 0.03f;
    public float airAccelIncreaseAmount = 0.01f;
    public float jumpIncreaseAmount = 0.3f;

    private void Start()
    {
        player = GetComponent<Player>();
        controller = GetComponent<Controller2D>();
    }

    public void MoveIncrease(ref float groundAccel, ref float airAccel, ref float moveSpeed, ref float maxJumpHeight, ref float timeToJumpApex, float dirInputX)
    {
        
        if (controller.state != Controller2D.pState.BackwardsWalk)
        {
            if (Mathf.Abs(dirInputX) > 0)
            {
                if (controller.collisions.descendingSlope)
                {
                    groundAccel += groundAccelIncreaseAmount * 100 * Time.deltaTime;
                    moveSpeed += (moveSpeedIncreaseAmount * 2f) * Time.deltaTime;
                    maxJumpHeight += (jumpIncreaseAmount * 2f) * Time.deltaTime;
                    //print("descending slope");
                }
                else
                {
                    moveSpeed += moveSpeedIncreaseAmount * Time.deltaTime;
                    groundAccel += groundAccelIncreaseAmount * Time.deltaTime;
                    airAccel += airAccelIncreaseAmount * Time.deltaTime;
                    maxJumpHeight += jumpIncreaseAmount * Time.deltaTime;
                }
            }
            else
            {
                moveSpeed -= (moveSpeedIncreaseAmount * 4) * Time.deltaTime;
                groundAccel -= (groundAccelIncreaseAmount * 4) * Time.deltaTime;
                airAccel -= (airAccelIncreaseAmount* 4) * Time.deltaTime;
                maxJumpHeight -= (jumpIncreaseAmount * 4) * Time.deltaTime;
            }
        }
        else
        {
            moveSpeed = 3;
        }

        groundAccel = Mathf.Clamp(groundAccel, 0.1f, 0.5f);
        moveSpeed = Mathf.Clamp(moveSpeed, 6, 11);
        airAccel = Mathf.Clamp(airAccel, 0.2f, 0.5f);
        maxJumpHeight = Mathf.Clamp(maxJumpHeight, 4, 9);
        timeToJumpApex = maxJumpHeight * 0.125f;
    }


}
