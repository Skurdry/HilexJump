using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void StartLevel(int level)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
