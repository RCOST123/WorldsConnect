using UnityEngine;

public class WinTester : MonoBehaviour
{
    private GameManager gm;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gm != null)
                gm.PlayerWins();
        }
    }
}
