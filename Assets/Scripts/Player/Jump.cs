using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public Collider col;
    Rigidbody rig;
    public float JumpUpwardsTime=1f;
    public float JumpForce = 250f;
    public float JumpHoldDuration=1;
    public float Gravity = 14f;
    public float GroundCheck=1f;
    float distToGround;
    bool jumpPressed;
    bool onJumpHold;
    //float jumpHold=1;

    void Start()
    {
        if (col==null) col  = GetComponentInChildren<Collider>();
        if (rig==null) rig = GetComponentInChildren<Rigidbody>(); 
        distToGround = col.bounds.extents.y;
        jumpPressed=false;
        //onJumpHold=false;
        //jumpHold=1;
    }
    void Update()
    {
        if (CloseToGround()){
            if (Input.GetButtonDown("Jump")){ //if (Input.GetKeyDown(KeyCode.Space)){
                jumpPressed=true;
            }
        }
        else {
            rig.AddForce(new Vector3(0,-Gravity*Time.deltaTime,0));
        }
        if (onJumpHold && rig.velocity.y>0 && Input.GetButtonUp("Jump")) {
            onJumpHold = false;
            rig.velocity = new Vector3(rig.velocity.x, Mathf.Clamp(rig.velocity.y, 0, 1), rig.velocity.z);
        }
        //if (!onJumpHold)
        if (IsGrounded() && jumpPressed){
            //jumpHold=1;
            //rig.velocity=new Vector3(rig.velocity.x, 0, rig.velocity.z);
            jumpPressed=false;
            onJumpHold=true;
            //StopCoroutine("OnholdReset");
            //StartCoroutine("OnholdReset");
            rig.AddForce(new Vector3(0,JumpForce,0), ForceMode.Impulse);
            //StartCoroutine("JumpForceOnTimer");
        }
        /*if (Input.GetButton("Jump") && onJumpHold){
            //jumpHold /= 1+jumpHold;
            rig.AddForce(new Vector3(0,JumpForce*jumpHold,0), ForceMode.Impulse);
        } */
    }

    bool CloseToGround() {
        if (rig.velocity.y==0) return true;
        else {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down+rig.velocity, out hit, distToGround + GroundCheck)){
                //Debug.DrawRay(transform.position, -Vector3.up+rig.velocity, Color.yellow, 1f);
                if (hit.collider!=col) return true;
                else return false;
            }
            else return false;
        }
    }

    bool IsGrounded() {
        //StopAllCoroutines();
        if (rig.velocity.y==0) return true;
        else return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.2f);
    }

    /*IEnumerator OnholdReset(){
        yield return new WaitForSeconds(1);
        onJumpHold=false;
        yield return null;
    }*/

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
