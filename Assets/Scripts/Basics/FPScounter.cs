using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPScounter : MonoBehaviour
{
    Text textUI;
    public float tick = 0.1f;
    float timer;
    void Start()
    {
        textUI = GetComponent<Text>();
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= tick){
            timer = 0;
            if (textUI) textUI.text = "FPS: " + ((int)(1f / Time.deltaTime)).ToString();
        }
    }
}
