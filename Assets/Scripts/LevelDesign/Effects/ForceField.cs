using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public Vector3 direction = new Vector3(0, 1f, 0);
    public float power = 10f;
    public ForceMode mode = ForceMode.Acceleration;

    void OnTriggerStay(Collider other){
        if (this.enabled){
            Rigidbody rig = other.GetComponent<Rigidbody>();
            if (rig){
                rig.AddForce(direction * power, mode);
            }
        }
    }
}
