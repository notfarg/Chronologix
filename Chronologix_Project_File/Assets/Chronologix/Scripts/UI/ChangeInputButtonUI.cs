using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class ChangeInputButtonUI : MonoBehaviour
{
    public string inputName;
    public Text buttonText;

    private void Start()
    {
        if (GetComponent<Button>() != null)
        {
            buttonText.text = inputName + " = " + InputManager.instance.actions.FindAction(inputName).GetBindingDisplayString();
        }
    }
    public void ChangingInput()
    {
        GetComponent<Button>().enabled = false;
        buttonText.text = "Press input to assign to " + inputName;
        InputManager.instance.changeComplete += ChangeComplete;
        InputManager.instance.actions.FindAction(inputName).Disable();
        InputManager.instance.SetInput(InputManager.instance.actions.FindAction(inputName));
    }

    public void ChangingAxisInput()
    {
        string selection = GetComponent<Dropdown>().options[GetComponent<Dropdown>().value].text;
        InputManager.instance.changeComplete += ChangeAxisComplete;
        InputManager.instance.actions.FindAction(inputName).Disable();
        InputManager.instance.SetAxisInput(InputManager.instance.actions.FindAction(inputName), selection);
    }

    public void ChangeComplete()
    {
        InputManager.instance.SaveControls();
        GetComponent<Button>().enabled = true;
        InputManager.instance.actions.FindAction(inputName).Enable();
        buttonText.text = inputName + " = " + InputManager.instance.actions.FindAction(inputName).GetBindingDisplayString();
        InputManager.instance.changeComplete -= ChangeComplete;
    }

    public void ChangeAxisComplete()
    {
        InputManager.instance.SaveControls();
        InputManager.instance.actions.FindAction(inputName).Enable();
        InputManager.instance.changeComplete -= ChangeAxisComplete;
    }
}
