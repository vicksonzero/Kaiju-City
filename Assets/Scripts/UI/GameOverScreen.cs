using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public RectTransform winLabel;
    public RectTransform winOkButton;
    public RectTransform loseLabel;
    public RectTransform loseOkButton;

    // Start is called before the first frame update
    void Start()
    {
        winLabel.gameObject.SetActive(false);
        winOkButton.gameObject.SetActive(false);
        loseLabel.gameObject.SetActive(false);
        loseOkButton.gameObject.SetActive(false);
    }

    public void Show(bool isWin)
    {
        Cursor.lockState = CursorLockMode.None;

        if (gameObject) gameObject.SetActive(true);
        winLabel.gameObject.SetActive(isWin);
        DOVirtual.DelayedCall(2f, () => winOkButton.gameObject.SetActive(true));

        loseLabel.gameObject.SetActive(!isWin);
        DOVirtual.DelayedCall(2f, () => loseOkButton.gameObject.SetActive(true));
    }
}