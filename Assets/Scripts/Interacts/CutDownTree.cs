using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutDownTree : MonoBehaviour
{
    public Transform log;
    Transform cutter;
    Vector3 cutterPos;
    public bool activated;
    string objTag;

    void Start()
    {
        activated=false;
        objTag = this.tag;
    }

    void Update()
    {
        if (activated){
            if (Vector3.Distance(cutter.position, cutterPos)<0.2f){
                if (Input.GetButtonUp("Action")){
                    CutDown();
                }
            }
            else {
                Abort();
            }
        }
    }

    void CutDown(){
        log.parent = null;
        if (log.GetComponent<Rigidbody>()) {
            log.GetComponent<Rigidbody>().isKinematic = false;
            if (cutter) log.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Sign(transform.position.x-cutter.transform.position.x), 0, 0) * 3f, ForceMode.Impulse);
        }
        Destroy(this);
    }

    void Interact(Transform t){
        //Destroy(gameObject);
        Debug.Log("Interact recived");
        activated=true;
        cutter = t;
        cutterPos = cutter.position;
        this.tag = "Untagged";
    }

    void Abort(){
        activated = false;
        this.tag = objTag;
    }
}
