using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutDownTree : MonoBehaviour
{
    public Transform log;
    Transform cutter;
    Vector3 cutterPos;
    public bool activated;
    public float hitPoints=1f;
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
                    Transform item = cutter.GetComponent<InteractControl>().rightGrab;
                    if (item){
                        if (item.GetComponent<Axe>()){
                            TakeHit(1);
                        }
                        else {
                            Abort();
                        }
                    }
                    else {
                        Abort();
                    }
                }
            }
            else {
                Abort();
            }
        }
    }

    void TakeHit(float damage){
        hitPoints -= damage;
        if (hitPoints <= 0){
            CutDown();
        }
    }

    void CutDown(){
        log.parent = null;
        log.tag = "Grabbable";
        if (log.GetComponent<Rigidbody>()) {
            log.GetComponent<Rigidbody>().isKinematic = false;
            if (cutter) log.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Sign(transform.position.x-cutter.transform.position.x), 0, 0) * 0.5f, ForceMode.Impulse);
        }
        Destroy(this);
    }

    void OnInteract(MessageArgs msg){
        msg.received = true;
        //Destroy(gameObject);
        Debug.Log("Interact recived");
        activated=true;
        cutter = msg.sender;
        cutterPos = cutter.position;
        Attack a = cutter.GetComponent<Attack>();
        if (a) a.enabled = false;
        this.tag = "Untagged";
    }

    void Abort(){
        activated = false;
        Attack a = cutter.GetComponent<Attack>();
        if (a) a.enabled = true;
        this.tag = objTag;
    }
}
