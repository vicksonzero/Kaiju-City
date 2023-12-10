using UnityEngine;
using UnityEngine.UI;

namespace DicksonMd.Utils
{
    public class ButtonLink : MonoBehaviour
    {
        public Button button;
        public string url;


        // Use this for initialization
        void Start()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            button.onClick.AddListener(() =>
            {
                Application.OpenURL(url);
            });
        }

    }
}