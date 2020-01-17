using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MoveType
{
    Normal,
    Sprint,
    Crouch,
    Prone,
    Air
}

[CreateAssetMenu(fileName = "MovementData", menuName = "ScriptableObjects/MotorData/MovementData", order = 1)]
public class MovementData : ScriptableObject
{

    public MoveType type;
    public float speed;
    public float accelTimeFactor;
    public float decelTimeFactor;
    public float quickTurnTimeFactor;
    public float swapOutTimeFactor;
}

