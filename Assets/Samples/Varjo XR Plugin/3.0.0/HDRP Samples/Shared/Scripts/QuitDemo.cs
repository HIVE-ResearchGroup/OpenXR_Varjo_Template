using UnityEngine;

public class QuitDemo : MonoBehaviour
{
    public KeyCode quitPreview = KeyCode.Escape;
    void Update()
    {
        if (Input.GetKey(quitPreview))
        {
            #if UNITY_EDITOR
                        // Application.Quit() does not work in the editor so
                        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                     Application.Quit();
            #endif
        }
    }
}