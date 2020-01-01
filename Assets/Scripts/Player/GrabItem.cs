using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    public Transform AnchorPivot;
    Transform previousAnchorPivot;
    public Transform possibleItem;
    Transform item;
    bool grabbing;

    void Start()
    {
        grabbing=false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Grab")){
            if (!grabbing && possibleItem){
                //Debug.Log("Grab");
                item = possibleItem;
                previousAnchorPivot = item.parent;
                item.SetParent(AnchorPivot);
                item.localPosition = Vector3.zero;
                item.localRotation = Quaternion.identity;
                if (item.GetComponent<Rigidbody>()) item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                foreach (Collider c in item.GetComponents<Collider>())
                {
                    if (!c.isTrigger) c.enabled = false;
                }

                grabbing=true;
            }
            else{
                //Debug.Log("posItem: "+possibleItem);
                //Debug.Log(previousAnchorPivot);
                item.SetParent(previousAnchorPivot);
                foreach (Collider c in item.GetComponents<Collider>())
                {
                    if (!c.isTrigger) c.enabled = true;
                }
                if (item.GetComponent<Rigidbody>()) item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
                //if (previousAnchorPivot) item.SetParent(previousAnchorPivot);
                //else item.DetachChildren();
                grabbing=false;
                Debug.Log("Not grabbing");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag=="Item"){
            possibleItem = other.transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (possibleItem==other.transform){
            possibleItem=null;
        }
    }

    
}
