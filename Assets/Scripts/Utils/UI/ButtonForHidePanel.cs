using System;
using UnityEngine;
using UnityEngine.UI;

namespace DicksonMd.Utils
{
    [RequireComponent(typeof(Button))]
    public class ButtonForHidePanel : MonoBehaviour
    {
        public HidePanel panel;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => panel.Toggle(true));
        }
    }
}