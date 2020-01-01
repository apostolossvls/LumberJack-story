using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    public Transform AnchorPivot;
    Transform previousAnchorPivot;
    List<Transform> possibleItem;
    Transform item;
    bool grabbing;

    void Start()
    {
        grabbing=false;
        possibleItem = new List<Transform>{};
    }

    void Update()
    {
        if (Input.GetButtonDown("Grab")){
            Transform pItem=null;
            int index = GetShortDistance();
            if (index!=-1) pItem = possibleItem[index];
            if (!grabbing && pItem){
                //Debug.Log("Grab");
                item = pItem;
                previousAnchorPivot = item.parent;
                //GameObject g = new GameObject();
                //g.name = "itemPivot";
                //g.transform.parent = AnchorPivot.transform;
                //item.transform.parent = g.transform;
                item.parent = AnchorPivot;
                item.localPosition = Vector3.zero;
                item.localRotation = Quaternion.identity;
                if (item.GetComponent<Rigidbody>()) item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                foreach (Collider c in item.GetComponents<Collider>())
                {
                    if (!c.isTrigger) c.enabled = false;
                }

                grabbing=true;
            }
            else if (grabbing){
                //Debug.Log("posItem: "+possibleItem);
                //Debug.Log(previousAnchorPivot);
                item.SetParent(previousAnchorPivot);
                foreach (Collider c in item.GetComponents<Collider>())
                {
                    if (!c.isTrigger) c.enabled = true;
                }
                if (item.GetComponent<Rigidbody>()) item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
                Destroy(AnchorPivot.Find("itemPivot").gameObject);
                // myObjList.Where(x => x.name == yourname).SingleOrDefault();
                //if (previousAnchorPivot) item.SetParent(previousAnchorPivot);
                //else item.DetachChildren();
                grabbing=false;
                Debug.Log("Not grabbing");
            }
        }
    }

    /*void OnTriggerStay(Collider other)
    {
        if (other.tag=="Item"){
            possibleItem = other.transform;
        }
    }*/

    void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Item" && OnArray(other.transform)==-1){
            possibleItem.Add(other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (OnArray(other.transform)!=-1){
            possibleItem.RemoveAt(OnArray(other.transform));
        }
    }

    int OnArray(Transform t){
        for (int i = 0; i < possibleItem.Count; i++)
        {
            if (possibleItem[i]==t) return i;
        }
        return -1;
    }

    int GetShortDistance(){
        int f = -1;
        float dist = 0;
        for (int i = 0; i < possibleItem.Count; i++)
        {
            if (Vector3.Distance(transform.position, possibleItem[i].position)> dist){
                dist = Vector3.Distance(transform.position, possibleItem[i].position);
                f = i;
            }
        }
        return f;
    }
}
