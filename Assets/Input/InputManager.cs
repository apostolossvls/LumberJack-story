using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    InputMaster controls;

    void Awake()
    {
        controls = new InputMaster();
        //controls.Player.Any.performed += ctx => AnyKeyPressed(ctx.action.bindings[0]);
        controls.Player.Interact.performed += ctx => PressInteract();

        /*
        InputSystem.onEvent +=
        (eventPtr, device) =>
        {
            //if (!eventPtr.IsA<InputAction.>() && !eventPtr.IsA<DeltaStateEvent>())
            //    return;
            var con = device.allControls;
            var buttonPressPoint = InputSystem.settings.defaultButtonPressPoint;
            for (var i = 0; i < con.Count; ++i)
            {
                var control = con[i] as ButtonControl;
                if (control == null || control.synthetic || control.noisy)
                    continue;
                if (control.ReadValueFromEvent(eventPtr, out var value) && value >= buttonPressPoint)
                {
                    break;
                }
            }
        };
        */
    }

    void OnEnable()
    {
        controls.Enable();
    }
    void OnDisable()
    {
        controls.Disable();
    }

    void AnyKeyPressed(InputBinding b)
    {
        //InputActionRebindingExtensions.RebindingOperation
        controls.Player.Interact.Disable();
        controls.Player.Interact.PerformInteractiveRebinding().Start();
        controls.Player.Interact.Enable();
        Debug.Log("interact key path: "+ controls.Player.Interact.bindings[0].effectivePath);
        /*
        KeyCode key = FetchKey();
        Debug.Log("Key: " + key.ToString() + " , path: " + b.effectivePath);
        string myPath = "<Keyboard>/" + key.ToString().ToLower();
        try
        {
            ChangeBindingPath(controls.Player.Interact.bindings[0], myPath);
            Debug.Log("*Success* - Key: " + key.ToString() + " , path: " + controls.Player.Interact.bindings[0].effectivePath);
        }
        catch (System.Exception)
        {
            Debug.LogWarning("Couldn't bind key: " + key.ToString());
            throw;
        }
        */
    }

    void ChangeBindingPath(InputBinding b, string s){
        b.path = s;
        b.overridePath = s;
    }   

    void PressInteract(){
        Debug.Log("Interact pressed");
    }

    KeyCode FetchKey()
    {
        var e = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < e; i++)
        {
            if (Input.GetKey((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }
}
