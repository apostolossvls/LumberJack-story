using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    Collider col;
    Rigidbody rig;
    Jump jump;
    PlayerMovement playerMovement;
    float distToGround;
    bool OnLedgeGrab;
    
    public Transform[] raycastPivots; //head to toe, on 0: if no object hit then grab
    public string[] canGrabFrom;
    public float PowerToSeconds = 0.2f;
    public LayerMask layerMask;

    void Start()
    {
        col = GetComponent<Collider>();
        rig = GetComponent<Rigidbody>();
        jump = GetComponent<Jump>();
        playerMovement = GetComponent<PlayerMovement>();
        distToGround = col.bounds.extents.y;
        OnLedgeGrab = false;
    }

    void OnCollisionStay(Collision other)
    {
        if (StringMatch(other.transform.tag) && !OnLedgeGrab){
            bool allContactsCorrectY = true;
            Vector3 contactsPosAverage = Vector3.zero;
            foreach (ContactPoint contact in other.contacts)
            {
                contactsPosAverage+=contact.point;
                if (contact.normal.y>0.1f) allContactsCorrectY = false;
            }
            contactsPosAverage = new Vector3 (contactsPosAverage.x/other.contactCount,contactsPosAverage.y/other.contactCount,contactsPosAverage.z/other.contactCount);
            if (allContactsCorrectY) {
                //Vector3 dir = other.contacts[0].normal;
                //Debug.DrawRay(contactsPosAverage, contactsPosAverage.normalized, Color.blue, 3f);
                
                RaycastHit[] hit0;
                hit0 = Physics.RaycastAll(raycastPivots[0].position, transform.forward, 1, layerMask);
                bool flag = false;
                foreach (RaycastHit h in hit0)
                {
                    if (h.transform.gameObject.isStatic) flag = true;
                }
                if (!flag)
                {
                    bool[] hitMade = new bool[raycastPivots.Length-1];
                    for (int i = 1; i < raycastPivots.Length; i++)
                    {
                        hitMade[i-1] = false;
                        RaycastHit[] hit;
                        hit = Physics.RaycastAll(raycastPivots[i].position, transform.forward, 1, layerMask);
                        foreach (RaycastHit h in hit)
                        {
                            if (h.transform.gameObject.isStatic) {
                                hitMade[i-1] = true;
                                break;
                            }
                        }
                        /*if (Physics.Raycast(raycastPivots[i].position, transform.forward, out hit, 1))
                        {
                            hitMade[i-1] = true;
                            /*if (!willGrab){
                                willGrab = true;
                                float p = power;
                                p = power *  ( (raycastPivots.Length+1 - i) / raycastPivots.Length);
                                Debug.Log("Power: "+p);
                                StartGrab(p);
                                break;
                            }
                        }
                        RaycastHit[] hit0;
                        hit0 = Physics.RaycastAll(raycastPivots[0].position, transform.forward, 1, layerMask);
                        bool flag = false;
                        foreach (RaycastHit h in hit0)
                        {
                            if (TagMatch(h.transform.tag)) flag = true;
                        }
                        if (flag)
                        {
                        }*/
                    }
                    if (hitMade[0]) StartGrab(PowerToSeconds);
                    else if (hitMade[1]) StartGrab(PowerToSeconds * 2/3);
                    else if (hitMade[2]) StartGrab(PowerToSeconds / 3);
                }
            }
        }
    }

    void StartGrab(float secondsUp){
        OnLedgeGrab=true;
        Vector3 v = jump.gameObject.GetComponent<Rigidbody>().velocity;
        v = new Vector3(v.x, 0, v.z);
        jump.gameObject.GetComponent<Rigidbody>().velocity = v;
        jump.JumpOnDemand = true;
        jump.onJumpHold = true;
        playerMovement.enabled = false;
        StartCoroutine(SetJumpPressedForSeconds(secondsUp));
    }

    IEnumerator SetJumpPressedForSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        if (jump) {
            jump.onJumpHold = false;
            Vector3 v = jump.gameObject.GetComponent<Rigidbody>().velocity;
            v = new Vector3(v.x, 0, v.z);
            jump.gameObject.GetComponent<Rigidbody>().velocity = v;
        }
        playerMovement.enabled = true;
        rig.AddForce(transform.forward * 3f, ForceMode.Impulse);
        OnLedgeGrab=false;
    }

    public bool IsGrounded() {
        if (rig.velocity.y==0) return true;
        else return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.2f);
    }

    bool StringMatch (string s){
        foreach (string t in canGrabFrom)
        {
            if (s==t) return true;
        }
        return false;
    }
}
