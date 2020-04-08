using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : AgentController3D
{
    public float jumpTimeInterval = 5;
    public float jumpTimer;

    private void Awake()
    {
        motor = GetComponent<AgentMotor3D>();
    }
    void Update()
    {
        motor.CheckGround();
        motor.FindGroundRotation();
        motor.ApplyLocalGravity();
        if (motor.isGrounded)
        {
            if (currentSpeedData != groundMovement)
            {
                currentSpeedData = groundMovement;
            }

            jumpTimer += Time.deltaTime;

            if (jumpTimer >= jumpTimeInterval)
            {
                motor.BasicJump(basicJumpData);
                jumpTimer = 0;
            }
        }
        else
        {
            if (currentSpeedData != airMovement)
            {
                currentSpeedData = airMovement;
            }
        }
    }
}
