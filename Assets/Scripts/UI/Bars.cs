using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class Bars : MonoBehaviour
{
    public string title;

    [SerializeField]
    private float value = 100;

    public float valueMax = 100;
    public float width;
    public int decimalPlaces = 0;
    public bool showValueLabel = true;
    public float transitionDelay = 1;
    public float transitionDuration = 0.3f;

    public float Percentage => value / valueMax;


    [Header("Reference")]
    public TextMeshProUGUI titleLabel;

    public TextMeshProUGUI valueLabel;

    public RectTransform bar;
    public RectTransform barBg;
    public RectTransform barTransition;

    [CanBeNull]
    private Tween _transitionTween;

    private void OnValidate()
    {
        titleLabel.text = title;
        valueLabel.gameObject.SetActive(showValueLabel);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (width == 0)
        {
            width = barBg.sizeDelta.x;
        }

        valueLabel.gameObject.SetActive(showValueLabel);
    }

    public void SetValue(float val) => SetValue(val, null);

    public void SetValue(float val, float? valMax)
    {
        if (valMax.HasValue)
        {
            valueMax = valMax.Value;
        }

        value = val;
        valueLabel.text = value.ToString($"F{decimalPlaces}");
        bar.sizeDelta = new Vector2(width * Percentage, bar.sizeDelta.y);

        if (_transitionTween != null) _transitionTween.Kill();
        _transitionTween = DOVirtual.Float(
                barTransition.sizeDelta.x,
                width * Percentage,
                transitionDuration,
                x => barTransition.sizeDelta = new Vector2(x, bar.sizeDelta.y)
            )
            .SetDelay(transitionDelay);
    }
}