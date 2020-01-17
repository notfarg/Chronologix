using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AgentMotor3D))]
public class AgentController3D : MonoBehaviour
{
    public AgentMotor3D motor;
    public Vector2 lastInput;
    public Vector2 currentMoveInput;
    public bool accelStart;
    public bool accelStop;
    public bool accelQuickTurn;
    public bool canAccelOnSwap;
    public float groundFrictionFactor;
    public MovementData[] maxSpeeds;
    public MovementData currentSpeedData;
    public MovementData lastSpeedData;
    public bool canWallJump;
    public bool canBasicJump;
    public bool canChargedJump;
    public bool canMultiJump;
    public JumpData basicJumpData;
    public JumpData minChargedJump;
    public JumpData maxChargedJump;
    public bool chargedJumpTimerRunning;
    public float maxJumpChargeTime;
    public float jumpChargeTimer;
    public JumpData[] multiJumpData;
    public int currentJump;
    public bool canSprint, isSprinting;
    public bool canCrouch, isCrouching;
    public bool canProne, isProne;

    public void Move(Vector2 input)
    {
        // If the max speed hasn't changed
        if (currentSpeedData == lastSpeedData)
        {
            if (lastInput.magnitude <= 0.1f && input.magnitude > 0.1f)
            {
                //if we are starting from no input
                if (accelStart)
                {
                    float timeToAccel = currentSpeedData.accelTimeFactor * Mathf.Abs(currentSpeedData.speed - motor.currentMoveSpeed);
                    motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, input.y).normalized, timeToAccel);
                }
                else
                {
                    motor.BasicMove(new Vector3(input.x, 0, input.y).normalized, currentSpeedData.speed * input.normalized.magnitude);
                }
            }
            else if (lastInput.magnitude > 0.1f && input.magnitude <= 0.1f)
            {
                //if we are stopping from any input
                if (accelStop)
                {
                    float timeToAccel = currentSpeedData.decelTimeFactor * motor.currentMoveSpeed;
                    motor.Accelerate(0, new Vector3(input.x, 0, input.y).normalized, timeToAccel);
                }
                else
                {
                    motor.BasicMove(new Vector3(input.x, 0, input.y).normalized, 0);
                }
            }
            else if (lastInput.normalized != input.normalized && input.magnitude > 0.1f && lastInput.magnitude > 0.1f)
            {
                //if input is changing direction
                if (accelQuickTurn)
                {
                    float timeToAccel = currentSpeedData.quickTurnTimeFactor * Mathf.Abs(Vector3.Angle(new Vector3(input.x, 0, input.y).normalized, motor.lastMoveDirection) / 180f);
                    motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, input.y).normalized, timeToAccel);
                }
                else
                {
                    motor.BasicMove(new Vector3(input.x, 0, input.y).normalized, currentSpeedData.speed * input.normalized.magnitude);
                }
            }
            else
            {
                // if input hasn't changed
                if (accelStop || accelStart || accelQuickTurn)
                {
                    motor.Accelerate();
                }
                else
                {
                    motor.BasicMove(new Vector3(input.x, 0, input.y).normalized, currentSpeedData.speed * input.normalized.magnitude);
                }
            }
        }
        else
        {
            // if the max speed has changed
            if (motor.currentMoveSpeed != currentSpeedData.speed)
            {
                // calc acceleration to the new max speed
                if (canAccelOnSwap)
                {
                    float timeToAccel = currentSpeedData.swapOutTimeFactor * Mathf.Abs(currentSpeedData.speed - motor.currentMoveSpeed);
                    motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, input.y).normalized, timeToAccel);
                }
                else
                {
                    motor.BasicMove(new Vector3(input.x, 0, input.y).normalized, currentSpeedData.speed * input.normalized.magnitude);
                }
            }
            lastSpeedData = currentSpeedData;
        }
        //update last input value
        lastInput = input;
    }

    public void Jump()
    {
        for (int i = 0; i < maxSpeeds.Length; i++)
        {
            if (maxSpeeds[i].type == MoveType.Air)
            {
                currentSpeedData = maxSpeeds[i];
                break;
            }
        }
        if (canBasicJump)
        {
            if (motor.isGrounded || (motor.isTouchingWall && canWallJump))
            {
                motor.BasicJump(basicJumpData);
            }
        }
        if (canChargedJump)
        {
            if (motor.isGrounded || (motor.isTouchingWall && canWallJump))
            {
                // Create a percentage representation of how long the jump button was held
                JumpData finalJump = new JumpData();
                finalJump.height = minChargedJump.height + (maxChargedJump.height - minChargedJump.height) * (Mathf.Min(1, jumpChargeTimer / maxJumpChargeTime));
                finalJump.timeToPeak = minChargedJump.timeToPeak + (maxChargedJump.timeToPeak - minChargedJump.timeToPeak) * (Mathf.Min(1, jumpChargeTimer / maxJumpChargeTime));
                jumpChargeTimer = 0;
                motor.BasicJump(finalJump);
            }
        }

        if (canMultiJump)
        {
            if (currentJump < multiJumpData.Length)
            {
                motor.BasicJump(multiJumpData[currentJump]);
                currentJump++;
            }
        }
    
    motor.isGrounded = false;
    }

}
