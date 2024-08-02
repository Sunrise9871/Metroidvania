using UnityEngine;
using UnityEngine.UI;

namespace UI.FullscreenCanvas
{
    [RequireComponent(typeof(Button))]
    public class QuitGameButton : MonoBehaviour
    {
        private Button _button;

        private void Awake() => _button = GetComponent<Button>();

        private void OnEnable() => _button.onClick.AddListener(QuitGame);

        private void OnDisable() => _button.onClick.RemoveListener(QuitGame);

        private void QuitGame()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}