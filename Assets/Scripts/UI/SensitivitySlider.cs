using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    public Vector2 value;
    public Vector2 sliderMin;
    public Vector2 sliderMax = Vector2.one;
    public Vector2 scale = Vector2.one;
    public Ease easingType;

    [Header("Output")]
    public UnityEvent<Vector2> valueChanged;

    [Header("Reference")]
    public Slider xSlider;
    public Slider ySlider;
    public Toggle xInvertToggle;
    public Toggle yInvertToggle;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}