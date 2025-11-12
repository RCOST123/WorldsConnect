using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject winScreenCanvas;  // drag your WinScreenCanvas here

    private bool gameIsWon = false;

    void Awake()
    {
        if (winScreenCanvas != null)
            winScreenCanvas.SetActive(false); // ensure hidden at start
    }

    // Call this method when your win condition is met
    public void PlayerWins()
    {
        if (gameIsWon) return;
        gameIsWon = true;

        if (winScreenCanvas != null)
            winScreenCanvas.SetActive(true);

        Time.timeScale = 0f; // pause gameplay
    }

    // Hook these to buttons:
    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        // Make sure a scene named "MainMenu" is added to Build Settings
        SceneManager.LoadScene("Menu");
    }
}
