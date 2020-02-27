using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    InputMaster controls;

    void Awake(){
        controls = new InputMaster();
        controls.Player.Any.performed += ctx => AnyKeyPressed(ctx.action.bindings[0]);
    }

    void OnEnable()
    {
        controls.Enable();
    }
    void OnDisable()
    {
        controls.Disable();
    }

    void AnyKeyPressed(InputBinding b){
        KeyCode key = FetchKey();
        Debug.Log("Key: "+key.ToString()+ " , path: "+b.effectivePath);
        string myPath = "<Keyboard>/"+key.ToString();
        try
        {
            b.overridePath = myPath;
            Debug.Log("*Success* - Key: "+key.ToString()+ " , path: "+b.effectivePath);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    KeyCode FetchKey() {
        var e = System.Enum.GetNames(typeof(KeyCode)).Length;
        for(int i = 0; i < e; i++)
        {
            if(Input.GetKey((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }
            
        return KeyCode.None;
    }
}
