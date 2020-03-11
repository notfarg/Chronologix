using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(PlayerInput))]
public class SidescrollerController : AgentController3D
{
    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ApplyLocalGravity();
        motor.FindGroundRotation();

        if (motor.isGrounded)
        {
            if (currentSpeedData.type == MoveType.Air)
            {
                for (int i = 0; i < maxSpeeds.Length; i++)
                {
                    if (maxSpeeds[i].type == MoveType.Normal)
                    {
                        currentSpeedData = maxSpeeds[i];
                        break;
                    }
                }
            }
        }

        Move(currentMoveInput);
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump();
        }
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        currentMoveInput = new Vector2(context.ReadValue<Vector2>().x, 0);
    }
}
