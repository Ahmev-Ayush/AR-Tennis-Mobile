using UnityEngine;
using System;

public class AudioManagerScript : MonoBehaviour
{
    public static AudioManagerScript Instance;
    public Sound[] sounds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            // source is defined in Sound.cs
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found: " + name);
            return;
        }
        s.source.Play();
    }


}
