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
    public GrabCollider grabCollider;
    List<Transform> possibleGrabs;
    string[] grabTags;
    public bool leftHandGrabbing;
    public bool rightHandGrabbing;
    public Transform leftGrab;
    public Transform rightGrab;
    public float grabForce=300f;

    void Start()
    {
        ReleaseHand();
    }

    void Update(){
        possibleGrabs = grabCollider.possibleGrabs;
        grabTags = grabCollider.tags;
        int index = GetShortDistance();
        if (index!=-1 && Input.GetButtonUp("Grab")){
            if (possibleGrabs[index].tag==grabTags[0]){ //item
                //grab item
                Debug.Log("grabbing item (grab control)");
            }
            else if (possibleGrabs[index].tag==grabTags[1]){ //fixedJoint , grabbable
                //grab grabbable
                Debug.Log("grabbing grabbable (grab control)");
            } 
            else if (possibleGrabs[index].tag==grabTags[2]){ //pushdrag , draggable
                //grab draggable
                Debug.Log("grabbing draggable (grab control)");
                ReleaseHand();
                GrabPushDrag(possibleGrabs[index]);
            } 
        }
    }

    int GetShortDistance(){
        int f = -1;
        float dist = 0;
        for (int i = 0; i < grabCollider.possibleGrabs.Count; i++)
        {
            if (Vector3.Distance(transform.position, possibleGrabs[i].position)> dist){
                dist = Vector3.Distance(transform.position, possibleGrabs[i].position);
                f = i;
            }
        }
        return f;
    }

    public void GrabBothHands(Component c, Transform t){
        ReleaseHand();
        leftHandGrabbing = true;
        rightHandGrabbing = true;
        leftGrab = t;
        rightGrab = t;
    }

    void ReleaseHand(bool rLeft = true, bool rRight = true){
        if (rLeft){
            leftHandGrabbing = false;
        }
        if (rRight){
            rightHandGrabbing = false;
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

    void GrabPushDrag(Transform obj){
        //Debug.DrawRay( transform.position, transform.right, Color.black, 1f);
        leftGrab=obj;
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
}
