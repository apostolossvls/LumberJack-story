using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        NavMeshAgentDisabler dis = GetComponent<NavMeshAgentDisabler>();
        if (dis) dis.SetAgent(false);
        yield return new WaitForSeconds(deathDelayAfterFall);
        if (dis) dis.SetAgent(true);
        Destroy(gameObject);
        yield return null;
    }
}
