using UnityEngine;

namespace DicksonMd.Utils
{
    public class ToggleCursorOnStart : MonoBehaviour
    {
        public bool isShowCursor;

        private void Start()
        {
            SetCursorState(isShowCursor);
        }

        private void SetCursorState(bool newState)
        {
#if (UNITY_IOS || UNITY_ANDROID)
#else
            Cursor.lockState = !newState ? CursorLockMode.Locked : CursorLockMode.None;
#endif
            // Cursor.visible = false;
        }
    }
}