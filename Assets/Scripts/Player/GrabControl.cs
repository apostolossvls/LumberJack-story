using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabControl : MonoBehaviour
{
    public bool leftHandGrabbing;
    public bool rightHandGrabbing;
    public Transform leftGrab;
    public Transform rightGrab;
    public Component script;

    void Start()
    {
        ReleaseBothHands();
    }

    void GrabBothHands(Component c, Transform t){
        ReleaseBothHands();
        script = c;
        leftHandGrabbing = true;
        rightHandGrabbing = true;
    }

    void ReleaseBothHands(){
        script = null;
        leftHandGrabbing = false;
        rightHandGrabbing = false;
    }
}
