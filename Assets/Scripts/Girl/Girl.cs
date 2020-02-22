using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : MonoBehaviour
{
    InteractControl interactControl;
    Inventory inventory;
    

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnInventoryRelease(MessageArgs msg){
        msg.received = true;
        //let release
        //reset
        //GetComponent<Collider>().isTrigger = false;
    }

    void OnInteract(MessageArgs msg){
        //grab
        msg.received = true;
        interactControl = msg.sender.GetComponentInChildren<InteractControl>();
        inventory = msg.sender.GetComponentInChildren<Inventory>();

        inventory.SaveToInventory(transform, 0, true, false);
        gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
        //GetComponent<Collider>().isTrigger = true;
        Debug.Log("girl inventory");
        /*
        if (interactControl.rightGrab != null && inventory.slots[0] != null){
            
        }
        else {

        }
        */
    }
}
