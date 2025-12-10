using UnityEngine;

public class SteamControl : MonoBehaviour
{
    public PlayerController player;
    public GameObject steambottomtrigger;
    public GameObject steammiddletrigger;
    public GameObject steamtoptrigger;
    public AudioSource windSound;
    public int minkey = 1;
    public int i = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        steambottomtrigger.SetActive(false);
        steammiddletrigger.SetActive(false);
        steamtoptrigger.SetActive(false);
        windSound = GameObject.Find("Wind_Sound").GetComponent<AudioSource>();
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
                windSound.Play();
                //AudioSource.PlayClipAtPoint(windSound, transform.position);
                i = i - 1;
            }
        }
    }
}
