using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollowPivot : MonoBehaviour
{
    public Transform target;
    public Transform cam;
    public Collider[] boundaries;
    CinemachineVirtualCamera vcam;
    CinemachineConfiner confiner;
    Collider currentCollider;

    void Awake(){
        if (target){
            vcam = cam.GetComponent<CinemachineVirtualCamera>();
            confiner = vcam.GetComponent<CinemachineConfiner>();
        }
    }

    void Update()
    {
        for (int i = 0; i < boundaries.Length; i++)
        {

        if (!boundaries[i]) continue; 

        if (boundaries[i].bounds.Contains(target.position)){
            
            CameraBoundary cb = boundaries[i].GetComponent<CameraBoundary>();
            if (cb) {
                currentCollider = cb.cameraBoundary;
                if (confiner.m_BoundingVolume != cb.cameraBoundary){
                    confiner.m_BoundingVolume = cb.cameraBoundary;
                }
            }
            else Debug.Log(this+": Error confiner.m_BoundingVolume missing");
            break;
        }

        }

        if (currentCollider){
            Magnetize(currentCollider);
            /*
            Debug.Log("col: "+currentCollider);
        if (currentCollider.bounds.Contains(new Vector3(target.position.x, target.position.y, currentCollider.transform.position.z))){
            transform.position = target.position;
            Debug.Log("IN X,Y");
        }
        else {
            if (currentCollider.bounds.Contains(new Vector3(target.position.x, currentCollider.transform.position.y, currentCollider.transform.position.z))){
                Debug.Log("IN X");
                transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            }
            if (currentCollider.bounds.Contains(new Vector3(currentCollider.transform.position.x, target.position.y, currentCollider.transform.position.z))){
                Debug.Log("IN Y");
                transform.position = new Vector3(transform.position.x, target.position.y, target.position.z);
            }
        }
        */

        }
    }

    public void ChangeTarget(Transform t){
        target = t;
    }

    void ResetPosition(){
        transform.position = target.position;
    }

    void Magnetize(Collider col){
        Vector3 v = col.ClosestPoint(target.position);
        transform.position = new Vector3(v.x, v.y, transform.position.z);
        /*
        transform.position = new Vector3(
            //Mathf.Clamp(Mathf.Abs(target.position.x), float.MinValue, col.bounds.extents.x + Mathf.Abs(col.transform.position.x)) * Mathf.Sign(target.position.x),
            //Mathf.Clamp(Mathf.Abs(target.position.y), float.MinValue, col.bounds.extents.y + Mathf.Abs(col.transform.position.y)) * Mathf.Sign(target.position.y),
            //Mathf.Clamp(Mathf.Abs(target.position.x), float.MinValue, col.bounds.extents.x + Mathf.Abs(col.bounds.center.x)) * Mathf.Sign(target.position.x),           
            //Mathf.Clamp(Mathf.Abs(target.position.x), float.MinValue, col.bounds.extents.x + Mathf.Abs(col.transform.position.x) + col.bounds.center.x) * Mathf.Sign(target.position.x),
            //Mathf.Clamp(Mathf.Abs(target.position.x), float.MinValue, col.bounds.extents.x + col.bounds.center.x) * Mathf.Sign(target.position.x),
            //Mathf.Clamp(Mathf.Abs(target.position.x), float.MinValue, col.bounds.max.x + Mathf.Abs(col.transform.position.x +(col.bounds.center.x - col.transform.position.x))) * Mathf.Sign(target.position.x),
            Mathf.Clamp(Mathf.Abs(target.position.x), float.MinValue, col.bounds.extents.x + Mathf.Abs(2 * col.transform.position.x - col.transform.position.x)) * Mathf.Sign(target.position.x),
            Mathf.Clamp(Mathf.Abs(target.position.y), float.MinValue, col.bounds.extents.y + Mathf.Abs(col.bounds.center.y)) * Mathf.Sign(target.position.y),
            target.position.z
        );
        */
    }
}
