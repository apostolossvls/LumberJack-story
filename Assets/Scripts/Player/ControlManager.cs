using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public Transform human;
    public Transform dog;
    public Behaviour[] HumanBehavioursActive;
    public Behaviour[] DogBehavioursActive;
    public Behaviour[] HumanBehavioursInactive;
    public Behaviour[] DogBehavioursInactive;
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
        if (Input.GetButtonDown("SwitchCharacter")){
            if (onHuman){
                onHuman = false;
                onDog = true;
            }
            else {
                onHuman = true;
                onDog = false;
            }

            foreach (Behaviour b in HumanBehavioursActive)
            {
                b.enabled = onHuman;
            }
            foreach (Behaviour b in DogBehavioursActive)
            {
                b.enabled = onDog;
            }
            foreach (Behaviour b in HumanBehavioursInactive)
            {
                b.enabled = !onHuman;
            }
            foreach (Behaviour b in DogBehavioursInactive)
            {
                b.enabled = !onDog;
            }
            SetPlayerCharOnStandby(human, !onHuman);
            SetPlayerCharOnStandby(dog, !onDog);

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

    void SetPlayerCharOnStandby(Transform character, bool onStandby=true){
        character.position += Vector3.forward * 0.5f * (onStandby? 1 : -1);
    }
}
