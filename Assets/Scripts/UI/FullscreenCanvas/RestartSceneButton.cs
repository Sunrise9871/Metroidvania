using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.FullscreenCanvas
{
    [RequireComponent(typeof(Button))]
    public class RestartGameButton : MonoBehaviour
    {
        private Button _button;

        private void Awake() => _button = GetComponent<Button>();

        private void OnEnable() => _button.onClick.AddListener(RestartGame);

        private void OnDisable() => _button.onClick.RemoveListener(RestartGame);

        private void RestartGame()
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScene);
        }
    }
}