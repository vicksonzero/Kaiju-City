using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }

    [Header("UI")]
    public AudioMixer mixer;

    public string masterVolumeKey = "MasterVolume";
    public Slider masterSlider;
    public RectTransform masterSliderInTitle;
    public RectTransform masterSliderInSettings;
    public string bgmVolumeKey = "BgmVolume";
    public Slider bgmSlider;
    public string sfxVolumeKey = "SfxVolume";
    public Slider sfxSlider;
    public string uiVolumeKey = "UiVolume";
    public Slider uiSlider;

    public enum AudioChannel
    {
        Master,
        Bgm,
        Sfx,
        Ui
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        MoveMasterVolumeToTitle();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void MoveMasterVolumeToTitle()
    {
        if (masterSliderInTitle)
            masterSlider.transform.SetParent(masterSliderInTitle, false);
    }

    public void MoveMasterVolumeToSettings()
    {
        if (masterSliderInSettings)
            masterSlider.transform.SetParent(masterSliderInSettings, false);
    }

    public void SetMasterVolume(float value)
    {
        PlayerPrefs.SetFloat(masterVolumeKey, value);
        mixer.SetFloat(masterVolumeKey, Mathf.Log10(value) * 20);
    }

    public void SetBgmVolume(float value)
    {
        PlayerPrefs.SetFloat(bgmVolumeKey, value);
        mixer.SetFloat(bgmVolumeKey, Mathf.Log10(value) * 20);
    }

    public void SetSfxVolume(float value)
    {
        PlayerPrefs.SetFloat(sfxVolumeKey, value);
        mixer.SetFloat(sfxVolumeKey, Mathf.Log10(value) * 20);
    }

    public void SetUiVolume(float value)
    {
        PlayerPrefs.SetFloat(uiVolumeKey, value);
        mixer.SetFloat(uiVolumeKey, Mathf.Log10(value) * 20);
    }

    public void SaveVolume()
    {
    }

    public void LoadVolume()
    {
        SetMasterVolume(masterSlider.value = PlayerPrefs.GetFloat(masterVolumeKey, 0.0001f));
        SetBgmVolume(bgmSlider.value = PlayerPrefs.GetFloat(bgmVolumeKey, 1));
        SetSfxVolume(sfxSlider.value = PlayerPrefs.GetFloat(sfxVolumeKey, 1));
        SetUiVolume(uiSlider.value = PlayerPrefs.GetFloat(uiVolumeKey, 1));
    }

    private void OnEnable()
    {
        LoadVolume();
#if (UNITY_IOS || UNITY_ANDROID)
#else
        Cursor.lockState = CursorLockMode.None;
#endif
    }

    private void OnDisable()
    {
#if (UNITY_IOS || UNITY_ANDROID)
#else
        Cursor.lockState = CursorLockMode.Locked;
#endif
    }
}