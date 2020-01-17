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
    bool[] HumanBehavioursActiveWasActive;
    bool[] DogBehavioursActiveWasActive;
    void Start()
    {
        onHuman = true;
        onDog = false;
        HumanBehavioursActiveWasActive = new bool[HumanBehavioursActive.Length];
        DogBehavioursActiveWasActive = new bool[DogBehavioursActive.Length];
        SetBehavioursActive(0);
        for (int i = 0; i < DogBehavioursActiveWasActive.Length; i++)
        {
            DogBehavioursActiveWasActive[i] = true;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("SwitchCharacter")){
            if (onHuman){
                onHuman = false;
                onDog = true;
                SetBehavioursActive(0);
            }
            else {
                onHuman = true;
                onDog = false;
                SetBehavioursActive(1);
            }
            
            for (int i = 0; i < HumanBehavioursActive.Length; i++)
            {
                HumanBehavioursActive[i].enabled = onHuman && HumanBehavioursActiveWasActive[i];
            }
            for (int i = 0; i < DogBehavioursActive.Length; i++)
            {
                DogBehavioursActive[i].enabled = onDog && DogBehavioursActiveWasActive[i];
            }
            /*
            foreach (Behaviour b in HumanBehavioursActive)
            {
                b.enabled = onHuman;
            }
            foreach (Behaviour b in DogBehavioursActive)
            {
                b.enabled = onDog;
            }
            */
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

    void SetBehavioursActive(int index){
        if (index==0){
            for (int i = 0; i < HumanBehavioursActiveWasActive.Length; i++)
            {
                HumanBehavioursActiveWasActive[i] = HumanBehavioursActive[i].enabled;
            }
        }
        else if (index==1){
            for (int i = 0; i < DogBehavioursActiveWasActive.Length; i++)
            {
                DogBehavioursActiveWasActive[i] = DogBehavioursActive[i].enabled;
            }
        }
    }

    void SetPlayerCharOnStandby(Transform character, bool onStandby=true){
        character.position = new Vector3(character.position.x, character.position.y, (onStandby? LevelSettings.secondZLine : LevelSettings.mainZLine));
    }
}
