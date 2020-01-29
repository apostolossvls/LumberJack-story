using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    [TextArea(15,20)]
    public string message;
    public TextMeshPro textMeshPro;
    public Text textUI;
    public Image bubbleImage;
    public float speed; //0 instant
    int charIndex;

    void OnEnable()
    {
        
        if (speed==0){
            if (textMeshPro.isActiveAndEnabled) textMeshPro.text = message;
            else if (textUI.isActiveAndEnabled) textUI.text = message;
            else return;
        }
        else {
            if (textMeshPro.isActiveAndEnabled) textMeshPro.text = "";
            else if (textUI.isActiveAndEnabled) textUI.text = "";
            else return;
            charIndex = 0;
        }
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText(){
        if (speed!=0){
            charIndex = 0;
            while (charIndex < message.Length){
                if (textMeshPro.isActiveAndEnabled) {
                    textMeshPro.text += message[charIndex];
                }
                else if (textUI.isActiveAndEnabled) {
                    textUI.text += message[charIndex];
                }
                charIndex++;
                yield return new WaitForSeconds(0.05f/speed);
            }
        }
        yield return null;
    }
}
