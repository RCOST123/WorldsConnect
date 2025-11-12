using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string retrySceneName = "Level_1"; // your main level scene
    public string menuSceneName = "Menu";     // your main menu scene

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(retrySceneName);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
