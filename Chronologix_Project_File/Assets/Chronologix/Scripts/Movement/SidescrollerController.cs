using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[RequireComponent(typeof(PlayerInput))]
public class SidescrollerController : AgentController3D
{
    public PlayerAttackSpawner attackData;

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
        motor.ApplyLocalGravity();
        motor.FindGroundRotation();
        motor.CheckGround();
        if (motor.isGrounded)
        {
            currentSpeedData = groundMovement;
        } else
        {
            currentSpeedData = airMovement;
        }

        if (motor.CheckForWall(new Vector3(currentMoveInput.x,0,0).normalized))
        {
            Move(Vector3.zero);
        } else {
            Move(currentMoveInput.normalized);
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
            AnalyticTracker.instance.NPCInteract("attack");
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
