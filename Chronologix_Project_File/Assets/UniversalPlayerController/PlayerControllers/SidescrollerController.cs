using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(FixedPerspectiveCamController))]
[RequireComponent(typeof(PlayerInput))]
public class SidescrollerController : AgentController3D
{
    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ApplyLocalGravity();
        motor.CheckForWalls();
        motor.FindGroundRotation();

        if (motor.isGrounded)
        {
            currentJump = 0;
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

        if (motor.isTouchingWall && canWallJump)
        {
            currentJump = 0;
        }

        if (chargedJumpTimerRunning)
        {
            jumpChargeTimer += Time.fixedDeltaTime;
        }

        Move(currentMoveInput);
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (canBasicJump || canMultiJump || canWallJump)
            {
                Jump();
            }
            else if (canChargedJump)
            {
                chargedJumpTimerRunning = true;
            }
        }
        else if (context.canceled)
        {
            if (canChargedJump)
            {
                Jump();
                chargedJumpTimerRunning = false;
                jumpChargeTimer = 0;
            }
        }
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        currentMoveInput = new Vector2(context.ReadValue<Vector2>().x,0);
    }

    public void ToggleSprint(InputAction.CallbackContext context)
    {
        if (canSprint)
        {
            if (context.interaction is PressInteraction && context.started)
            {
                if (isSprinting)
                {
                    for (int i = 0; i < maxSpeeds.Length; i++)
                    {
                        if (maxSpeeds[i].type == MoveType.Normal)
                        {
                            currentSpeedData = maxSpeeds[i];
                            isSprinting = false;
                            break;
                        }
                    }
                }
                if (!isSprinting)
                {
                    for (int i = 0; i < maxSpeeds.Length; i++)
                    {
                        if (maxSpeeds[i].type == MoveType.Sprint)
                        {
                            currentSpeedData = maxSpeeds[i];
                            isSprinting = true;
                            break;
                        }
                    }
                }
            }
            
        }
    }

    public void ToggleCrouchProne(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction && context.performed)
        {
            if (canCrouch)
            {
                if (!isCrouching && !isProne)
                {
                    for (int i = 0; i < maxSpeeds.Length; i++)
                    {
                        if (maxSpeeds[i].type == MoveType.Crouch)
                        {
                            currentSpeedData = maxSpeeds[i];
                            isCrouching = true;
                            break;
                        }
                    }
                }
                else if (!isProne && isCrouching)
                {
                    for (int i = 0; i < maxSpeeds.Length; i++)
                    {
                        if (maxSpeeds[i].type == MoveType.Normal)
                        {
                            currentSpeedData = maxSpeeds[i];
                            isCrouching = false;
                            break;
                        }
                    }
                }
            }
        }
        if (context.interaction is MultiTapInteraction && context.performed)
        {
            if (canProne)
            {
                if (!isProne)
                {
                    for (int i = 0; i < maxSpeeds.Length; i++)
                    {
                        if (maxSpeeds[i].type == MoveType.Prone)
                        {
                            currentSpeedData = maxSpeeds[i];
                            isProne = true;
                            isCrouching = false;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < maxSpeeds.Length; i++)
                    {
                        if (maxSpeeds[i].type == MoveType.Crouch)
                        {
                            currentSpeedData = maxSpeeds[i];
                            isProne = false;
                            break;
                        }
                    }
                }
            }
        }

    }
}
