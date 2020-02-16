using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Sound[] sounds;
    public Transform parent;

    void Awake(){
        foreach (Sound s in sounds)
        {
            s.source = parent.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play (string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s==null) {
            Debug.LogWarning("Sound: "+name+" not found");
            return;
        } 
        s.source.Play();
    }

    public void BypassEffectsMusic (bool f){
        if (f) audioMixer.SetFloat("MusicDriMix", 0);
        else audioMixer.SetFloat("MusicDriMix", 100);
    }
}
