using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int keysToWin = 5;
    public GameObject door;
    private PlayerController player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (player != null && player.GetKeyCount() >= keysToWin)
        {
            // Handle level completion
            Debug.Log("Level Complete!");
        }
    }
}