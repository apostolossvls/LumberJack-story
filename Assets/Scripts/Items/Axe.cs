using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public float speedForce=1f;
    Rigidbody rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    void OnThrow(){
        rig.AddTorque(speedForce * transform.right, ForceMode.Acceleration);
    }
}
