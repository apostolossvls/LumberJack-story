using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public KeyCode switchKey;
    public Transform human;
    public Transform dog;
    public Behaviour[] HumanBehaviours;
    public Behaviour[] DogBehaviours;
    public Transform PlayerPivot;
    bool onHuman;
    bool onDog;
    void Start()
    {
        onHuman = true;
        onDog = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey)){
            if (onHuman){
                onHuman = false;
                onDog = true;
            }
            else {
                onHuman = true;
                onDog = false;
            }

            foreach (Behaviour b in HumanBehaviours)
            {
                b.enabled = onHuman;
            }
            foreach (Behaviour b in DogBehaviours)
            {
                b.enabled = onDog;
            }

            if (PlayerPivot){
                if (PlayerPivot.GetComponent<PositionMatchPosition>()){
                    Transform t = null;
                    if (onHuman) t = human;
                    else if (onDog) t = dog;
                    PlayerPivot.GetComponent<PositionMatchPosition>().changeTarget(t);
                }
            }
        }
    }
}
