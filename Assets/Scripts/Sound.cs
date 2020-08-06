﻿using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound : MonoBehaviour
{
	
	public string name;
	public bool isUnique;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	[Range(0f, 1f)]
	public float spatialBlend = 1f;
	[Range(0f, 1.1f)]
	public float reverbZoneMix = 0f;

	public float minDistance = 10f;
	public float maxDistance = 20f;


	public bool loop = false;



	[HideInInspector]
	public AudioSource source;


    private void Awake()
    {
        if (isUnique)
        {
			name += transform.parent.name;
        }
    }
}
