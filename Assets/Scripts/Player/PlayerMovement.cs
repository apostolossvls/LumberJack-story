using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Collider col;
    public Rigidbody rig;
    public float moveSpeed = 5f;
    public bool lookingLeft=true;
    public float rotationSpeed = 10.0f;
    Vector3 movement;
    public float movementControl=1f;
    float distToGround;

    void Start(){
        if (col==null) col  = GetComponent<Collider>();
        if (rig==null && gameObject.GetComponent<Rigidbody>()){
            rig = gameObject.GetComponent<Rigidbody>();
        }
        distToGround = col.bounds.extents.y;
        lookingLeft=true;
    }

    void Update(){
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        if (movement.x>0) lookingLeft = true;
        else if (movement.x<0) lookingLeft = false;
        Debug.DrawLine(rig.position, rig.position +  movement, Color.red, 1f);
    }

    void FixedUpdate(){
        //rig.velocity = new Vector3(movement.x==0? rig.velocity.x:0, rig.velocity.y, movement.z==0? rig.velocity.z:0);
        rig.MovePosition(rig.position + movement * moveSpeed * movementControl * Time.fixedDeltaTime);
        //rig.MovePosition(rig.position + movement * moveSpeed * Time.fixedDeltaTime * (IsGrounded()? 1 : Mathf.Clamp01(Mathf.Abs(rig.velocity.y))));
        //rig.AddForce(movement * moveSpeed * Time.fixedDeltaTime, ForceMode.Impulse);
        //rig.velocity = new Vector3(rig.velocity.x + (movement.x * moveSpeed * Time.fixedDeltaTime), rig.velocity.y, rig.velocity.z + (movement.z* moveSpeed * Time.fixedDeltaTime));
        
        /*if (movement!=Vector3.zero) {
            Vector3 v = new Vector3(movement.z, movement.y, movement.x);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(v), Time.deltaTime * rotationSpeed);
        }*/
        Vector3 v = lookingLeft? new Vector3(0f, 0f, 1f) : new Vector3 (0f, 0f, -1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(v), Time.deltaTime * rotationSpeed);
    }

    bool IsGrounded() {
        if (rig.velocity.y==0) return true;
        else return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
    }
}
