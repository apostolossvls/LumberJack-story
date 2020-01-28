using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlatform : MonoBehaviour
{
    public Vector3[] points;
    public int pointIndex;
    Vector3 currentTarget;
    Vector3 dif;
    public float tolerance;
    public float speed;
    public float delayTime;
    float delayStart;
    public bool automatic;
    public string[] tags;
    List<Transform> obj;
    Rigidbody rig;

    void Start(){
        rig = GetComponent<Rigidbody>();
        obj = new List<Transform>{};
        if (points.Length > 0){
            currentTarget = points[0];
        }
        tolerance = speed * Time.fixedDeltaTime;
    }

    void FixedUpdate(){
        if (transform.localPosition!=currentTarget){
            MovePlatform();
        }
        else {
            dif=Vector3.zero;
            UpdateTarget();
        }
        /*
        for (int i = 0; i < obj.Count; i++)
        {
            if (obj[i]==null) obj.RemoveAt(i);
            else {
                //Rigidbody r = obj[i].GetComponent<Rigidbody>();
                //if (r) r.velocity -= dif;
                obj[i].position += dif;
            }
        }
        */
    }

    void MovePlatform(){
        Vector3 heading = currentTarget - transform.localPosition;
        rig.MovePosition(transform.position+heading.normalized * speed * Time.fixedDeltaTime);
        //Vector3.Lerp (movingPlatform.position, newPostion, smooth * Time.deltaTime) ;
        //transform.localPosition += heading.normalized * speed * Time.fixedDeltaTime;
        dif = heading.normalized * speed * Time.fixedDeltaTime;

        //transform.localPosition = Vector3.Lerp(transform.localPosition, currentTarget, speed * Time.fixedDeltaTime);
        //dif = transform.localPosition - lastPos;


        if (heading.magnitude < tolerance){
            transform.localPosition = currentTarget;
            delayStart = Time.time;
        }
        //if (dif.magnitude<=tolerance){
        //    transform.localPosition = currentTarget;
        //    delayStart = Time.time;
        //}
    }

    void UpdateTarget(){
        if (automatic){
            if (Time.time - delayStart > delayTime){
                NextPlatform();
            }
        }
    }

    public void NextPlatform(){
        pointIndex++;
        if (pointIndex >= points.Length) pointIndex = 0;
        currentTarget = points[pointIndex];
    }

    void OnTriggerEnter(Collider other){
        if (TagMatch(other.tag)) {
        //if (TagMatch(other.tag) && OnArray(other.transform)==-1) {
            //obj.Add(other.transform);
            other.transform.parent = transform;
        }
    }
    void OnTriggerExit(Collider other){
        /*if (OnArray(other.transform)!=-1) {
            obj.Remove(other.transform);
        }*/
        if (other.transform.parent == transform) {
            other.transform.parent = null;
        }
    }
    bool TagMatch (string s){
        foreach (string t in tags)
        {
            if (s==t) return true;
        }
        return false;
    }
    int OnArray(Transform t){
        for (int i = 0; i < obj.Count; i++)
        {
            if (obj[i]==t) return i;
        }
        return -1;
    }
}
