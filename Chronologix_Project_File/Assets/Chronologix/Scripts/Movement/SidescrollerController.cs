using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Spine.Unity;

[RequireComponent(typeof(PlayerInput))]
public class SidescrollerController : AgentController3D
{
    public PlayerAttackSpawner attackData;
    public SkeletonAnimation animationController;
    private void Awake()
    {
        motor.CheckGround();
        if (motor.isGrounded)
        {
            currentSpeedData = groundMovement;
        }
        else
        {
            currentSpeedData = airMovement;
        }
    }
    // Update is called once per frame
    void Update()
    {
        motor.CheckGround();
        motor.FindGroundRotation();
        motor.ApplyLocalGravity();

        if (!motor.isGrounded)
        {
            animationController.AnimationName = "Coze-Jumping";
        }

        if (motor.isGrounded)
        {
            if (currentSpeedData != groundMovement)
            {
                currentSpeedData = groundMovement;
            }
        } else
        {
            if (currentSpeedData != airMovement)
            {
                currentSpeedData = airMovement;
            }
        }

        if (motor.CheckForWall(new Vector3(currentMoveInput.x,0,0).normalized))
        {
            Move(Vector3.zero);
        } else {
            Move(currentMoveInput.normalized);
            if (motor.isGrounded)
            {
                if (currentMoveInput.x > 0)
                {
                    animationController.gameObject.transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                    attackData.facingLeft = false;
                    animationController.AnimationName = "Coze-Running";
                }
                else if (currentMoveInput.x < 0)
                {
                    animationController.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    attackData.facingLeft = true;
                    animationController.AnimationName = "Coze-Running";
                }
                else
                {
                    animationController.AnimationName = "Coze-Idle";
                }
            }
        }
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump();
        }

        if (!AnalyticTracker.instance.playerHasJumped)
        {
            AnalyticTracker.instance.FirstJump();
        }

        if (GameManager.instance.nearNPC)
        {
            AnalyticTracker.instance.NPCInteract("jump");
        }
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        lastInput = currentMoveInput;
        currentMoveInput = new Vector2(context.ReadValue<Vector2>().x, 0);
        if (currentMoveInput.x == 0) {
            float timeToAccel = lastSpeedData.decelTimeFactor * motor.currentMoveSpeed;
            motor.CalcAcceleration(0,currentMoveInput,timeToAccel);
        } else if (lastInput.x == 0)
        {
            float timeToAccel = lastSpeedData.accelTimeFactor * Mathf.Abs(motor.currentMoveSpeed - currentSpeedData.speed);
            motor.CalcAcceleration(currentSpeedData.speed, currentMoveInput, timeToAccel);
        } else if (currentSpeedData.speed != lastSpeedData.speed)
        {
            float timeToAccel = lastSpeedData.swapOutTimeFactor * Mathf.Abs(lastSpeedData.speed - currentSpeedData.speed);
            motor.CalcAcceleration(currentSpeedData.speed, currentMoveInput, timeToAccel);
        }

        if (!AnalyticTracker.instance.playerHasMoved)
        {
            AnalyticTracker.instance.FirstMove();
        }
    }
}
