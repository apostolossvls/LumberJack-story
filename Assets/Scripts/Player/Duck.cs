using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public Transform body;
    public Vector3 DuckScale  = new Vector3(1f,1f,1f);
    Vector3 nowScale;
    bool ducking;

    void Start(){
        ducking=false;
    }
    void Update()
    {
        if (Input.GetButtonDown("Duck")){
            if (!ducking){
                nowScale = body.transform.localScale;
                body.transform.localScale = DuckScale;
            }   
            else {
                body.transform.localScale = nowScale;
            }
            ducking=!ducking;
        }
    }
}
