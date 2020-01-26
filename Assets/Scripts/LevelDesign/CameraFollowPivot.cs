using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPivot : MonoBehaviour
{
    public Transform target;
    public Transform cam;
    public Collider boundaries;

    void Update()
    {
        if (target){
            Vector3 v = new Vector3(target.position.x, target.position.y, cam.position.z);
            if (boundaries.bounds.Contains(v)){
                transform.position = target.position;
            }
            else {
                if (boundaries.bounds.Contains(new Vector3(target.position.x, boundaries.transform.position.y, cam.position.z))){
                    //Debug.Log("IN X");
                    transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
                }
                if (boundaries.bounds.Contains(new Vector3(boundaries.transform.position.x, target.position.y, cam.position.z))){
                    //Debug.Log("IN Y");
                    transform.position = new Vector3(transform.position.x, target.position.y, target.position.z);
                }
            }
        }
    }

    public void ChangeTarget(Transform t){
        target = t;
    }
}
