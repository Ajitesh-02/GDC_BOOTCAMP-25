using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start()
    {
        // Destroy after 3 seconds to avoid clutter
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Destroy the enemy
            Destroy(other.gameObject);

            // Destroy the bullet itself
            Destroy(gameObject);
        }
    }
}