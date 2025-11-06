using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject flagPrefab;

    [Header("Settings")]
    public int initialKillGoal = 5;
    public float teleportCooldown = 3f;

    private static Checkpoint instance;

    private List<Vector3> checkpoints = new List<Vector3>();
    private int killCount = 0;
    private int killGoal;
    private int currentCheckpointIndex = -1;
    private bool canTeleport = true;

    // ✅ Total kills across all missions
    private int totalKills = 0;
    public static int TotalKills => instance ? instance.totalKills : 0; // getter

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        killGoal = initialKillGoal;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && canTeleport && checkpoints.Count > 0)
        {
            TeleportToNextCheckpoint();
        }
    }

    // Called by enemies when they die
    public static void ReportEnemyKilled(Vector3 position)
    {
        if (instance != null)
            instance.OnEnemyKilled(position);
    }

    private void OnEnemyKilled(Vector3 position)
    {
        killCount++;
        totalKills++; // ✅ increase total kills

        Debug.Log("Kills so far: " + totalKills + " | Current goal: " + killGoal);

        if (killCount >= killGoal)
        {
            killCount = 0;

            // ✅ Double kill goal but clamp it to max 20
            killGoal = Mathf.Min(killGoal * 2, 20);

            // Spawn checkpoint at last enemy’s position
            GameObject flag = Instantiate(flagPrefab, position, Quaternion.identity);
            checkpoints.Add(flag.transform.position);

            Debug.Log("New mission started! Next kill goal: " + killGoal);
        }
    }

    private void TeleportToNextCheckpoint()
    {
        if (checkpoints.Count == 0) return;

        currentCheckpointIndex = (currentCheckpointIndex + 1) % checkpoints.Count;
        player.position = checkpoints[currentCheckpointIndex];

        StartCoroutine(TeleportCooldown());
    }

    private IEnumerator TeleportCooldown()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }
}