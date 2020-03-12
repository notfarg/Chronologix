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
        // If the max speed hasn't changed
        if (currentSpeedData == lastSpeedData)
        {
            if (lastInput.magnitude <= 0.1f && input.magnitude > 0.1f)
            {
                float timeToAccel = currentSpeedData.accelTimeFactor * Vector3.Distance(input.normalized * currentSpeedData.speed,new Vector3(motor.rBody.velocity.x,0,0));
                motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, 0).normalized, timeToAccel);
            }
            else if (lastInput.magnitude > 0.1f && input.magnitude <= 0.1f)
            {
                float timeToAccel = currentSpeedData.decelTimeFactor * motor.currentMoveSpeed;
                motor.Accelerate(0, new Vector3(input.x, 0, input.y).normalized, timeToAccel);
            }
            else if (lastInput.normalized != input.normalized && input.magnitude > 0.1f && lastInput.magnitude > 0.1f)
            {
                float timeToAccel = currentSpeedData.quickTurnTimeFactor * Mathf.Abs(Vector3.Angle(new Vector3(input.x, 0, 0).normalized, motor.currentMoveDirection) / 180f);
                motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, 0).normalized, timeToAccel);
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
                float timeToAccel = currentSpeedData.swapOutTimeFactor * Vector3.Distance(input.normalized * currentSpeedData.speed, new Vector3(motor.rBody.velocity.x, 0, 0));
                motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, 0).normalized, timeToAccel);
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
            motor.isGrounded = false;
        }
    }

}
