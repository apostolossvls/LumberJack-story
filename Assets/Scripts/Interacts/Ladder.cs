﻿using System.Collections;
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
    int thisLayer;

    void Start()
    {
        onLadder = false;
        thisLayer = gameObject.layer;
    }

    void Update()
    {
        if (onLadder && user){
            if (Mathf.Abs(user.transform.localRotation.x) < 30){
                float vertical = Input.GetAxisRaw("Vertical");
                ladderPoint.localPosition = new Vector3(
                    ladderPoint.localPosition.x,
                    Mathf.Clamp(ladderPoint.localPosition.y + vertical * Mathf.Sign(transform.up.y) * Time.deltaTime *climbSpeed, ladderHeightMin, ladderHeightMax),
                    ladderPoint.localPosition.z);
                user.position = ladderPoint.position;
                //Debug.Log("user.transform.up.y:" + user.transform.up.y);
            }
            else {
                user.GetComponent<InteractControl>().ReleaseHand();
            }
            
        }
    }

    bool GetOnLadder(Transform t){
        bool success = false;
        RaycastHit[] hits = Physics.RaycastAll(t.position, t.forward, 2);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == transform){
                success = true;
                InteractControl interactControl = t.GetComponent<InteractControl>();
                if (interactControl){
                    interactControl.ReleaseHand();
                    interactControl.rightGrab = transform;
                    interactControl.leftGrab = transform;
                    interactControl.rightHandGrabbing = true;
                    interactControl.leftHandGrabbing = true;

                    interactControl.rigidbodyConstraints[0] = GetComponent<Rigidbody>().constraints;
                }
                
                ladderPoint.position = new Vector3(hits[i].point.x + ladderPoint.localPosition.z, hits[i].point.y, ladderPoint.position.z);
                onLadder = true;
                user = t;

                if (t.GetComponent<PlayerMovement>()){
                    t.GetComponent<PlayerMovement>().enabled = false;
                }

                gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");

                t.rotation = Quaternion.LookRotation(transform.forward);
                Debug.Log("GettingOnLadder");
                Debug.DrawRay(transform.position, transform.forward, Color.black, 3f);

            }
        }
        return success;
    }

    void ReleaseLadder(Transform t){
        Debug.Log("ReleasingLadder");
        onLadder = false;
        user = null;
        //gameObject.layer = thisLayer;
        if (t.GetComponent<PlayerMovement>()){
            t.GetComponent<PlayerMovement>().enabled = true;
        }
        Rigidbody r = t.GetComponent<Rigidbody>();
        if (r) r.velocity = new Vector3(0, 0, r.velocity.z);
    }

    void OnHoldInteract(MessageArgs msg){
        Debug.Log("OnHoldInteract"+msg.sender.name.ToString());
        msg.received = GetOnLadder(msg.sender);
    }

    void OnRelease(MessageArgs msg){
        msg.received  =true;
        ReleaseLadder(msg.sender);
    }

}
