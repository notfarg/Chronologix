using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchToggle : MonoBehaviour
{
    public UnityEvent toggleSwitch;

    private void Awake()
    {
        if (toggleSwitch == null)
        {
            toggleSwitch = new UnityEvent();
        }

        toggleSwitch.AddListener(Toggle);
    }

    public void Toggle()
    {
        Debug.Log("Switch toggled");
    }
}
