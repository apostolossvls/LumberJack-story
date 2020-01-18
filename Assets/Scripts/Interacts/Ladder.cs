using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Transform ladderPoint;
    public float ladderHeightMin;
    public float ladderHeightMax;
    public float climbSpeed=2f;
    Transform user;
    bool onLadder;

    void Start()
    {
        onLadder = false;
    }

    void Update()
    {
        if (onLadder && user){
            float vertical = Input.GetAxisRaw("Vertical");
            ladderPoint.localPosition = new Vector3(
                ladderPoint.localPosition.x,
                Mathf.Clamp(ladderPoint.localPosition.y + vertical * Time.deltaTime *climbSpeed, ladderHeightMin, ladderHeightMax),
                ladderPoint.localPosition.z);
            user.position = ladderPoint.position;
        }
    }

    void GetOnLadder(Transform t){
        RaycastHit[] hits = Physics.RaycastAll(t.position, t.forward, 2);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == transform){
                InteractControl interactControl = t.GetComponent<InteractControl>();
                if (interactControl){
                    interactControl.ReleaseHand();
                    interactControl.rightGrab = transform;
                    interactControl.leftGrab = transform;
                    interactControl.rightHandGrabbing = true;
                    interactControl.leftHandGrabbing = true;
                }
                
                ladderPoint.position = new Vector3(hits[i].point.x + ladderPoint.localPosition.x, hits[i].point.y, ladderPoint.position.z);
                onLadder = true;
                user = t;

                if (t.GetComponent<PlayerMovement>()){
                    t.GetComponent<PlayerMovement>().enabled = false;
                }
                
                t.rotation = Quaternion.LookRotation(transform.forward);
                Debug.Log("GettingOnLadder");
                Debug.DrawRay(transform.position, transform.forward, Color.black, 3f);

            }
        }

    }

    void ReleaseLadder(Transform t){
        Debug.Log("ReleasingLadder");
        onLadder = false;
        user = null;
        if (t.GetComponent<PlayerMovement>()){
            t.GetComponent<PlayerMovement>().enabled = true;
        }
        Rigidbody r = t.GetComponent<Rigidbody>();
        if (r) r.velocity = new Vector3(0, 0, r.velocity.z);
    }

    void OnHoldInteract(Transform t){
        GetOnLadder(t);
    }

    void OnRelease(Transform t){
        ReleaseLadder(t);
    }

}
