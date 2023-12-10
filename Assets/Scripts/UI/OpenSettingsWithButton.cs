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
            settingsPanel.gameObject.SetActive(!settingsPanel.gameObject.activeInHierarchy);
            _inputs.settings = false;
        }
    }
}