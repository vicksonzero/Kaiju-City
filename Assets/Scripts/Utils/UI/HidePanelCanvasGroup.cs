using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HideCanvasGroup : MonoBehaviour
{
    public bool startVisible;
    public Button button;
    private CanvasGroup _canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        // BuildNumberSO.GetAsset(so => Debug.Log($"build number: {(so ? so.buildNumber : "")}"));
        if (button) button.onClick.AddListener(() => Hide());
        Toggle(startVisible);
    }

    public void Show() => Toggle(true);
    public void Hide() => Toggle(true);

    public void Toggle(bool value)
    {
        _canvasGroup.alpha = value ? 1 : 0;
        _canvasGroup.interactable = value;
    }
}