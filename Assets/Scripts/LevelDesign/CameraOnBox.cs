using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraOnBox : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    Collider col;
    ControlManager controlManager;
    int previousCameraPriority;
    bool active;

    void Start()
    {
        controlManager = Object.FindObjectOfType<ControlManager>();
        col = GetComponent<Collider>();
        active = false;
    }

    void Update(){
        if ((ControlManager.onHuman && col.bounds.Contains(controlManager.human.position)) || (ControlManager.onDog && col.bounds.Contains(controlManager.dog.position))){
            if (!active){
                Activate();
            }
        }
        else if (active){
            Deactivate();
        }
    }

    void Activate(){
        active = true;
        previousCameraPriority = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Priority;
        vcam.m_Priority = previousCameraPriority + 1;
    }

    void Deactivate(){
        active = false;
        vcam.m_Priority = previousCameraPriority - 1;
    }
}
