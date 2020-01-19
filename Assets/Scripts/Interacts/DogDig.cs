using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogDig : MonoBehaviour
{
    public Transform buriedObject;
    public string tagAfterDig;
    public Transform endTransform;
    public Transform nowPos;
    public float speed=1f;
    public float DigAmount=3;
    float DigCount;
    Transform user;

    void Start(){
        DigCount = 0;
    }

    void Dig(float amount){
        DigCount += amount;
        Vector3 dif = (endTransform.position - nowPos.position) * (DigCount / DigAmount);
        Quaternion newR = Quaternion.Slerp(nowPos.rotation, endTransform.rotation, DigCount / DigAmount);
        nowPos.position += dif;
        nowPos.rotation = newR;
        StopCoroutine("ComingUp");
        StartCoroutine(ComingUp(buriedObject, nowPos));
    }

    void FinishDig(){
        Rigidbody r = buriedObject.GetComponent<Rigidbody>();
        if (r) r.isKinematic = false;
        if (buriedObject.transform.parent=transform){
            if (transform.parent!=null) buriedObject.transform.parent = transform.parent;
            else buriedObject.parent = null;
        }
        Collider c = buriedObject.GetComponent<Collider>();
        if (c){
            c.enabled = true;
        }
        buriedObject.tag = tagAfterDig;
        Debug.Log("Dig finished");
        transform.tag = "Untagged";
        if (GetComponent<Collider>()) Destroy(GetComponent<Collider>());
        Destroy(this);
    }

    IEnumerator ComingUp(Transform t, Transform newT){
        float timer=0;
        while (timer<1/speed){
            Debug.Log("DCALL");
            timer+=Time.deltaTime;
            float ratio = timer * (1/speed);
            t.rotation = Quaternion.Slerp(t.rotation, nowPos.rotation, ratio);
            t.position = Vector3.Slerp(t.position, nowPos.position,  ratio);
            yield return new WaitForEndOfFrame();
        }
        if (DigAmount<=DigCount){
            FinishDig();
        }
        yield return null;
    }

    void OnHoldInteract(MessageArgs msg){
        msg.received = true;
        Dig(1);
    }
}
