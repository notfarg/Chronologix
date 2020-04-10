using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct Override
{
    [SerializeField]
    public string id;
    [SerializeField]
    public string path;
}

[Serializable]
public struct JsonOverrides
{
    public Override[] overrides;
}
public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public InputActionAsset actions;
    public delegate void OnChangeComplete();
    public OnChangeComplete changeComplete;
    InputActionRebindingExtensions.RebindingOperation rebindOp;
    [SerializeField]
    JsonOverrides overrides;

    private void Awake()
    {
        if (InputManager.instance == null)
        {
            InputManager.instance = this;
            DontDestroyOnLoad(this.gameObject);
            LoadControls();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void SetInput(InputAction actionToRebind)
    {
        rebindOp = actionToRebind.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => changeComplete());

        rebindOp.Start();
        changeComplete += rebindOp.Dispose;
    }

    public void SetAxisInput(InputAction actionToRebind, string selection)
    {
        actionToRebind.Dispose();

        switch (selection)
        {
            case "WASD":
                actionToRebind.AddCompositeBinding("2DVector(mode=2)")
                    .With("up", "<Keyboard>/w")
                    .With("down", "<Keyboard>/s")
                    .With("left", "<Keyboard>/a")
                    .With("right", "<Keyboard>/d");
                break;

            case "Arrow Keys":
                actionToRebind.AddCompositeBinding("2DVector(mode=2)")
                    .With("up", "<Keyboard>/upArrow")
                    .With("down", "<Keyboard>/downArrow")
                    .With("left", "<Keyboard>/leftArrow")
                    .With("right", "<Keyboard>/rightArrow");
                break;

            case "Mouse Delta":
                actionToRebind.AddBinding("<Mouse>/delta");
                break;

            case "Left Stick":
                actionToRebind.AddBinding("<Gamepad>/leftStick");
                break;

            case "RightStick":
                actionToRebind.AddBinding("<Gamepad>/rightStick");
                break;

            default:
                actionToRebind.AddCompositeBinding("2DVector(mode=2)")
                                    .With("up", "<Keyboard>/w")
                                    .With("down", "<Keyboard>/s")
                                    .With("left", "<Keyboard>/a")
                                    .With("right", "<Keyboard>/d");
                break;
        }

        changeComplete();
    }

    public void SaveControls()
    {
        string actionJson = actions.ToJson();
        File.WriteAllText(Application.persistentDataPath + "/Controls.json", actionJson);

        int overrideCount = 0;
        foreach (var map in actions.actionMaps)
            foreach (var binding in map.bindings)
            {
                if (binding.overridePath != null)
                {
                    overrideCount++;
                }
            }
        overrides.overrides = new Override[overrideCount];
        int currentOverrideIndex = 0;
        foreach (var map in actions.actionMaps)
            for (int i = 0; i < map.bindings.Count; i++)
            {
                if (map.bindings[i].overridePath != null)
                {
                    Override newOverride = new Override();
                    newOverride.path = map.bindings[i].overridePath;
                    newOverride.id = map.bindings[i].id.ToString();
                    overrides.overrides[currentOverrideIndex] = newOverride;
                    currentOverrideIndex++;
                }
            }

        File.WriteAllText(Application.persistentDataPath + "/ControlOverrides.json", JsonUtility.ToJson(overrides));
    }

    public void LoadControls()
    {
        string actionJson = File.ReadAllText(Application.persistentDataPath + "/Controls.json");
        actions.LoadFromJson(actionJson);

        string overRideJson = File.ReadAllText(Application.persistentDataPath + "/ControlOverrides.json");
        overrides = JsonUtility.FromJson<JsonOverrides>(overRideJson);

        foreach (var map in actions.actionMaps)
        {
            var bindings = map.bindings;
            for (var i = 0; i < bindings.Count; ++i)
            {
                foreach (Override oride in overrides.overrides)
                {
                    if (oride.id == bindings[i].id.ToString())
                    {
                        map.ApplyBindingOverride(i, new InputBinding { overridePath = oride.path });
                    }
                }

            }
        }
    }
}
