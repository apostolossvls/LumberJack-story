using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetMovement : MonoBehaviour
{
    public Transform target;
    PlayerMovement playerMovement;
    public Vector3 offset;
    Vector3 oldposition;
    Vector3 nowPosition;
    public List<TargetPoint> points;
    float timer;
    public float tick=0.1f;
    public float tickTimer;
    public int LeaveSteps=5;

    public struct TargetPoint
    {
        public Vector3 position;
        public float time;
        public bool onground;
        public bool lookingLeft;
        public TargetPoint(Vector3 p, float t, bool g, bool left){
            this.position = p;
            this.time = t;
            this.onground = g;
            this.lookingLeft = left;
        }
    }

    void Start()
    {
        playerMovement = target.GetComponent<PlayerMovement>();
        timer=0;
        tickTimer=0;
        nowPosition = transform.position;
        oldposition = target.position;
        points = new List<TargetPoint>{};
        for (int i = 0; i < LeaveSteps; i++)
        {
            points.Add(new TargetPoint((target.position-transform.position)/LeaveSteps, 0, true, true));
        }
    }


    void Update()
    {
        points.Add(new TargetPoint(target.position, timer, IsGrounded(), playerMovement.lookingLeft));
        timer=0;
        oldposition = target.position;
        if (points.Count>LeaveSteps){
            PlayNextStep();
        }
        transform.position = Vector3.Lerp(transform.position, nowPosition, 1);
        Vector3 v = points[0].lookingLeft? new Vector3(0f, 0f, 1f) : new Vector3 (0f, 0f, -1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(v), Time.deltaTime * 10);
    }

    bool IsGrounded() {
        if (target.GetComponent<Rigidbody>().velocity.y==0) return true;
        else return Physics.Raycast(target.position, -Vector3.up, target.GetComponent<Collider>().bounds.extents.y + 0.2f);
    }

    void PlayNextStep(){
        //if (!points[0].onground) Debug.Log("On Air: "+points[0].time);
        //tickTimer=1f;
        nowPosition = points[0].position+offset;
        //if (!points[0].onground) tickTimer = points[0].time+1f;
        points.RemoveAt(0);
    }
}
