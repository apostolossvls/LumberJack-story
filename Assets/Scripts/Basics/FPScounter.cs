using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPScounter : MonoBehaviour
{
    Text textUI;
    void Start()
    {
        textUI = GetComponent<Text>();
    }

    void Update()
    {
        if (textUI) textUI.text = "FPS: " + ((int)(1f / Time.unscaledDeltaTime)).ToString();
    }
}
