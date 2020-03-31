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
    void FixedUpdate()
    {
        motor.CheckGround();
        motor.FindGroundRotation();

        if (!motor.isGrounded)
        {
            motor.ApplyLocalGravity();
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

        if (motor.CheckForWall(motor.groundRotation * new Vector3(currentMoveInput.x,0,0).normalized))
        {
            Move(Vector3.zero);
        } else {
            Move(currentMoveInput.normalized);
            if (motor.isGrounded)
            {
                if (currentMoveInput.x > 0)
                {
                    animationController.gameObject.transform.localScale = new Vector3(-0.5f, 0.5f, 1);
                    animationController.AnimationName = "Coze-Running";
                }
                else if (currentMoveInput.x < 0)
                {
                    animationController.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                    animationController.AnimationName = "Coze-Running";
                }
                else
                {
                    animationController.AnimationName = "Coze-Idle";
                }
            }
        }

        if (currentMoveInput.x > 0)
        {
            attackData.facingLeft = false;
        } else  if (currentMoveInput.x < 0)
        {
            attackData.facingLeft = true;
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
        currentMoveInput = new Vector2(context.ReadValue<Vector2>().x, 0);

        if (!AnalyticTracker.instance.playerHasMoved)
        {
            AnalyticTracker.instance.FirstMove();
        }
    }
}
