using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door1 : MonoBehaviour
{
    public string[] keyNames;
    public float grabPower=1;
    public bool locked = false;
    Rigidbody rig;

    void Start(){
        rig = GetComponent<Rigidbody>();
    }

    void Update(){
        rig.isKinematic = locked;
    }

    void OnInteract(MessageArgs msg){
        msg.received = true;
        GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Sign(msg.sender.position.x - transform.position.x), 0, 0) * grabPower, ForceMode.Impulse);
    }

    void TryKey(Transform obj){
        if (StringMatch(obj.name) && obj.GetComponentInParent<InteractControl>()){
            Unlock();
        }
    }

    void Unlock(){
        locked = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Item"){
            TryKey(collision.transform);
        }
    }

    bool StringMatch (string s){
        foreach (string t in keyNames)
        {
            if (s==t) return true;
        }
        return false;
    }
}
