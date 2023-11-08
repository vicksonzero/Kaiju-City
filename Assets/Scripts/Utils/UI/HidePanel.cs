using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidePanel : MonoBehaviour
{
    // TODO: perhaps require a CanvasGroup, and just hide it altogether?
    public bool startVisible;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        // BuildNumberSO.GetAsset(so => Debug.Log($"build number: {(so ? so.buildNumber : "")}"));
        if (button) button.onClick.AddListener(() => Hide());
        Toggle(startVisible);
    }

    public void Show() => Toggle(true);
    public void Hide() => Toggle(false);

    public void Toggle(bool value)
    {
        gameObject.SetActive(value);
    }
}