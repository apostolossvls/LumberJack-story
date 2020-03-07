using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public int languageIndex;
    public Component[] bubbles;
    int bubbleIndex;

    void Start()
    {
        bubbleIndex = 0;
        List<Component> l = new List<Component>{}; 
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            Component c = t.GetComponent<SpeechBubble>();
            if (c) {
                l.Add(c);
                continue;
            }
            c = t.GetComponent<SpeechPause>();
            if (c) {
                l.Add(c);
            }
        }
        bubbles = l.ToArray();
        NextBubble();
    }

    void NextBubble(){
        if (bubbleIndex<bubbles.Length){
            if (bubbles[bubbleIndex] is SpeechBubble){
                SpeechBubble b = (SpeechBubble)bubbles[bubbleIndex];
                b.dialog = this;
            }
            else if (bubbles[bubbleIndex] is SpeechPause){
                SpeechPause b = (SpeechPause)bubbles[bubbleIndex];
                b.dialog = this;
            }
            bubbles[bubbleIndex].gameObject.SetActive(true);
            bubbleIndex++;
        }
    }

    public void OnBubbleEnd(){
        NextBubble();
    }
}
