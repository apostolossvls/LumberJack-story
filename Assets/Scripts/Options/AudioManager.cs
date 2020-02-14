using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource[] Music;
    public AudioSource[] UIaudio1;
    public AudioSource[] Heroaudio1;
    public AudioSource[] MenuBuildings;
    public AudioSource[] Notifications;

    public void BypassEffectsMusic (bool f){
        if (f) audioMixer.SetFloat("MusicDriMix", 0);
        else audioMixer.SetFloat("MusicDriMix", 100);
    }
}
