using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class OpenSettingsWithButton : MonoBehaviour
{
    private StarterAssetsInputs _inputs;

    public Settings settingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        _inputs = FindObjectOfType<StarterAssetsInputs>();
        settingsPanel.LoadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputs.settings)
        {
            ShowSettings();
            _inputs.settings = false;
        }
    }

    public void ShowSettings()
    {
        var value = !settingsPanel.gameObject.activeInHierarchy;
        GamePause.Inst.TogglePause(value);
        settingsPanel.gameObject.SetActive(value);
    }

    public void HideSettings()
    {
        GamePause.Inst.Unpause();
    }
}