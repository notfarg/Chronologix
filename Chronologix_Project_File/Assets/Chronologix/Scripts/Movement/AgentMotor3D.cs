﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AgentMotor3D : MonoBehaviour
{
    // Mandatory
    public Rigidbody rBody;
    public Vector3 directionOfGravity;

    //Ground Detection
    public bool isGrounded;
    public float groundingDistance;
    public LayerMask groundingLayers;

    //Slopes
    public Quaternion groundRotation;
    public Vector3 groundNormal;
    public float groundAngle;

    //Basic Movement
    public float currentMoveSpeed;
    public Vector3 currentMoveDirection;

    //Acceleration
    [HideInInspector]
    public float targetMoveSpeed, startMoveSpeed, lastMoveSpeed;
    [HideInInspector]
    public Vector3 targetMoveDirection, startMoveDirection, lastMoveDirection;
    [HideInInspector]
    public Quaternion currentMoveRotation;
    public float accelerationX;
    public float accelerationY;
    public float accelerationZ;
    public float currentAccelerationTime, accelerationTimer;

    //Gravity and Jumping
    public float localGravity;
    public Vector3 currentVertVelocity;
    public float wallCheckDistance;
    public Vector3 wallCheckSize;
    public bool wallFound;
    public LayerMask wallLayers;
    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public void CheckGround()
    {
        //Look in direction of directionGravity for any objects in groundingLayer layer within groundingDistance units. If an object is found, isGrounded is true, else, false
        RaycastHit[] boxCastHits = new RaycastHit[1];
        if (Physics.BoxCastNonAlloc(transform.position + Vector3.up / 2, new Vector3(0.4f, 0.5f, 0.4f), directionOfGravity, boxCastHits, Quaternion.identity, groundingDistance) != 0)
        {
            if (groundingLayers == (groundingLayers | (1 << boxCastHits[0].collider.gameObject.layer)))
            {
                isGrounded = true;
                rBody.velocity = new Vector3(rBody.velocity.x, 0, 0);
            }
            else
            {
                isGrounded = false;
                groundRotation = Quaternion.identity;
                groundAngle = 0;
            }
        }
        else
        {
            isGrounded = false;
            groundRotation = Quaternion.identity;
            groundAngle = 0;
        }
    }


    public bool CheckForWall(Vector3 direction)
    {
        RaycastHit[] boxCastHits = new RaycastHit[1];
        if (Physics.BoxCastNonAlloc(transform.position + Vector3.up * (wallCheckSize.y / 2 + 1f) + direction.normalized * wallCheckDistance, wallCheckSize / 2, direction, boxCastHits, groundRotation, wallCheckDistance, wallLayers) != 0)
        {
            wallFound = true;
            return wallFound;
        }
        wallFound = false;
        return wallFound;
    }
    public void FindGroundRotation()
    {
        if (isGrounded)
        {
            RaycastHit hit1, hit2, hit3;
            Physics.Raycast(new Ray(transform.position, directionOfGravity), out hit1, Mathf.Infinity, groundingLayers);
            Physics.Raycast(new Ray(transform.position + transform.right * 0.1f, directionOfGravity), out hit2, Mathf.Infinity, groundingLayers);
            Physics.Raycast(new Ray(transform.position + transform.forward * 0.1f, directionOfGravity), out hit3, Mathf.Infinity, groundingLayers);

            if (((hit2.point - transform.position).magnitude <= (hit1.point - transform.position).magnitude * 5) && ((hit3.point - transform.position).magnitude <= (hit1.point - transform.position).magnitude * 5))
            {
                Vector3 vec1 = hit2.point - hit1.point;
                Vector3 vec2 = hit3.point - hit1.point;
                groundNormal = Vector3.Cross(vec1, vec2).normalized;
                groundRotation = Quaternion.FromToRotation(directionOfGravity, groundNormal);
                groundAngle = Mathf.Round(Quaternion.Angle(Quaternion.identity, groundRotation));
            }
            else
            {
                groundNormal = -directionOfGravity;
                groundRotation = Quaternion.identity;
                groundAngle = 0f;
            }
        }
        else
        {
            groundNormal = -directionOfGravity;
            groundRotation = Quaternion.identity;
            groundAngle = 0f;
        }
    }

    public void BasicJump(JumpData jump)
    {
        //Force the Rigidbody to move in opposite direction of directionGravity at initialVelocity
        localGravity = 2 * jump.height / (Mathf.Pow((jump.timeToPeak), 2));
        float initialJumpVelocity = localGravity * (jump.timeToPeak);
        currentVertVelocity = initialJumpVelocity * -directionOfGravity.normalized;
        rBody.AddForce(initialJumpVelocity * Vector3.up, ForceMode.VelocityChange);
    }

    public void ApplyLocalGravity()
    {
        //Apply acceleration to the Rigidbody in the direction of directionGravity at rate localGravity
        if (!isGrounded)
        {
            rBody.AddForce(localGravity * Vector3.down, ForceMode.Acceleration);
            currentVertVelocity += localGravity * Vector3.down * Time.deltaTime;
        }
        else
        {
            currentVertVelocity = Vector3.zero;
        }
    }

    public void Accelerate()
    {
        
        if (accelerationTimer < currentAccelerationTime)
        {
            rBody.AddForce(groundRotation * (accelerationX * targetMoveDirection), ForceMode.Acceleration);
        } else
        {
            Vector3 currentVec = rBody.velocity - currentVertVelocity;
            Vector3 targetVec = groundRotation * (targetMoveSpeed * targetMoveDirection);
            Vector3 difference = (targetVec - currentVec)/Time.deltaTime;
            rBody.AddForce(difference, ForceMode.Acceleration);
        }
        UpdateAccelValues();
    }

    // To be called every call of Accelerate()
    public void UpdateAccelValues()
    {
        if (accelerationTimer <= currentAccelerationTime)
        {
            accelerationTimer += Time.deltaTime;
        }
        currentMoveDirection = targetMoveDirection;
        currentMoveSpeed = (rBody.velocity - currentVertVelocity).magnitude;
    }

    // Calculate Acceleratation values
    public void CalcAcceleration(float targetSpd, Vector3 targetDir, float time)
    {
        if (targetDir != Vector3.zero)
        {
            targetMoveDirection = targetDir.normalized;
        }
        targetMoveSpeed = targetSpd;
        currentAccelerationTime = time;
        accelerationTimer = 0;
        accelerationX = (targetMoveSpeed - currentMoveSpeed) / currentAccelerationTime;
    }

}
