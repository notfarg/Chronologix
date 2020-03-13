using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AgentMotor3D))]
public class AgentController3D : MonoBehaviour
{

    [SerializeField]
    protected Vector2 lastInput;
    [SerializeField]
    protected AgentMotor3D motor;
    [SerializeField]
    protected Vector2 currentMoveInput;
    [SerializeField]
    protected float groundFrictionFactor;
    [SerializeField]
    protected MovementData groundMovement;
    [SerializeField]
    protected MovementData airMovement;
    [SerializeField]
    protected MovementData currentSpeedData;
    [SerializeField]
    protected MovementData lastSpeedData;
    [SerializeField]
    protected JumpData basicJumpData;

    public void Move(Vector2 input)
    {
        if (currentSpeedData == lastSpeedData)
        {
            if (motor.currentMoveSpeed != currentSpeedData.speed)
            {
                if (lastInput != input)
                {
                    if (input.magnitude < 0.1f)
                    {
                        float timeToAccel = currentSpeedData.decelTimeFactor * motor.currentMoveSpeed;
                        motor.Accelerate(0, input.normalized, timeToAccel);
                    }
                    else if (lastInput.magnitude < 0.1f)
                    {
                        float timeToAccel = currentSpeedData.accelTimeFactor * Mathf.Abs(motor.currentMoveSpeed - currentSpeedData.speed);
                        motor.Accelerate(currentSpeedData.speed, input.normalized, timeToAccel);
                    }
                    else
                    {
                        float timeToAccel = currentSpeedData.quickTurnTimeFactor * Mathf.Abs(Vector2.Angle(input, lastInput) / 180);
                        motor.Accelerate(currentSpeedData.speed, input.normalized, timeToAccel);
                    }
                }
                else
                {
                    motor.Accelerate();
                }
            }
            else
            {
                motor.Accelerate();
            }
        }
        else
        {
            float timeToAccel = currentSpeedData.swapOutTimeFactor * Mathf.Abs(motor.currentMoveSpeed - currentSpeedData.speed);
            motor.Accelerate(currentSpeedData.speed, input.normalized, timeToAccel);
            lastSpeedData = currentSpeedData;
        }
        lastInput = input;
    }

    public void Jump()
    {
        
        if (motor.isGrounded)
        {
            motor.BasicJump(basicJumpData);
            motor.isGrounded = false;
        }
    }

}
