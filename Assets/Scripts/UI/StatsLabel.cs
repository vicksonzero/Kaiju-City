using System;
using System.Collections;
using System.Collections.Generic;
using EditorCools;
using TMPro;
using UnityEngine;

public class StatsLabel : MonoBehaviour
{
    public string key;

    public StatsType type = StatsType.Float;

    public TextMeshProUGUI text;

    public enum StatsType
    {
        Int,
        Float,
        Time,
    }

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    [Button()]
    public void Refresh()
    {
        if (string.IsNullOrWhiteSpace(key)) return;
        if (!text) text = GetComponent<TextMeshProUGUI>();
        if (!text) return;

        string value = type switch
        {
            StatsType.Float => PlayerPrefs.GetFloat(key, -1).ToString("0.00"),
            StatsType.Int => PlayerPrefs.GetInt(key, -1).ToString(),
            StatsType.Time => FormatTime(PlayerPrefs.GetFloat(key, -1)),
        };

        if (value == "-1" || value == "-1.00")
        {
            gameObject.SetActive(false);
            return;
        }

        text.text = value;
    }

    private static string FormatTime(float timeLeft)
    {
        if (timeLeft == -1) return "-1";
        return
            $"{Mathf.Floor(timeLeft / 60):00}:{Mathf.Floor(timeLeft % 60):00}.{(timeLeft - Math.Truncate(timeLeft)) * 1000:000}";
    }
}