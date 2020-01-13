using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public float speedForce=1f;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnThrow(){
        rigidbody.AddTorque(speedForce * transform.right, ForceMode.Acceleration);
    }
}
