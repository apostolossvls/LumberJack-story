using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageArgs
{
    public Transform sender;
    public bool received = false;
    public MessageArgs(Transform s){
        received = false;
        this.sender = s;
    }
}
