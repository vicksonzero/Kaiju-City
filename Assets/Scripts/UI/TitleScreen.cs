using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public Settings settingsScreen;
    // Start is called before the first frame update
    void Start()
    {
        settingsScreen.MoveMasterVolumeToTitle();
        settingsScreen.LoadVolume();
    }

    public void StartGame()
    {

    }
}