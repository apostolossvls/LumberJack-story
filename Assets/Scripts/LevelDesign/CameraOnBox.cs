using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraOnBox : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    Collider col;
    bool active;
    ControlManager cm;

    void Start()
    {
        cm = Object.FindObjectOfType<ControlManager>();
        col = GetComponent<Collider>();
        active = false;
    }


    void OnTriggerEnter(Collider other){
        if ((ControlManager.onHuman && other.tag=="PlayerHuman") || (ControlManager.onDog && other.tag=="PlayerDog")){
            Activate();
        }
    }

    void OnTriggerExit(Collider other){
        if ((ControlManager.onHuman && other.tag=="PlayerHuman") || (ControlManager.onDog && other.tag=="PlayerDog")){
            Deactivate();
        }
    }

    void Activate(){
        vcam.m_Priority = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.Priority + 1;
    }

    void Deactivate(){
        vcam.m_Priority -= 2;
    }
}
