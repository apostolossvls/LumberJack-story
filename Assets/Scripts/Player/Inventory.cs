using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    InteractControl interactControl;
    public Transform[] slots;
    public Transform[] slotsPos;
    public bool inventoryOpen;
    public int slotSelected;
    //bool opening, closing;

    void Start()
    {
        inventoryOpen = false;
        //opening = false;
        //closing = false;
        slotSelected = -1;
        interactControl = GetComponent<InteractControl>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory") && !inventoryOpen){
            OpenInventory();
        }
        if (Input.GetButtonUp("Inventory") && inventoryOpen){
            CloseInventory();
        }
        if (inventoryOpen){
            if (Input.GetAxisRaw("SecondHorizontal")!=0 || Input.GetAxisRaw("SecondVertical")!=0){
                Vector2 inp;
                inp.x = Input.GetAxisRaw("SecondHorizontal");
                inp.y = Input.GetAxisRaw("SecondVertical");
                GetCrossInput(inp.x, inp.y);
            }
        }
    }

    public void SaveToInventory(Transform t){
        int index = -1;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i]==null) index = i;
        }
        if (index>=0){
            SetItemToSlot(t, index);
        }
        else {
            SwapItems(0);
        }
    }

    public void SwapItems(int slot){
        if (interactControl){
            Transform handItem = interactControl.rightGrab;
            Transform slotItem = slots[slot];

            if (handItem){
                interactControl.ReleaseHand(true, false);
                SetItemToSlot(handItem, slot);
            }

            if (slotItem){
                Debug.Log("slot filled");
                Rigidbody r = slotItem.GetComponent<Rigidbody>();
                if (r) r.isKinematic = false;
                slotItem.tag = "Item";
                slotItem.SetParent(null);
                interactControl.GrabItem(slotItem, true);
            }
        }
    }

    void SetItemToSlot(Transform t, int slot){
        t.tag = "Untagged";
        Rigidbody r = t.GetComponent<Rigidbody>();
        if (r) r.isKinematic = true;
        slots[slot] = t;
        t.parent = slotsPos[slot];
        t.localPosition = new Vector3(0,0,0);
        t.localRotation = new Quaternion(0,0,0,0);
    }

    void OpenInventory(){
        inventoryOpen = true;
        if (interactControl) interactControl.enabled = false;
        SelectSlot(-1);
    }
    void CloseInventory(bool forced=false){
        if (!forced){
            if (slotSelected>=0){
                SwapItems(slotSelected);
            }
        }
        inventoryOpen = false;
        if (interactControl) interactControl.enabled = true;
    }

    void SelectSlot(int index){
        slotSelected = index;
    }

    void GetCrossInput(float x, float y){
        Debug.Log("Input second. X: "+x+" , Y: "+y);
        int index = -1;
        if (Mathf.Abs(x) > 0.3f || Mathf.Abs(y) > 0.3f){
            if (-x>y && x<y){
                index = 0;
            }
            else if (x>=-y && x<=y){
                index = 1;
            }
            else if (x>y && -x<y){
                index = 2;
            }
        }
        SelectSlot(index);
    }

    void OnDisable(){
        if (inventoryOpen){
            CloseInventory(true);
        }
    }
}
