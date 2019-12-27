using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public Collider col;
    Rigidbody rig;
    public float JumpUpwardsTime=1f;
    public float JumpForce = 250f;
    public float Gravity = 14f;
    public float GroundCheck=1f;
    float distToGround;
    bool jumpPressed;

    void Start()
    {
        if (col==null) col  = GetComponent<Collider>();
        if (rig==null) rig = GetComponentInParent<Rigidbody>(); 
        distToGround = col.bounds.extents.y;
        jumpPressed=false;
    }
    void Update()
    {
        if (CloseToGround()){
            if (Input.GetKeyDown(KeyCode.Space)){
                jumpPressed=true;
            }
        }
        else {
            rig.AddForce(new Vector3(0,-Gravity*Time.deltaTime,0));
        }
        if (IsGrounded() && jumpPressed){
            //rig.velocity=new Vector3(rig.velocity.x, 0, rig.velocity.z);
            jumpPressed=false;
            rig.AddForce(new Vector3(0,JumpForce,0), ForceMode.Impulse);
            //StartCoroutine("JumpForceOnTimer");
        }
    }

    bool CloseToGround() {
        if (rig.velocity.y==0) return true;
        else {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -Vector3.up+rig.velocity, out hit, distToGround + GroundCheck)){
                //Debug.DrawRay(transform.position, -Vector3.up+rig.velocity, Color.yellow, 1f);
                if (hit.collider!=col) return true;
                else return false;
            }
            else return false;
        }
    }

    bool IsGrounded() {
        StopAllCoroutines();
        if (rig.velocity.y==0) return true;
        else return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
    }

    /*IEnumerator JumpForceOnTimer(){
        //rig.AddForce(new Vector3(0,JumpForce,0), ForceMode.Impulse);
        float timer=0.01f;
        while(timer<JumpUpwardsTime){
            rig.AddForce(new Vector3(0,-0.1f*JumpForce*Time.deltaTime*timer/JumpUpwardsTime,0), ForceMode.Impulse);
            timer+=Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 10; i++)
        {
            rig.AddForce(new Vector3(0,JumpForce*Time.deltaTime*9-i), ForceMode.Impulse);
            yield return new WaitForSeconds(JumpUpwardsTime/10);
        }
        yield return null;
    }*/
}
