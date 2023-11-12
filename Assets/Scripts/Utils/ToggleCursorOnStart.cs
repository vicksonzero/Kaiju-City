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
            Cursor.lockState = !newState ? CursorLockMode.Locked : CursorLockMode.None;
            // Cursor.visible = false;
        }
    }
}