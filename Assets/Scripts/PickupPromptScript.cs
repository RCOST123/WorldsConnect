using UnityEngine;

public enum SkillType { Wings, Claws, Dash, WaterBalloon }

public class SkillPickup : MonoBehaviour
{
    public SkillType skillType;
    private bool playerInRange = false;
    private PlayerController playerController;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        playerController = other.GetComponent<PlayerController>();

        Debug.Log("In Range of " + skillType);
        UIManager.Instance.ShowPickupPrompt("Press X to pick up " + skillType, transform);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        playerController = null;

        UIManager.Instance.HidePickupPrompt();
    }

    void Update()
    {
        if (playerInRange && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Joystick1Button2)))
        {
            if (playerController != null)
            {
                playerController.UnlockSkill(skillType);
                Destroy(gameObject);
                UIManager.Instance.HidePickupPrompt();
            }
        }
    }
}
