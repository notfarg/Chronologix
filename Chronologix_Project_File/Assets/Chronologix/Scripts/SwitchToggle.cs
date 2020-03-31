using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchToggle : MonoBehaviour
{
    public bool turnedOn;

    public void Toggle()
    {
        if (!turnedOn)
        {
            turnedOn = true;

            GameManager.instance.numActiveSwitches++;
        }
    }
}
