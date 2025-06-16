using UnityEngine.SceneManagement;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
