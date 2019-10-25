#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Variables
    public AudioMixerGroup audioMixerGroup;
    public Sound[] soundsCollection;

    public static AudioManager current;
    #endregion

    #region Unity's Functions
    private void Awake()
    {
        if(current == null)
        {
            current = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound sound in soundsCollection)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.loop = sound.loop;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.outputAudioMixerGroup = audioMixerGroup;
        }
    }

    private void Start()
    {
        Playsound("Music");
    }
    #endregion

    #region Functions
    public void Playsound(string name)
    {
        Sound sound = Array.Find(soundsCollection, s => s.name == name);
        if(sound != null)
        {
            sound.source.Play();
        }
    }
    #endregion
}
