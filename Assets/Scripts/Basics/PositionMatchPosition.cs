using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMatchPosition : MonoBehaviour
{
    public Transform target;
    public bool x=true;
    public bool y=true;
    public bool z=true;

    public float changeSpeed=0f;
    float timer=0;
    Transform newTarget;
    bool changing=false;

    void Start(){
        newTarget = null;
        timer=0;
    }

    void Update()
    {
        if (target && !changing){
            transform.position = new Vector3(
                x? target.transform.position.x : transform.position.x,
                y? target.transform.position.y : transform.position.y,
                z? target.transform.position.z : transform.position.z);
        }

        if (changing){
            timer+=Time.deltaTime;
            transform.position += (newTarget.position-transform.position)*changeSpeed*Time.deltaTime;
            if (Vector3.Distance(newTarget.position,transform.position)<0.1f){
                changing=false;
                target = newTarget;
                newTarget = null;
                timer=0;
            }
        }
    }

    public void  changeTarget(Transform t){
        if (changeSpeed==0) target = t;
        else {
            newTarget = t;
            changing=true;
        }
    }
}
