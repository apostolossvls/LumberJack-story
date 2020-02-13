using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePlayerForAnimation : MonoBehaviour
{
    public ControlManager controlManager;
    public Animation anim;
    public bool takeAnimationDuration;
    public float duration = 5f;
    public bool inspectorTest;

    void Start(){
        inspectorTest = false;
    }
    void Update(){
        if (inspectorTest){
            inspectorTest = false;
            StartCoroutine(Play());
        }
    }

    public IEnumerator Play(){
        bool inputs = controlManager.acceptInputs;
        if (takeAnimationDuration && anim) {
            duration = anim.clip.length;
        }

        controlManager.PlayersActive(false);
        controlManager.acceptInputs = false;
        if (anim){
            anim.Play();
        }
        yield return new WaitForSeconds(duration);

        controlManager.PlayersActive(true);
        controlManager.acceptInputs = inputs;
        Debug.Log("animation end");
        yield return null;
    }
}
