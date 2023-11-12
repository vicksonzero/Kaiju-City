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

    // Start is called before the first frame update
    void OnValidate()
    {
        name = $"{title} Credits";
        // var height = 0f;
        var className = !string.IsNullOrWhiteSpace(titleLink) ? "Link" : "Normal";
        titleText.text = $"<style=\"{className}\">{title}</style>";
        titleText.gameObject.SetActive(!string.IsNullOrWhiteSpace(title));
        // height += titleText.preferredHeight;

        className = !string.IsNullOrWhiteSpace(byLink) ? "Link" : "Normal";
        byText.text = $"<style=\"{className}\">By: {by}</style>";
        byText.gameObject.SetActive(!string.IsNullOrWhiteSpace(by));
        // height += byText.preferredHeight;


        className = !string.IsNullOrWhiteSpace(licenseLink) ? "Link" : "Normal";
        licenseText.text = $"<style=\"{className}\">License: {license}</style>";
        licenseText.gameObject.SetActive(!string.IsNullOrWhiteSpace(license));
        // height += licenseText.preferredHeight;

        notesText.text = notes;
        notesText.gameObject.SetActive(!string.IsNullOrWhiteSpace(notes));
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