using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutDownTree : MonoBehaviour
{
    public Transform log;
    public bool activated;

    void Start()
    {
        activated=false;
    }

    void Update()
    {
        if (activated){
            if (Input.GetButtonUp("Action")){
                CutDown();
            }
        }
    }

    void CutDown(){
        log.parent = null;
        if (log.GetComponent<Rigidbody>()) log.GetComponent<Rigidbody>().isKinematic = false;
    }

    void Interact(){
        //Destroy(gameObject);
        Debug.Log("Interact recived");
        activated=true;
    }

    void Abort(){
        activated = false;
    }
}
