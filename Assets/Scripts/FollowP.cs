using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Target to follow")]
    [Tooltip("Assign the player's Transform here (or tag your player as 'Player' to auto-detect)")]
    public Transform player; // Drag your Player GameObject here in the Inspector

    [Header("Follow settings")]
    [Tooltip("How fast the follower moves toward the player (units per second)")]
    public float followSpeed = 5f;

    [Tooltip("How far behind the player the follower stays")]
    public float followDistance = 2f;

    [Tooltip("Offset from the player’s position (e.g. above the head)")]
    public Vector3 offset = new Vector3(0, 1, 0);

    void Start()
    {
        // Automatically find the player if not assigned
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
            else
            {
                Debug.LogWarning("[FollowPlayer] No player assigned and no GameObject tagged 'Player' found!");
            }
        }
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Calculate target position
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position - direction * followDistance + offset;

        // Move toward target position at a constant speed
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }
}
