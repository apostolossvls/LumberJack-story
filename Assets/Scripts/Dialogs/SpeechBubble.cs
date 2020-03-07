using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    [HideInInspector]
    public Dialog dialog;
    [TextArea(15,20)]
    public string[] message;
    public TextMeshPro textMeshPro;
    public Text textUI;
    public Image bubbleImage;
    public float speed; //0 instant'
    public float lifeTime=5;
    public Transform followParent;
    public Vector3 parentOffset = new Vector3(0,2,0);
    int charIndex;

    void OnEnable()
    {
        StopCoroutine(ShowText());
        StartCoroutine(ShowText());
    }

    void OnDisable(){
        StopCoroutine(DestroyAfterShow());
    }

    IEnumerator ShowText(){
        int languageIndex = dialog.languageIndex;
        if (followParent) {
            transform.parent = followParent;
            transform.localPosition = Vector3.zero + parentOffset;
        }
        if (speed==0){
            if (textMeshPro.isActiveAndEnabled) textMeshPro.text = message[languageIndex];
            else if (textUI.isActiveAndEnabled) textUI.text = message[languageIndex];
            else yield return null;
        }
        else {
            if (textMeshPro.isActiveAndEnabled) textMeshPro.text = "";
            else if (textUI.isActiveAndEnabled) textUI.text = "";
            else yield return null;;
            charIndex = 0;
            while (charIndex < message[languageIndex].Length){
                if (textMeshPro.isActiveAndEnabled) {
                    textMeshPro.text += message[languageIndex][charIndex];
                }
                else if (textUI.isActiveAndEnabled) {
                    textUI.text += message[languageIndex][charIndex];
                }
                charIndex++;
                yield return new WaitForSeconds(0.05f/speed);
            }
        }
        StartCoroutine(DestroyAfterShow());
        yield return null;
    }

    IEnumerator DestroyAfterShow(){
        yield return new WaitForSeconds(lifeTime);
        dialog.OnBubbleEnd();
        Destroy(gameObject);
        yield return null;
    }
}
