using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioClipChooseOne : MonoBehaviour
{
    public AudioClip[] clips;

    private void Awake()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        if (audioSource.playOnAwake) audioSource.Play();
    }
}