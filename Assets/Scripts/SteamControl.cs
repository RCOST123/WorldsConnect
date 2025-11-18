using UnityEngine;

public class SteamControl : MonoBehaviour
{
    public PlayerController player;
    public GameObject steambottomtrigger;
    public GameObject steammiddletrigger;
    public GameObject steamtoptrigger;
    public AudioClip windSound;
    public int minkey = 1;
    public int i = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        steambottomtrigger.SetActive(false);
        steammiddletrigger.SetActive(false);
        steamtoptrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetKeyCount() >= minkey)
        {
            Debug.Log("Steam on");
            steambottomtrigger.SetActive(true);
            steammiddletrigger.SetActive(true);
            steamtoptrigger.SetActive(true);
            if (i == 1)
            {
                AudioSource.PlayClipAtPoint(windSound, transform.position);
                i = i - 1;
            }
        }
    }
}
