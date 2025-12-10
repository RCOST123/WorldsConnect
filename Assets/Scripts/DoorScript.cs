using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int keysRequired = 3;
    private PlayerController player;
    private bool doorUnlocked = false;
    public GameObject unlocked_door;
    public AudioSource doorSound;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        unlocked_door.SetActive(false);
        doorSound = GameObject.Find("Door_Sound").GetComponent<AudioSource>();
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
        doorSound.Play();
        ///AudioSource.PlayClipAtPoint(doorSound, transform.position);
        Destroy(gameObject); // Or disable the door collider
    }
}