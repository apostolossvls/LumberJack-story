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

    void Awake(){
        if (target){
            vcam = cam.GetComponent<CinemachineVirtualCamera>();
            confiner = vcam.GetComponent<CinemachineConfiner>();
            //Vector3 v = new Vector3(target.position.x, target.position.y, cam.position.z);
            for (int i = 0; i < boundaries.Length; i++)
            {

            if (!boundaries[i]) continue; 

            if (boundaries[i].bounds.Contains(target.position)){
                transform.position = target.position;
                CameraBoundary cb = boundaries[i].GetComponent<CameraBoundary>();
                if (cb) confiner.m_BoundingVolume = cb.cameraBoundary;
                else Debug.Log(this+": Error confiner.m_BoundingVolume missing");
                break;
            }
            else {
                if (boundaries[i].bounds.Contains(new Vector3(target.position.x, boundaries[i].transform.position.y, target.position.z))){
                    //Debug.Log("IN X");
                    transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
                }
                if (boundaries[i].bounds.Contains(new Vector3(boundaries[i].transform.position.x, target.position.y, target.position.z))){
                    //Debug.Log("IN Y");
                    transform.position = new Vector3(transform.position.x, target.position.y, target.position.z);
                }
            }

            }
        }
    }

    void Update()
    {
        if (target){
            //Vector3 v = new Vector3(target.position.x, target.position.y, cam.position.z);
            for (int i = 0; i < boundaries.Length; i++)
            {

            if (!boundaries[i]) continue; 

            if (boundaries[i].bounds.Contains(target.position)){
                transform.position = target.position;
                CameraBoundary cb = boundaries[i].GetComponent<CameraBoundary>();
                if (cb) confiner.m_BoundingVolume = cb.cameraBoundary;
                else Debug.Log(this+": Error confiner.m_BoundingVolume missing");
                break;
            }
            else {
                if (boundaries[i].bounds.Contains(new Vector3(target.position.x, boundaries[i].transform.position.y, target.position.z))){
                    //Debug.Log("IN X");
                    transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
                }
                if (boundaries[i].bounds.Contains(new Vector3(boundaries[i].transform.position.x, target.position.y, target.position.z))){
                    //Debug.Log("IN Y");
                    transform.position = new Vector3(transform.position.x, target.position.y, target.position.z);
                }
            }

            }
        }
    }

    public void ChangeTarget(Transform t){
        target = t;
    }
}
