using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stopDistance = 0.8f;
    [SerializeField] private float damagePerSecond = 10f;

    private Transform player;
    private Rigidbody2D rb;
    private PHealth playerHealth;
    private bool touchingPlayer = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Face the player
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        // Stop moving when close enough
        float distance = Vector2.Distance(player.position, transform.position);
        if (distance > stopDistance && !touchingPlayer)
        {
            Vector2 newPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);
        }
        else
        {
            rb.velocity = Vector2.zero; // stay still when close
        }

        // Deal damage while close
        if (touchingPlayer && playerHealth != null)
        {
            playerHealth.TakeDamage(damagePerSecond * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
            playerHealth = collision.gameObject.GetComponent<PHealth>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
        }
    }
}