using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechPause : MonoBehaviour
{
    [HideInInspector]
    public Dialog dialog;
    public float pauseSeconds = 1f;

    void OnEnable()
    {
        StartCoroutine(GoNext());
    }

    IEnumerator GoNext(){
        yield return new WaitForSeconds(pauseSeconds);
        dialog.OnBubbleEnd();
        Destroy(gameObject);
        yield return null;
    }
}
