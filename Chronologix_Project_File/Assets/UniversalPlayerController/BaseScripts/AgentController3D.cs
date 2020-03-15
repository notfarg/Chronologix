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
        if (lastSpeedData.speed != currentSpeedData.speed)
        {
            float timeToAccel = lastSpeedData.swapOutTimeFactor * Mathf.Abs(lastSpeedData.speed - currentSpeedData.speed);
            motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, input.y), timeToAccel);
        }
        else if ((motor.currentMoveSpeed < (new Vector3(input.x, 0, input.y).normalized * currentSpeedData.speed).magnitude) && (input != Vector2.zero && input != lastInput))
        {
            float timeToAccel = lastSpeedData.accelTimeFactor * Mathf.Abs(motor.rBody.velocity.x - input.x * currentSpeedData.speed);
            motor.Accelerate(currentSpeedData.speed, new Vector3(input.x, 0, input.y), timeToAccel);
        }
        else if ((motor.currentMoveSpeed > 0) && (input == Vector2.zero && input != lastInput))
        {
            float timeToAccel = lastSpeedData.decelTimeFactor * motor.currentMoveSpeed;
            motor.Accelerate(0, new Vector3(input.x, 0, input.y), timeToAccel);
        }
        else
        {
            motor.Accelerate();
        }

        lastSpeedData = currentSpeedData;
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
