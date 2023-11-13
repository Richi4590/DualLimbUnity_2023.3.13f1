using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static LimbInputSystem inputActions;
    public static event Action<string> actionMapChange;

    private void Awake()
    {
        inputActions = new LimbInputSystem();
        inputActions.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        ToggleActionMap(inputActions.Hand);   
    }

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        actionMapChange?.Invoke(actionMap.name);
    }
}
