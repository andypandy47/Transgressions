using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementIncrease : MonoBehaviour {

    Controller2D controller;
    Player player;

    public float moveSpeedIncreaseAmount = 1f;
    public float groundAccelIncreaseAmount = 0.03f;
    public float airAccelIncreaseAmount = 0.01f;

    private void Start()
    {
        player = GetComponent<Player>();
        controller = GetComponent<Controller2D>();
    }

    public void MoveIncrease(ref float groundAccel, ref float airAccel, ref float moveSpeed, float dirInputX)
    {
        
        if (controller.state != Controller2D.pState.BackwardsWalk)
        {
            if (Mathf.Abs(dirInputX) > 0)
            {
                if (controller.collisions.descendingSlope)
                {
                    groundAccel += groundAccelIncreaseAmount * 100 * Time.deltaTime;
                }
                else
                {
                    moveSpeed += moveSpeedIncreaseAmount * Time.deltaTime;
                    groundAccel += groundAccelIncreaseAmount * Time.deltaTime;
                    airAccel += airAccelIncreaseAmount * Time.deltaTime;
                }
            }
            else
            {
                moveSpeed -= (moveSpeedIncreaseAmount * 4) * Time.deltaTime;
                groundAccel -= (groundAccelIncreaseAmount * 4) * Time.deltaTime;
                airAccel -= (airAccelIncreaseAmount* 4) * Time.deltaTime;
            }
        }
        else
        {
            moveSpeed = 3;
        }

        groundAccel = Mathf.Clamp(groundAccel, 0.1f, 0.5f);
        moveSpeed = Mathf.Clamp(moveSpeed, 6, 9);
        airAccel = Mathf.Clamp(airAccel, 0.2f, 0.5f);
    }


}
