using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreditsRow : MonoBehaviour
{
    public string title;
    public string titleLink;
    public string by;
    public string byLink;
    public string license;
    public string licenseLink;
    public string notes;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI byText;
    public TextMeshProUGUI licenseText;
    public TextMeshProUGUI notesText;

    public TMP_StyleSheet stylesheet;

    // Start is called before the first frame update
    void OnValidate()
    {
        name = $"{title} Credits";
        // var height = 0f;
        var isLink = !string.IsNullOrWhiteSpace(titleLink);
        titleText.text = $"<style=\"{(isLink ? "Link" : "Normal")}\">{title}</style>";
        titleText.gameObject.SetActive(!string.IsNullOrWhiteSpace(title));
        titleText.styleSheet = stylesheet;
        // height += titleText.preferredHeight;

        isLink = !string.IsNullOrWhiteSpace(byLink);
        byText.text = $"<style=\"{(isLink ? "Link" : "Normal")}\">By: {by}</style>";
        byText.gameObject.SetActive(!string.IsNullOrWhiteSpace(by));
        byText.styleSheet = stylesheet;
        // height += byText.preferredHeight;


        isLink = !string.IsNullOrWhiteSpace(licenseLink);
        licenseText.text = $"<style=\"{(isLink ? "Link" : "Normal")}\">License: {license}</style>";
        licenseText.gameObject.SetActive(!string.IsNullOrWhiteSpace(license));
        licenseText.styleSheet = stylesheet;
        // height += licenseText.preferredHeight;

        notesText.text = notes;
        notesText.gameObject.SetActive(!string.IsNullOrWhiteSpace(notes));
        notesText.styleSheet = stylesheet;
        // height += notesText.preferredHeight;

        // var rt = GetComponent<RectTransform>();
        // rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
    }

    public void OnTitleClicked()
    {
        if (!string.IsNullOrWhiteSpace(titleLink))
        {
            Application.OpenURL(titleLink);
        }
    }

    public void OnByClicked()
    {
        if (!string.IsNullOrWhiteSpace(byLink))
        {
            Application.OpenURL(byLink);
        }
    }

    public void OnLicenseClicked()
    {
        if (!string.IsNullOrWhiteSpace(licenseLink))
        {
            Application.OpenURL(licenseLink);
        }
    }


    // Update is called once per frame
    void Update()
    {
    }
}