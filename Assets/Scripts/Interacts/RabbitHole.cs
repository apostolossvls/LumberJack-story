using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitHole : MonoBehaviour
{
    public Transform endhole;
    Transform user;

    void UseRabbitHole(){
        if (user && endhole){
            user.transform.position = endhole.position;
        }
    }

    bool TestRabbitHole(Transform t){
        bool flag = false;
        user = t;
        if (user){
            InteractControl interactControl = user.GetComponent<InteractControl>();
            if (interactControl){
                if (!interactControl.IsHuman){
                    flag = true;
                    UseRabbitHole();
                }
            }
        }
        return flag;
    }

    void OnHoldInteract(MessageArgs msg){
        msg.received = TestRabbitHole(msg.sender); 
    }
}
