using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform pointA;      // first patrol point
    public Transform pointB;      // second patrol point
    public float speed = 2f;      // movement speed

    private Transform target;     // current target point

    void Start()
    {
        // Start moving toward point A first
        target = pointA;
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        // Move toward the current target point
        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Flip direction when the enemy reaches a point
        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            target = target == pointA ? pointB : pointA;
            Flip();
        }
    }

    void Flip()
    {
        // Reverse the enemy's X scale to face the other direction
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
