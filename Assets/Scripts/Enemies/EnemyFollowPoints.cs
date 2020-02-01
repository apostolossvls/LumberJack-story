using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPoints : MonoBehaviour
{
    public Vector3[] points;
    public int pointIndex;
    Vector3 currentTarget;
    public float tolerance;
    public float speed=1f;
    public float delayTime;
    float delayStart;
    public bool automatic=true;
    Rigidbody rig;

    void Start(){
        rig = GetComponent<Rigidbody>();
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
            UpdateTarget();
        }
    }

    void MovePlatform(){
        Vector3 heading = currentTarget - transform.localPosition;
        rig.MovePosition(transform.position + heading.normalized * speed * Time.fixedDeltaTime);

        if (heading.magnitude < tolerance){
            transform.localPosition = currentTarget;
            delayStart = Time.time;
        }
    }

    void UpdateTarget(){
        if (automatic){
            if (Time.time - delayStart > delayTime){
                NextPoint();
            }
        }
    }

    public void NextPoint(){
        pointIndex++;
        if (pointIndex >= points.Length) pointIndex = 0;
        currentTarget = points[pointIndex];
    }
}
