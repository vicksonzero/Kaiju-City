using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class GamePause : MonoBehaviour
{
    private static GamePause _inst;
    public static GamePause Inst => _inst ??= FindObjectOfType<GamePause>();

    public bool isPaused = false;

    [Header("References")]
    public AudioMixerSnapshot pausedAudioSnapshot;

    public AudioMixerSnapshot unpausedAudioSnapshot;

    private void Start()
    {
        DOTween.defaultTimeScaleIndependent = true;
    }

    public void TogglePause(bool? value = null)
    {
        if (value is null) value = !isPaused;
        if (value!.Value) Pause();
        else Unpause();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pausedAudioSnapshot.TransitionTo(1f);
        isPaused = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        unpausedAudioSnapshot.TransitionTo(1f);
        isPaused = false;
    }
}