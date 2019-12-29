using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceJoint : MonoBehaviour
{
    float force;
    void Start()
    {
        force = GetComponent<FixedJoint>().breakForce;
    }
    
    void OnJointBreak(float breakForce)
    {
        Debug.Log("A joint has just been broken!, force: " + breakForce);
        FixedJoint j = gameObject.AddComponent<FixedJoint>();
        j.breakForce = force;
    }
}
