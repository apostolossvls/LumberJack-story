using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    InteractControl interactControl;
    public Transform[] slots;
    public Transform[] slotsPos;
    public bool inventoryOpen;
    public int slotSelected;
    string[] tags = new string[3];
    //bool opening, closing;

    //UI
    public GameObject UIobject;
    public Text[] slotsText;
    public GameObject[] slotHighlight;
    public float inputHoldNeeded = 1f;
    public float inputHold;

    void Start()
    {
        interactControl = GetComponent<InteractControl>();
    }

    void OnEnable(){
        inputHold=0;
        inventoryOpen = false;
        slotSelected = -1;
        RefreshUI();
        UIobject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButton("Inventory") && !inventoryOpen){
            if (inputHold >= inputHoldNeeded){
                OpenInventory();
            }
            inputHold+=Time.deltaTime;
        }
        if (Input.GetButtonUp("Inventory")){
            if (inventoryOpen) {
                CloseInventory();
            }
            inputHold=0;
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

    public void SaveToInventory(Transform t, int slot=-1, bool forced = false, bool OnHand = true){
        Debug.Log("SaveToInventory");
        if (slot < 0){
            int index = -1;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i]==null) {
                    index = i;
                    break;
                }
            }
            if (index>=0){
                SwapItems(index);
            }
            else if (forced){
                SwapItems(0);
            }
        }
        else if (forced || slots[slot] == null) {
            if (OnHand) SwapItems(0);
            else SwapItems(0, t);
        }
    }

    public void SwapItems(int slot , Transform obj = null){
        if (interactControl){
            Transform otherItem;
            if (!obj) otherItem = interactControl.rightGrab;
            else otherItem = obj;
            Transform slotItem = slots[slot];
            string tempTag = tags[slot];

            if (otherItem){
                if (!obj) interactControl.ReleaseHand(true, false);
                SetItemToSlot(otherItem, slot);
                StartCoroutine(ChangePosOverTime(otherItem, slotsPos[slot], 10f));
            }
            else {
                slots[slot] = null;
            }
            if (slotItem){
                Debug.Log("slot filled");
                Rigidbody r = slotItem.GetComponent<Rigidbody>();
                if (r) r.isKinematic = false;
                slotItem.SetParent(null);
                slotItem.tag = (tempTag!=null && tempTag!="") ? tempTag : "Item";
                MessageArgs msg = new MessageArgs(transform);
                slotItem.SendMessage("OnInventoryRelease", msg, SendMessageOptions.DontRequireReceiver);
                if (!msg.received){
                    if (!interactControl.rightHandGrabbing)
                        interactControl.GrabItem(slotItem, true);
                }
            }
        }
    }

    void SetItemToSlot(Transform t, int slot){
        tags[slot] = t.tag;
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
        RefreshUI();
        UIobject.SetActive(true);
    }
    void CloseInventory(bool forced=false){
        if (!forced){
            if (slotSelected>=0){
                SwapItems(slotSelected);
            }
        }
        slotSelected = -1;
        inventoryOpen = false;
        if (interactControl) interactControl.enabled = true;
        UIobject.SetActive(false);
        RefreshUI();
    }

    void SelectSlot(int index){
        slotSelected = index;
    }

    void GetCrossInput(float x, float y){
        //Debug.Log("Input second. X: "+x+" , Y: "+y);
        int index = -1;
        if (Mathf.Abs(x) > 0.3f || Mathf.Abs(y) > 0.3f){
            if (-x>y && x<y){
                index = 1; //left
            }
            else if (x>=-y && x<=y){
                index = 0; //top
            }
            else if (x>y && -x<y){
                index = 2; //right
            }
        }
        SelectSlot(index);
        RefreshHighlightUI();
    }

    //UI
    void RefreshUI(){
        for (int i = 0; i < slotsText.Length; i++)
        {
            string s = "";
            if (slots[i]) s = slots[i].name;
            slotsText[i].text = s;
        }
        RefreshHighlightUI();
    }

    void RefreshHighlightUI(){
        for (int i = 0; i < slotHighlight.Length; i++)
        {
            if (slotSelected==i) slotHighlight[i].SetActive(true);
            else slotHighlight[i].SetActive(false);
        }
    }
    
    private Vector3 posVel = Vector3.zero;
    IEnumerator ChangePosOverTime(Transform t, Transform newT, float speed, int handIndex=0){
        float timer=0;
        while (timer<1/speed){
            t.localRotation = Quaternion.Slerp(t.localRotation, newT.localRotation, Time.deltaTime * speed);
            t.localPosition = Vector3.SmoothDamp(t.localPosition, newT.localPosition, ref posVel, 1/speed);
            timer+=Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        yield return null;
    }

    void OnDisable(){
        if (inventoryOpen){
            CloseInventory(true);
        }
    }
}
