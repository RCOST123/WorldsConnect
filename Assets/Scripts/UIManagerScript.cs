using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TextMeshProUGUI pickupText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowPickupPrompt(string message, Transform target)
    {
        if (pickupText == null) return;

        pickupText.text = message;
        pickupText.gameObject.SetActive(true);
        pickupText.transform.position = target.position + Vector3.up * 1.5f;
    }

    public void HidePickupPrompt()
    {
        if (pickupText == null) return;
        pickupText.gameObject.SetActive(false);
    }
}
