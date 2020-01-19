using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitHole : MonoBehaviour
{
    public Transform hole1;
    public Transform hole2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnInteract(MessageArgs msg){
        msg.received = true;
        
    }
}
