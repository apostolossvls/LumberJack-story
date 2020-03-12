using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public InputMaster controls;
    public Collider col;
    public Rigidbody rig;
    public CinemachineVirtualCamera cam;
    public Vector3 offset;
    public float moveSpeed = 5f;
    public bool lookingLeft=true;
    public bool lookChanging=false;
    public float rotationSpeed = 10.0f;
    Vector3 movement;
    public float movementControl=1f;
    float distToGround;
    CinemachineComposer cinemachineTransposer;


    void Awake(){
        controls = new InputMaster();
        controls.Player.Movement.performed += cxt => Move(cxt.ReadValue<Vector2>());
    }

    void OnEnable(){
        controls.Player.Enable();
    }

    void OnDisable(){
        controls.Player.Disable();
    }

    void Start(){
        if (col==null) col  = GetComponent<Collider>();
        if (rig==null && gameObject.GetComponent<Rigidbody>()){
            rig = gameObject.GetComponent<Rigidbody>();
        }
        distToGround = col.bounds.extents.y;
        lookingLeft=true;
        lookChanging=false;
        cinemachineTransposer = cam.GetCinemachineComponent<CinemachineComposer>();
    }

    void Move(Vector2 direction){
        //Debug.Log("Player new move: "+ direction);
    }

    void Update(){
        //Debug.DrawRay(transform.position, transform.forward, Color.green);
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        //if hit wall on x, dont move on x
        if (Mathf.Abs(movement.x)>0){
            //float mX = movement.x * moveSpeed * movementControl;
            // movement * moveSpeed * movementControl * Time.fixedDeltaTime
            RaycastHit[] hit = Physics.RaycastAll(transform.position, new Vector3(movement.x, 0, 0), col.bounds.extents.x+0.1f);
            foreach (RaycastHit h in hit)
            {
                Rigidbody r = h.transform.GetComponentInChildren<Rigidbody>();
                if (!r) {
                    Collider c = h.transform.GetComponentInChildren<Collider>();
                    if (c)
                        if (c.isTrigger) continue;
                    movement.x = 0;
                }
                else
                {
                    if (r.gameObject == gameObject) continue;
                    if (r.isKinematic)
                        movement.x = 0;
                }
            }
        }

        Vector3 camT;
        if ((movement.x<0 && lookingLeft) || (movement.x>0 && !lookingLeft)) {
            StopCoroutine("lookChangingTime");
            StartCoroutine("lookChangingTime");
        }
        if (movement.x>0) {
            lookingLeft = true;
            camT = new Vector3(offset.x,offset.y,offset.z);
        }
        else if (movement.x<0) {
            lookingLeft = false;
            camT = new Vector3(-offset.x,offset.y,offset.z);
        }
        else{
            camT = new Vector3(0,0,0);
        }
        cinemachineTransposer.m_TrackedObjectOffset = camT;
        //Debug.DrawLine(rig.position, rig.position +  movement, Color.red, 1f);
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
        Vector3 v = lookingLeft? new Vector3(1f, 0f, -0.001f) : new Vector3 (-1f, 0f, -0.001f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(v), Time.deltaTime * rotationSpeed);
    }

    bool IsGrounded() {
        if (rig.velocity.y==0) return true;
        else return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
    }

    IEnumerator lookChangingTime(){
        lookChanging = true;
        yield return new WaitForSeconds(5/rotationSpeed);
        lookChanging = false;
        yield return null;
    }
}
