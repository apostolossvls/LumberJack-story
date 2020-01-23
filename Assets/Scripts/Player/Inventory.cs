using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform[] slots;
    public Transform[] slotsPos;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void SaveToInventory(Transform t){
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i]==null) {
                t.parent = slotsPos[i];
                t.localPosition = new Vector3(0,0,0);
                t.localRotation = new Quaternion(0,0,0,0);
            }
        }
    }

    void GoToIntentory(MessageArgs msg){
        msg.received = true;
        SaveToInventory(msg.sender);
    }
}
