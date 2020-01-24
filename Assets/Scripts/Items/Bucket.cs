using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    bool filled;
    public GameObject contentPrefab;

    void Start(){
        filled = true;
    }

    bool ThrowContent(Transform t){
        bool flag = false;
        if (filled){
            filled = false;
            flag = true;
            GameObject g = GameObject.Instantiate(contentPrefab, transform.position, Quaternion.identity);
            InteractControl i = t.GetComponent<InteractControl>();
            Vector3 throwingAngle = i.throwingAngle;
            float throwPower = i.throwPower;
            float throwForce = i.throwForce;
            g.GetComponent<Rigidbody>().AddForce(throwingAngle * throwPower * throwForce, ForceMode.Impulse);
        }
        return flag;
    }

    void OnThrow(MessageArgs msg){
        msg.received = ThrowContent(msg.sender);
    }
}
