using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public RectTransform winLabel;
    public RectTransform loseLabel;

    // Start is called before the first frame update
    void Start()
    {
        winLabel.gameObject.SetActive(false);
        loseLabel.gameObject.SetActive(false);
    }

    public void Show(bool isWin)
    {
        Cursor.lockState = CursorLockMode.None;

        if (gameObject) gameObject.SetActive(true);
        winLabel.gameObject.SetActive(isWin);
        loseLabel.gameObject.SetActive(!isWin);
    }
}