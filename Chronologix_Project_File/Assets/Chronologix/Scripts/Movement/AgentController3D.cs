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
        motor.Accelerate();
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
