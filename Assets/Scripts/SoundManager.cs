using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;


    public List<Sound> sounds;

    void Awake()
    {
        /*
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		*/
        updateSounds();

    }

    void clearNullSounds()
    {
        sounds.RemoveAll(null);
    }

    void removeUnused()
    {
        List<Sound> newSounds = GameObject.FindObjectsOfType<Sound>().ToList();

        foreach (Sound s in newSounds)
        {
            if (!sounds.Contains(s))
            {
                sounds.Remove(s);
                Destroy(s.source);
            }
        }
    }


    void updateSounds()
    {
        List<Sound> newSounds = GameObject.FindObjectsOfType<Sound>().ToList();

        //adding new sounds
        foreach (Sound s in newSounds)
        {
            if (!sounds.Contains(s))
            {
                print("Found new sound, Adding " + s.name);
                sounds.Add(s);
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
                s.source.spatialBlend = s.spatialBlend;
                s.source.reverbZoneMix = s.reverbZoneMix;
                s.source.minDistance = s.minDistance;
                s.source.maxDistance = s.maxDistance;
            }


        }

        ///*
        //removing new sounds
        removeUnused();
        //*/

    }
    

    public void stopAllSound()
    {
        updateSounds();
        print("Stopping all sounds");
        foreach (Sound s in sounds)
        {
            //s.source.Stop();
            Stop(s.name);
        }
    }

    Sound findSound(string sound)
    {
        foreach (Sound s in sounds)
        {
            if (s.name.Equals(sound))
            {
                return s;
            }
        }
        Debug.Log("Could not find sound " + sound);
        return null;
    }

    public void Stop(string sound)
    {
        //Sound s = Array.Find(sounds, item => item.name == sound);
        Sound s = findSound(sound);
        if (s == null)
        {
            //updateSounds();
            //s = Array.Find(sounds, item => item.name == sound);
            s = findSound(sound);

            if (s == null)
            {
                Debug.LogWarning("Sound: " + sound + " not found!");
                return;

            }
        }
        if (s.source.isPlaying)
        {
            print("Sound Stopping: " + sound);
            s.source.Stop();

        }
    }

    public void Play(string sound)
    {
        //Sound s = Array.Find(sounds, item => item.name == sound);
        Sound s = findSound(sound);

        if (s == null)
        {
            updateSounds();

            //s = Array.Find(sounds, item => item.name == sound);
            s = findSound(sound);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + sound + " not found!");
                return;

            }
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

}
