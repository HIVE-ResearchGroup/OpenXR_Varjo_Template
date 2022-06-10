using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    /**
     * Short script for ending preview
     */
    public class QuitPreview : MonoBehaviour
    {

        public InputAction quitAction;
        // Start is called before the first frame update


        private void OnEnable()
        {
            quitAction.Enable();
        }

        private void OnDisable()
        {
            quitAction.Disable();
        }

        private void Awake()
        {
            quitAction.started += ctx =>
            {
#if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
#else
                     Application.Quit();
#endif
            };
        }
    }
}
