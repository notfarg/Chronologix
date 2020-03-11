using System.Collections;
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
    public bool useSlopeSlowDown;
    public float slopeSlowDownAngleThreshold;
    public float slopeStopSlideAngleThreshold;
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
    [HideInInspector]
    public float accelerationX, accelerationY, accelerationZ, currentAccelerationTime, accelerationTimer;

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
        if (Physics.BoxCastNonAlloc(transform.position + Vector3.up/2, new Vector3(0.4f,0.5f,0.4f), directionOfGravity, boxCastHits, Quaternion.identity, groundingDistance) != 0)
        {
            if (groundingLayers == (groundingLayers | (1 << boxCastHits[0].collider.gameObject.layer)))
            {
                isGrounded = true;
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
        if (Physics.BoxCastNonAlloc(transform.position + Vector3.up * (wallCheckSize.y/2 + 0.1f) + direction.normalized * wallCheckDistance, wallCheckSize / 2, direction, boxCastHits, Quaternion.identity, wallCheckDistance, wallLayers) != 0)
        {
            return true;
        }
        return false;
    }
    public void FindGroundRotation()
    {
        if (isGrounded)
        {
            RaycastHit hit1, hit2, hit3;
            Physics.Raycast(new Ray(transform.position, directionOfGravity), out hit1, Mathf.Infinity, groundingLayers);
            Physics.Raycast(new Ray(transform.position + transform.forward * 0.1f, directionOfGravity), out hit2, Mathf.Infinity, groundingLayers);
            Physics.Raycast(new Ray(transform.position + transform.right * 0.1f, directionOfGravity), out hit3, Mathf.Infinity, groundingLayers);

            if (((hit2.point - transform.position).magnitude <= (hit1.point - transform.position).magnitude * 5) && ((hit3.point - transform.position).magnitude <= (hit1.point - transform.position).magnitude * 5))
            {
                Vector3 vec1 = hit2.point - hit1.point;
                Vector3 vec2 = hit3.point - hit1.point;
                groundNormal = Vector3.Cross(vec1, vec2).normalized;
                groundRotation = Quaternion.FromToRotation(directionOfGravity, groundNormal);
                groundAngle = Mathf.Round(Quaternion.Angle(Quaternion.identity, groundRotation));
            } else
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
        //Apply the jump
        rBody.AddForce(currentVertVelocity, ForceMode.VelocityChange);

    }

    public void ApplyLocalGravity()
    {
        //Apply acceleration to the Rigidbody in the direction of directionGravity at rate localGravity
        rBody.AddForce(localGravity * directionOfGravity, ForceMode.Acceleration);
        FindVertVelocity();
    }

    public void FindVertVelocity()
    {
        if (!isGrounded)
        {
            currentVertVelocity += localGravity * directionOfGravity * Time.fixedDeltaTime;
        }
        else
        {
            currentVertVelocity = Vector3.zero;
        }
    }

    public void Move(Vector3 relativeDirection, float speed)
    {
        //Allows for the isolation of jump and movement speed
        Vector3 currentMove = groundRotation * (currentMoveRotation * (relativeDirection * speed));
        //Based on last horizontal movement, add relevant force to increase horizontal speed
        if (isGrounded && useSlopeSlowDown)
        {
            if (groundAngle <= slopeSlowDownAngleThreshold)
            {
                //if touching the ground and the slope wont require a slow down, proceed as normal along the floor
                rBody.AddForce(currentMove + currentVertVelocity - rBody.velocity, ForceMode.VelocityChange);
            }
            else
            {
                // else, check if we are walking uphill.
                if (Vector3.Angle(groundNormal, currentMove.normalized) < 90)
                {
                    // if the normal is greater than 90 degrees then that means we are walking into the face, aka, uphill
                    if (groundAngle < slopeStopSlideAngleThreshold)
                    {
                        //if still at a walkable slope, reduce speed by a factor according to how steep of an incline and the angle of attack against the slope
                        //factor is represented by angle difference between ground and movement direction compared to the maximum possible (90 + ground angle)
                        //rBody.velocity = currentMove * (Vector3.Angle(groundNormal, currentMove.normalized) / (90 + groundAngle)) + currentVertVelocity;
                        rBody.AddForce(currentMove * (Vector3.Angle(groundNormal, currentMove.normalized) / (90 + groundAngle)) + currentVertVelocity - rBody.velocity, ForceMode.VelocityChange);
                    }
                    else
                    {
                        //if not at a walkable slope, don't move
                        //rBody.velocity = currentVertVelocity;
                        rBody.AddForce(currentVertVelocity - rBody.velocity, ForceMode.VelocityChange);

                    }
                }
                else
                {
                    //if we are walking downhill, we want to SPEED UP downward movement or SLIDE down
                    if (groundAngle < slopeStopSlideAngleThreshold)
                    {
                        //if still at a walkable slope, multiply speed by slope difference, 2* being the max
                        //rBody.velocity = currentMove * (1 + 2 * ((groundAngle - slopeSlowDownAngleThreshold) / (slopeStopSlideAngleThreshold - slopeSlowDownAngleThreshold))) + currentVertVelocity;
                        rBody.AddForce(currentMove * (1 + 2 * ((groundAngle - slopeSlowDownAngleThreshold) / (slopeStopSlideAngleThreshold - slopeSlowDownAngleThreshold))) + currentVertVelocity - rBody.velocity, ForceMode.VelocityChange);

                    }
                    else
                    {
                        //if not at a walkable slope, * 3 movement
                        //rBody.velocity = currentMove * 3 + currentVertVelocity;
                        rBody.AddForce(currentMove * 3 + currentVertVelocity - rBody.velocity, ForceMode.VelocityChange);

                    }
                }
            }
        }
        else
        {
            //rBody.velocity = currentMove + currentVertVelocity;
            rBody.AddForce(currentMove + currentVertVelocity - rBody.velocity, ForceMode.VelocityChange);
        }
    }

    //Accelerated Movement
    public void Accelerate()
    {
        UpdateAccelValues();
        if (accelerationTimer <= currentAccelerationTime)
        {
            //if time is still applicable, adjust
            Vector3 currentVelocity = startMoveDirection * startMoveSpeed + new Vector3(accelerationX, accelerationY, accelerationZ) * accelerationTimer;
            currentMoveSpeed = currentVelocity.magnitude;
            currentMoveDirection = currentVelocity.normalized;
        }
        else
        {
            //force values to expected targets when time is up
            currentMoveSpeed = targetMoveSpeed;
            currentMoveDirection = targetMoveDirection.normalized;
        }

        Move(currentMoveDirection, currentMoveSpeed);

    }

    // To be called every call of Accelerate()
    public void UpdateAccelValues()
    {
        accelerationTimer += Time.fixedDeltaTime;
        currentMoveRotation = rBody.rotation;
    }


    // Accelerate() when a new speed/direction is being considered
    public void Accelerate(float speed, Vector3 targetDir, float time)
    {
        CalcAcceleration(speed, targetDir, time);
        Accelerate();
    }

    // Calculate Acceleratation values
    public void CalcAcceleration(float targetSpd, Vector3 targetDir, float time)
    {
        targetMoveDirection = targetDir.normalized;
        targetMoveSpeed = targetSpd;
        accelerationTimer = 0;
        currentAccelerationTime = time;
        startMoveDirection = currentMoveDirection.normalized;
        startMoveSpeed = currentMoveSpeed;

        AssignAcceleration();
    }

    public void AssignAcceleration()
    {
        Vector3 tempTarget = targetMoveDirection;
        Vector3 tempStart = startMoveDirection;
        accelerationX = (tempTarget.x * targetMoveSpeed - tempStart.x * startMoveSpeed) / currentAccelerationTime;
        accelerationY = (tempTarget.y * targetMoveSpeed - tempStart.y * startMoveSpeed) / currentAccelerationTime;
        accelerationZ = (tempTarget.z * targetMoveSpeed - tempStart.z * startMoveSpeed) / currentAccelerationTime;
    }
}
