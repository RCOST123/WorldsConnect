using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int keysRequired = 3;
    private PlayerController player;
    private bool doorUnlocked = false;
    public GameObject unlocked_door;
    public AudioClip doorSound;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        unlocked_door.SetActive(false);
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
        unlocked_door.SetActive(true);
        AudioSource.PlayClipAtPoint(doorSound, transform.position);
        Destroy(gameObject); // Or disable the door collider
    }
}