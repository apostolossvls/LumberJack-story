using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public float spinForce=1f;
    public float maxAngVel=1f;
    Rigidbody rig;
    public bool flyingWithSpin;
    public float rayDistance=3f;
    public float slerpMult=100f;
    // Start is called before the first frame update
    void Start()
    {
        flyingWithSpin=false;
        rig = GetComponent<Rigidbody>();
    }

    void FixedUpdate(){
        if (flyingWithSpin) {
            rig.AddTorque(spinForce * transform.right);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rig.velocity, out hit, rayDistance)){
                Debug.DrawRay(transform.position, rig.velocity, Color.red);
                float angleV = Mathf.Atan2(rig.velocity.y, rig.velocity.x) * Mathf.Rad2Deg;
                Debug.Log(angleV);
                Transform t = hit.transform;
                if ( ! (angleV > -135 && angleV < -45)){
                    Debug.Log("ANGLE CORRECT");
                    if (!t.GetComponent<InteractControl>()){
                        Quaternion rot = Quaternion.Euler(new Vector3(transform.rotation.x, 90 * Mathf.Sign(rig.velocity.x), transform.rotation.z));
                        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * spinForce * slerpMult);
                    }
                }
            }
        }
    }

    void OnThrow(){
        rig.maxAngularVelocity = maxAngVel;
        flyingWithSpin=true;
        //rig.AddTorque(spinForce * transform.right, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (flyingWithSpin && !collision.gameObject.GetComponent<InteractControl>()) flyingWithSpin = false; 
        foreach (ContactPoint contact in collision.contacts)
        {
        }
    }
}
