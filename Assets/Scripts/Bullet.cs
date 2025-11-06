using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Notify checkpoint manager
            Checkpoint.ReportEnemyKilled(other.transform.position);

            Destroy(other.gameObject); // instantly destroy enemy
            Destroy(gameObject); // destroy bullet
        }
    }
}