using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    public Collider col;
    public Rigidbody rig;
    public float JumpForce = 5f;
    public float WallJumpForce = 5f;
    public float ControlTime=1f;
    public float Gravity = 14f;
    float distToGround;

    void Start()
    {
        if (col==null) col  = GetComponent<Collider>();
        if (rig==null) rig = GetComponent<Rigidbody>(); 
        distToGround = col.bounds.extents.y;
    }
    void Update()
    {
        if (IsGrounded()){
            if (Input.GetKeyDown(KeyCode.Space)){
                rig.AddForce(new Vector3(0,JumpForce,0), ForceMode.Impulse);
                rig.velocity += new Vector3(0,10,0);
            }
        }
        else {
            rig.AddForce(new Vector3(0,-Gravity*Time.deltaTime,0));
        }
    }

    bool IsGrounded() {
        if (rig.velocity.y==0) return true;
        else return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
    }

    void OnCollisionStay(Collision other)
    {
        if (!IsGrounded()){
            //Debug.Log("contact");
            bool allContactsCorrectY = true;
            Vector3 contactsPosAverage = Vector3.zero;
            foreach (ContactPoint contact in other.contacts)
            {
                contactsPosAverage+=contact.point;
                if (contact.normal.y>0.1f) allContactsCorrectY = false;
            }
            contactsPosAverage = new Vector3 (contactsPosAverage.x/other.contactCount,contactsPosAverage.y/other.contactCount,contactsPosAverage.z/other.contactCount);
            if (allContactsCorrectY) {
                if (Input.GetKeyDown(KeyCode.Space)){
                    Vector3 dir = other.contacts[0].normal;
                    //dir.Set(dir.x, dir.y+0.5f, dir.z); //dir.y+0.5f*Mathf.Sign(rig.velocity.y)
                    //dir.Normalize();
                    Debug.DrawRay(contactsPosAverage, dir, Color.blue, 3f);
                    rig.velocity = new Vector3(rig.velocity.x, 0, rig.velocity.z);
                    rig.AddForce(dir*WallJumpForce, ForceMode.Impulse);
                    //rig.AddForce(new Vector3(contactsPosAverage.x*WallJumpForce,JumpForce,0));
                    StartCoroutine("StopPlayerMovement");
                    Debug.DrawRay(contactsPosAverage, other.contacts[0].normal, Color.white, 3f);
                }
            }
        }
    }

    IEnumerator StopPlayerMovement(){
        if (GetComponent<PlayerMovement>()) {
            float timer=0;
            GetComponent<PlayerMovement>().movementControl=0;
            while (timer<ControlTime){
                GetComponent<PlayerMovement>().movementControl+=Time.fixedDeltaTime/ControlTime;
                GetComponent<PlayerMovement>().movementControl = Mathf.Clamp01(GetComponent<PlayerMovement>().movementControl);
                timer+=Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
        yield return null;
    }
}
