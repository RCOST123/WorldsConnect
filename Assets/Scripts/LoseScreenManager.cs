using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string menuSceneName = "Menu"; // your main menu scene

    private string retrySceneName;

    private void Start()
    {
        // Get the level we saved in PlayerScript.Die()
        retrySceneName = PlayerPrefs.GetString("LastLevel", "Level_1");
    }

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
