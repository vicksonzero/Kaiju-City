using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DicksonMd.Utils
{
    public class ButtonLeave : MonoBehaviour
    {
        public Button button;

        public enum BackTo
        {
            ExitGame,
            nextScene
        };

        public BackTo onPressButton;
        public string sceneName;


        // Use this for initialization
        void Start()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }

            button.onClick.AddListener(() =>
            {
                if (onPressButton == BackTo.ExitGame)
                {
                    Application.Quit();
                }
                else
                {
                    SceneManager.LoadScene(sceneName);
                }
            });
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}