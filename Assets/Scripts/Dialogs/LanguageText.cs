using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageText : MonoBehaviour
{
    [TextArea(5, 20)]
    public string[] text;

    void Start() {
        Setup();
    }

    public void Setup() {
        if (PauseMenuManager.instance)
        {
            int l = PauseMenuManager.instance.languageIndex;
            if (l >= 0)
            {
                Text t0 = GetComponent<Text>();
                if (t0)
                {
                    t0.text = text[l];
                    return;
                }
                TextMeshPro t1 = GetComponent<TextMeshPro>();
                if (t1)
                {
                    t1.text = text[l];
                    return;
                }
                TextMeshProUGUI t2 = GetComponent<TextMeshProUGUI>();
                if (t2)
                {
                    t2.text = text[l];
                    return;
                }
            }
        }
    }
}
