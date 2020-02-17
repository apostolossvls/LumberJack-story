using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractIndicator : MonoBehaviour
{
    public bool active = true;
    public bool showHint = true;
    public GameObject hint;
    public float timeBeforeHint = 5f;
    float timer;
    public Vector3 pos;

    void OnEnable()
    {
        hint.SetActive(false);
        pos = transform.position;
        if (IsActive()){
            StartCoroutine(HintDisplay());
        }
    }

    void Update(){
        if (transform.position != pos){
            StopCoroutine(HintDisplay());
            StartCoroutine(HintDisplay());
            pos = transform.position;
        }
    }

    IEnumerator HintDisplay(){
        if (showHint){
            timer = 0;
        
            while (timer < timeBeforeHint){
                timer += Time.deltaTime;
                yield return null;
            }

            DisplayHint();
        }
    }

    public void DisplayHint(){
        hint.SetActive(true);
    }

    public bool IsActive(){
        if (!active) {
            gameObject.SetActive(false);
            return false;
        }
        return true;
    }
}
