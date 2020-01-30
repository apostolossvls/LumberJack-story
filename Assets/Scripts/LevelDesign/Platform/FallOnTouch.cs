using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOnTouch : MonoBehaviour
{
    public float falldelay = 0f;
    public float deathDelayAfterFall=2f;
    Rigidbody rig;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Fall(){
        if (rig){
            rig.isKinematic = false;
            rig.useGravity = true;
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.tag=="PlayerHuman" || other.tag=="PlayerDog"){
            StartCoroutine(DelayFall());
        }
    }

    IEnumerator DelayFall(){
        if (falldelay>0) yield return new WaitForSeconds(falldelay);
        Fall();
        yield return new WaitForSeconds(deathDelayAfterFall);
        Destroy(gameObject);
        yield return null;
    }
}
