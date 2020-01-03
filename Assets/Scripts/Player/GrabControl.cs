using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GrabObject
{
    GameObject gameObject;
    Component component;

    GrabObject (GameObject g, Component c){
        gameObject = g;
        component = c;
    }
}

public class GrabControl : MonoBehaviour
{
    public bool IsHuman = true;
    public bool canGrabItem = true;
    public bool canGrabGrabbable = true;
    public bool canGrabDraggable = true;
    public GrabCollider grabCollider;
    public Transform[] HandPivot;
    Transform[] ItemParent = new Transform[2];
    List<Transform> possibleGrabs;
    string[] grabTags;
    public bool leftHandGrabbing;
    public bool rightHandGrabbing;
    public Transform leftGrab;
    public Transform rightGrab;
    public float grabForce=300f;
    RigidbodyConstraints rigidbodyConstraints;

    void Start()
    {
        ReleaseHand();
    }

    void Update(){
        possibleGrabs = grabCollider.possibleGrabs;
        grabTags = grabCollider.tags;
        int index = GetShortDistance();
        if (index!=-1 && Input.GetButtonUp("Grab")){
            if (possibleGrabs[index].tag==grabTags[0] && canGrabItem){ //item
                //grab item
                Debug.Log("grabbing item (grab control)");
                if (IsHuman){
                    if (leftHandGrabbing && rightHandGrabbing){
                        ReleaseHand(true, false);
                        GrabItem(possibleGrabs[index], true);
                    }
                    else if (rightHandGrabbing && !leftHandGrabbing){
                        GrabItem(possibleGrabs[index], false);
                    }
                    else {
                        GrabItem(possibleGrabs[index], true);
                    }
                }
                else {
                    ReleaseHand(true, false);
                    GrabItem(possibleGrabs[index], true);
                }
            }
            else if (possibleGrabs[index].tag==grabTags[1] && IsHuman && canGrabGrabbable){ //fixedJoint , grabbable
                //grab grabbable
                Debug.Log("grabbing grabbable (grab control)");
                ReleaseHand();
                GrabGrabbable(possibleGrabs[index]);
            } 
            else if (possibleGrabs[index].tag==grabTags[2] && IsHuman && canGrabDraggable){ //pushdrag , draggable
                //grab draggable
                Debug.Log("grabbing draggable (grab control)");
                ReleaseHand();
                GrabDraggable(possibleGrabs[index]);
            } 
        }
        if (Input.GetButtonUp("Release")){
            ReleaseHand();
        }
    }

    int GetShortDistance(){
        int f = -1;
        float dist = float.MaxValue;
        for (int i = 0; i < grabCollider.possibleGrabs.Count; i++)
        {
            if (Vector3.Distance(transform.position, possibleGrabs[i].position)< dist && !IsOnHand(possibleGrabs[i])){
                dist = Vector3.Distance(transform.position, possibleGrabs[i].position);
                f = i;
            }
        }
        return f;
    }

    bool IsOnHand(Transform t){
        if (t==rightGrab) return true;
        else if (t==leftGrab) return true;
        else return false;
    }

    void ReleaseHand(bool rRight = true, bool rLeft = true){
        if (rRight && rightHandGrabbing){
            rightHandGrabbing = false;
            if (rightGrab.tag=="Item"){
                ReleaseItem(rightGrab, true);
            }
            else if (rightGrab.tag=="Grabbable"){
                ReleaseGrabbable(rightGrab);
            }
            else if (rightGrab.tag=="Draggable"){
                ReleaseDraggable(rightGrab);
            }
            rightGrab=null;
        }
        if (rLeft && leftHandGrabbing){
            leftHandGrabbing = false;
            if (leftGrab.tag=="Item"){
                ReleaseItem(leftGrab, false);
            }
            else if (leftGrab.tag=="Grabbable"){
                ReleaseGrabbable(leftGrab);
            }
            if (leftGrab.tag=="Draggable"){
                ReleaseDraggable(leftGrab);
            }
            leftGrab=null;
        }

        /*if (script){
            if (script is GrabItem){
                Debug.Log("Script is GrabItem");
                GrabItem c = script as GrabItem;
                c.ResetHodling(); 
            }
            else if (script is GrabFixedJoint){
                Debug.Log("Script is GrabFixedJoint");
                GrabFixedJoint c = script as GrabFixedJoint;
                c.ResetHolding(); 
            }
            else if (script is PushDrag){
                Debug.Log("Script is PushDrag");
                PushDrag c = script as PushDrag;
                c.ResetHolding(); 
            }
            else {
                Debug.Log("Script error on GrabControl");
            }
        }
        script = null;*/
    }

    /*void DropItem(){
        //Debug.Log("posItem: "+possibleItem);
        //Debug.Log(previousAnchorPivot);
        item.SetParent(previousAnchorPivot);
        foreach (Collider c in item.GetComponents<Collider>())
        {
            if (!c.isTrigger) c.enabled = true;
        }
        if (item.GetComponent<Rigidbody>()) item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        //Destroy(AnchorPivot.Find("itemPivot").gameObject);
        // myObjList.Where(x => x.name == yourname).SingleOrDefault();
        //if (previousAnchorPivot) item.SetParent(previousAnchorPivot);
        //else item.DetachChildren();
        grabbing=false;
        Debug.Log("Not grabbing");
    }
    */

    void GrabItem(Transform obj, bool OnRight){
        int hand=0;
        if (OnRight){
            rightHandGrabbing=true;
            rightGrab=obj;
            hand=0;
        }
        else{
            leftHandGrabbing=true;
            leftGrab=obj;
            hand=1;
        }

        //Debug.Log("Grab");
        ItemParent[hand] = obj.parent;
        //GameObject g = new GameObject();
        //g.name = "itemPivot";
        //g.transform.parent = AnchorPivot.transform;
        //item.transform.parent = g.transform;
        obj.parent = HandPivot[hand];
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        if (obj.GetComponent<Rigidbody>()) obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        foreach (Collider c in obj.GetComponents<Collider>())
        {
            if (!c.isTrigger) c.enabled = false;
        }
    }

    void GrabGrabbable(Transform obj){
        rightHandGrabbing=true;
        leftHandGrabbing=true;
        rightGrab=obj;
        leftGrab=obj;

        //Debug.DrawRay( transform.position, transform.right, Color.black, 1f);
        FixedJoint fj = obj.gameObject.AddComponent<FixedJoint>();
        fj.breakForce = grabForce;
        fj.connectedBody = GetComponent<Rigidbody>();
        if (obj.GetComponent<Rigidbody>()){
            //m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
            rigidbodyConstraints = obj.GetComponent<Rigidbody>().constraints;
            //holdingGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    void GrabDraggable(Transform obj){
        rightHandGrabbing=true;
        leftHandGrabbing=true;
        rightGrab=obj;
        leftGrab=obj;

        //Debug.DrawRay( transform.position, transform.right, Color.black, 1f);
        SpringJoint sj = obj.gameObject.AddComponent<SpringJoint>();
        sj.spring = 50;
        sj.damper = 10;
        //holding.maxDistance = 0.3f;
        sj.tolerance = 2f;
        sj.enableCollision = true;
        sj.breakForce = grabForce;
        sj.connectedBody = GetComponent<Rigidbody>();
        /*if (holdingGameObject.GetComponent<Rigidbody>()){
            //m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
            rigidbodyConstraints = holdingGameObject.GetComponent<Rigidbody>().constraints;
            //holdingGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }*/

        if (GetComponent<Jump>()) GetComponent<Jump>().enabled =false;
    }

    void ReleaseItem(Transform t, bool OnRight){
        //Debug.Log("posItem: "+possibleItem);
        //Debug.Log(previousAnchorPivot);
        t.SetParent(ItemParent[OnRight? 0 : 1]);
        foreach (Collider c in t.GetComponents<Collider>())
        {
            if (!c.isTrigger) c.enabled = true;
        }
        if (t.GetComponent<Rigidbody>()) t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        //Destroy(AnchorPivot.Find("itemPivot").gameObject);
        // myObjList.Where(x => x.name == yourname).SingleOrDefault();
        //if (previousAnchorPivot) item.SetParent(previousAnchorPivot);
        //else item.DetachChildren();
    }

    void ReleaseGrabbable(Transform t){
        Destroy(t.GetComponent<FixedJoint>());
        //if (holdingGameObject.GetComponent<Rigidbody>()) holdingGameObject.GetComponent<Rigidbody>().constraints = rigidbodyConstraints;
        if (t.position.z!=0) 
            t.position = new Vector3(t.position.x, t.position.y, 0);
    }

    void ReleaseDraggable(Transform t){
        Destroy(t.GetComponent<SpringJoint>());
        //if (holdingGameObject.GetComponent<Rigidbody>()) holdingGameObject.GetComponent<Rigidbody>().constraints = rigidbodyConstraints;
        if (t.position.z!=0) 
            t.position = new Vector3(t.position.x, t.position.y, 0);
        
        if (GetComponent<Jump>()) GetComponent<Jump>().enabled =true;
    }
}
