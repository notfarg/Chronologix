using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpData", menuName = "ScriptableObjects/MotorData/JumpData", order = 0)]
public class JumpData : ScriptableObject
{
    public float height;
    public float timeToPeak;
}

