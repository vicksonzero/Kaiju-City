using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace DicksonMd.Utils
{
    public class BackButtonLeave : MonoBehaviour
    {
        public KeyCode[] keyCodes =
        {
            KeyCode.Escape,
            KeyCode.Backspace,
        };

        public enum BackTo
        {
            ExitGame,
            nextScene
        };

        public BackTo onPressBackButton;
        public string sceneName;


        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            var keyCodeIterator = keyCodes;
            foreach (var keycode in keyCodeIterator)
            {
                if (Input.GetKeyDown(keycode))
                {
                    if (onPressBackButton == BackTo.ExitGame)
                    {
                        Application.Quit();
                    }
                    else
                    {
                        SceneManager.LoadScene(sceneName);
                    }
                }
            }
        }
    }
}