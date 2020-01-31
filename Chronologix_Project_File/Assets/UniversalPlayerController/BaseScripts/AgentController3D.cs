using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AgentMotor3D))]
public class AgentController3D : MonoBehaviour
{
    public AgentMotor3D motor;

    [SerializeField]
    private Vector2 lastInput;
    public Vector2 currentMoveInput;
    public float groundFrictionFactor;
    public MovementData[] maxSpeeds;
    public MovementData currentSpeedData;
    public MovementData lastSpeedData;
    public JumpData basicJumpData;

    public void Move(Vector2 input)
    {
        // If the max speed hasn't changed
        if (currentSpeedData == lastSpeedData)
        {
            if (lastInput.magnitude <= 0.1f && input.magnitude > 0.1f)
            {
                float timeToAccel = currentSpeedData.accelTimeFactor * Mathf.Abs(currentSpeedData.speed - motor.currentMoveSpeed);
                motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, input.y).normalized, timeToAccel);
            }
            else if (lastInput.magnitude > 0.1f && input.magnitude <= 0.1f)
            {
                float timeToAccel = currentSpeedData.decelTimeFactor * motor.currentMoveSpeed;
                motor.Accelerate(0, new Vector3(input.x, 0, input.y).normalized, timeToAccel);
            }
            else if (lastInput.normalized != input.normalized && input.magnitude > 0.1f && lastInput.magnitude > 0.1f)
            {
                float timeToAccel = currentSpeedData.quickTurnTimeFactor * Mathf.Abs(Vector3.Angle(new Vector3(input.x, 0, input.y).normalized, motor.lastMoveDirection) / 180f);
                motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, input.y).normalized, timeToAccel);
            }
            else
            {
                motor.Accelerate();
            }
        }
        else
        {
            // if the max speed has changed
            if (motor.currentMoveSpeed != currentSpeedData.speed)
            {
                float timeToAccel = currentSpeedData.swapOutTimeFactor * Mathf.Abs(currentSpeedData.speed - motor.currentMoveSpeed);
                motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, input.y).normalized, timeToAccel);
            }
            lastSpeedData = currentSpeedData;
        }
        //update last input value
        lastInput = input;
    }

    public void Jump()
    {
        
        if (motor.isGrounded)
        {
            motor.BasicJump(basicJumpData);
            for (int i = 0; i < maxSpeeds.Length; i++)
            {
                if (maxSpeeds[i].type == MoveType.Air)
                {
                    currentSpeedData = maxSpeeds[i];
                    break;
                }
            }
            motor.isGrounded = false;
        }
    }

}
