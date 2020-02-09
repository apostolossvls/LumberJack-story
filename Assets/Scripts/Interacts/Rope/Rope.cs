using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    void OnGrab(MessageArgs msg){
        Debug.Log("on grab");
        msg.received = true;
        Transform t = msg.sender;
        SpringJoint sj = null;
        Rigidbody rig = t.GetComponent<Rigidbody>();
        if (rig){
            foreach (SpringJoint j in GetComponents<SpringJoint>())
            {
                if (j.connectedBody == rig){
                    sj = j;
                    break;
                }
            }
        }
        if (sj){
            Debug.Log("Found sj: "+sj);
            sj.autoConfigureConnectedAnchor = false;
            sj.connectedAnchor = Vector3.zero;
            sj.damper = 0;
            sj.tolerance = 1f;
            sj.minDistance = 0.0f;
            sj.maxDistance = 0.1f;
        }
    }
}
