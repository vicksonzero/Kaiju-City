using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }

    [Header("Audio")]
    public AudioMixer mixer;

    [Header("Control")]
    public UIVirtualTouchZone lookControl;

    public UIVirtualTouchZone shootControl;
    public InputActionReference lookAction;
    public Vector2 pcLookScale = new(1.5f, -1);
    public Vector2 joystickLookScale = new(1, 1);
    public Vector2 trackpadLookScale = new(2, 1.2f);

    [Header("UI")]
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

    public string lookModeKey = "RStickLookMode";
    public TMP_Text lookModeToggleLabel;
    public string rStickSensitivityXKey = "RStickSensitivityX";
    public TMP_InputField rStickSensitivityXField;
    public string rStickSensitivityYKey = "RStickSensitivityY";
    public TMP_InputField rStickSensitivityYField;

    private CursorLockMode _oldLockState;

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

    public void ToggleLookMode([CanBeNull] string value)
    {
        lookModeToggleLabel.text = !string.IsNullOrEmpty(value)
            ? value
            : lookModeToggleLabel.text == "Trackpad"
                ? "Joystick"
                : "Trackpad";
        PlayerPrefs.SetString(lookModeKey, lookModeToggleLabel.text);
        // effect
        if (!lookControl) return;
        lookControl.absoluteAiming = (lookModeToggleLabel.text == "Trackpad");
        shootControl.absoluteAiming = (lookModeToggleLabel.text == "Trackpad");
    }

    public void SetLookSensitivityX(string value)
    {
        if (!float.TryParse(value, out var valueFloat))
        {
            valueFloat = lookControl.sensitivityModifiers.x;
            rStickSensitivityXField.text = valueFloat.ToString("0.00");
        }

        PlayerPrefs.SetFloat(rStickSensitivityXKey, valueFloat);
        // effect
        if (!lookControl) return;

        var lookMode = PlayerPrefs.GetString(lookModeKey, "Trackpad");
        var scale = lookMode == "Trackpad"
            ? trackpadLookScale
            : joystickLookScale;
        lookControl.sensitivityModifiers = new Vector2(
            valueFloat * scale.x,
            lookControl.sensitivityModifiers.y);

        shootControl.sensitivityModifiers = new Vector2(
            valueFloat * scale.x,
            shootControl.sensitivityModifiers.y);
        // lookAction.action.ApplyParameterOverride("scaleVector2:x", valueFloat * pcLookScale.x);
    }

    public void SetLookSensitivityY(string value)
    {
        if (!float.TryParse(value, out var valueFloat))
        {
            valueFloat = lookControl.sensitivityModifiers.y;
            rStickSensitivityYField.text = valueFloat.ToString("0.00");
        }

        PlayerPrefs.SetFloat(rStickSensitivityYKey, valueFloat);
        // effect
        if (!lookControl) return;

        var lookMode = PlayerPrefs.GetString(lookModeKey, "Trackpad");
        var scale = lookMode == "Trackpad"
            ? trackpadLookScale
            : joystickLookScale;
        lookControl.sensitivityModifiers = new Vector2(
            lookControl.sensitivityModifiers.x,
            valueFloat * scale.y);
        shootControl.sensitivityModifiers = new Vector2(
            shootControl.sensitivityModifiers.x,
            valueFloat * scale.y);
        // lookAction.action.ApplyParameterOverride("scaleVector2:y", valueFloat * pcLookScale.y);
    }

    public void SaveVolume()
    {
    }

    public void LoadSettings()
    {
        SetMasterVolume(masterSlider.value = PlayerPrefs.GetFloat(masterVolumeKey, 0.0001f));
        SetBgmVolume(bgmSlider.value = PlayerPrefs.GetFloat(bgmVolumeKey, 0.4f));
        SetSfxVolume(sfxSlider.value = PlayerPrefs.GetFloat(sfxVolumeKey, 1));
        SetUiVolume(uiSlider.value = PlayerPrefs.GetFloat(uiVolumeKey, 0.7f));

        ToggleLookMode(lookModeToggleLabel.text = PlayerPrefs.GetString(lookModeKey, "Trackpad"));
        SetLookSensitivityX(rStickSensitivityXField.text =
            PlayerPrefs.GetFloat(rStickSensitivityXKey, 2).ToString("0.00"));
        SetLookSensitivityY(rStickSensitivityYField.text =
            PlayerPrefs.GetFloat(rStickSensitivityYKey, 0.8f).ToString("0.00"));
    }

    private void OnEnable()
    {
        LoadSettings();
#if (UNITY_IOS || UNITY_ANDROID)
#else
        _oldLockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
#endif
    }

    private void OnDisable()
    {
#if (UNITY_IOS || UNITY_ANDROID)
#else
        Cursor.lockState = _oldLockState;
#endif
    }
}