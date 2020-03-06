using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class InputButton : MonoBehaviour
{
    public string device;
    public InputActionReference action;
    public int index;
    public string path;
    [SerializeField]
    public System.Guid id;
    public string stringID;

    void Awake(){
        //InputAction action = null;
        int deviceIndex = -1;
        int actionIndex = -1;
        int bindingIndex = -1;
        //InputMaster.PlayerActions p = InputManager.controls.Player;

        string[] info = gameObject.name.Split('/');
        switch (device)
        {
            case "Keyboard":
                deviceIndex = 0;
                break;
            case "Controller":
                deviceIndex = 1;
                break;
            default:
                break;
        }
        /*
        switch (info[1])
        {
            case "Jump":
                action = p.Jump;
                actionIndex = 8;
                break;
            case "Action":
                action = p.Action;
                actionIndex = 9;
                break;
            case "Interact":
                action = p.Interact;
                actionIndex = 10;
                break;
            case "Release":
                action = p.Release;
                actionIndex = 11;
                break;
            default:
                break;
        }
        
        switch (info[2])
        {
            case "Up":
                bindingIndex = 0;
                break;
            case "Down":
                bindingIndex = 1;
                break;
            case "Left":
                bindingIndex = 2;
                break;
            case "Right":
                bindingIndex = 3;
                break;
            case "Positive":
                bindingIndex = 0;
                break;
            case "Negative":
                bindingIndex = 1;
                break;
            case "1":
                bindingIndex = 0;
                break;
            case "2":
                bindingIndex = 1;
                break;
            default:
                bindingIndex = 1;
                break;
        }
        */

        //if (action == null || deviceIndex == -1 || actionIndex == -1) return;

        /*
        List<InputBinding>[] bindings = new List<InputBinding>[2];
        bindings[0] = new List<InputBinding>{};
        bindings[1] = new List<InputBinding>{};
        for (int i = 0; i < action.ToInputAction().bindings[index].Count; i++)
        {
            /*
            if (action.bindings[i].isComposite) continue;
            //Debug.Log("groups: "+action.bindings[i].groups); //vector2
            if (action.bindings[i].groups=="Keyboard and Mouse")
                bindings[0].Add(action.bindings[i]);
            else if (action.bindings[i].groups=="Gamepad")
                bindings[1].Add(action.bindings[i]);
            
        }
        */
        //Debug.Log("action.ToInputAction().bindings: "+ action.ToInputAction().bindings.Count);
        /*
        if (action.ToInputAction().bindings.Count > index){
            //InputBinding b = action.ToInputAction().bindings[bindingIndex];
            path = action.ToInputAction().bindings[index].overridePath;
            id = action.ToInputAction().bindings[index].id;
            stringID = id.ToString();
        }
        */
    }

    void OnEnable(){
        if (action.ToInputAction().bindings.Count > index){
            //InputBinding b = action.ToInputAction().bindings[bindingIndex];
            path = action.ToInputAction().bindings[index].overridePath;
            id = action.ToInputAction().bindings[index].id;
            stringID = id.ToString();
        }
        InputBinding[] bi = InputManager.controls.Player.Get().bindings.ToArray();
        for (int j = 0; j < bi.Length; j++)
        {
            if (bi[j].id.Equals(id))
            {
                path = bi[j].overridePath;
                break;
            }
        }
        RefreshDisplay();
    }

    public void RefreshDisplay(){
        string p = path!=null && path!=""? path.Substring(path.LastIndexOf("/")+1).ToUpperInvariant() : " ";
        transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = p;

        //GetComponent<Toggle>().interactable = path!="" && path!=null;
    }
}
