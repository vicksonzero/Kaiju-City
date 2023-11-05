using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// using UnityEngine.UI;

namespace DicksonMd.Utils
{
    public class VersionNumber : MonoBehaviour
    {
        public Text label;
        public TextMeshProUGUI labelMesh;

        [Tooltip(
            "Available placeholders: %productName%, %version%, %buildNo%, %platform%, %companyName%, %platformFlags%, %datetime%")]
        public string template = "";


        // Start is called before the first frame update
        void Start()
        {
            if (label == null)
            {
                label = GetComponent<Text>();
            }

            if (labelMesh == null)
            {
                labelMesh = GetComponent<TextMeshProUGUI>();
            }

            if (template == "")
            {
                template = label ? label.text : labelMesh.text;
            }

            if (label)
                label.text = (template
                        .Replace("%version%", Application.version)
                        .Replace("%platform%", Application.platform.ToString())
                        .Replace("%companyName%", Application.companyName)
                        .Replace("%productName%", Application.productName)
                        .Replace("%platformFlags%", Application.isMobilePlatform ? " (Mobile)" : "")
                        .Replace("%datetime%", DateTime.Now.ToString("yyyyMMdd_hhmmss"))
                        .Replace("\\n", "\n")
                    );
            if (labelMesh)
                labelMesh.text = (template
                        .Replace("%version%", Application.version)
                        .Replace("%platform%", Application.platform.ToString())
                        .Replace("%companyName%", Application.companyName)
                        .Replace("%productName%", Application.productName)
                        .Replace("%platformFlags%", Application.isMobilePlatform ? " (Mobile)" : "")
                        .Replace("%datetime%", DateTime.Now.ToString("yyyyMMdd_hhmmss"))
                        .Replace("\\n", "\n")
                    );

            BuildNumberSO.GetAsset(so =>
            {
                var buildNumber = so ? so.buildNumber : 0;
                if (label)
                    label.text = (label.text
                            .Replace("%buildNo%", buildNumber.ToString("000"))
                        );
                if (labelMesh)
                    labelMesh.text = (labelMesh.text
                            .Replace("%buildNo%", buildNumber.ToString("000"))
                        );
            });
        }
    }
}