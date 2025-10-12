using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int keysRequired = 3;
    private PlayerController player;
    private bool doorUnlocked = false;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        if (!doorUnlocked && player != null && player.GetKeyCount() >= keysRequired)
        {
            UnlockDoor();
        }
    }

    void UnlockDoor()
    {
        doorUnlocked = true;
        Debug.Log("Door unlocked!");
        // Animate door, disable collider, load next scene, etc.
        Destroy(gameObject); // Or disable the door collider
    }
}