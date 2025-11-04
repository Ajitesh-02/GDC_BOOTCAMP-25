using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private Transform firePoint; // set this to the tip of your gun or player

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleShooting();
    }

    void HandleMovement()
    {
        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveDir += Vector3.left;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            moveDir += Vector3.right;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveDir += Vector3.up;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveDir += Vector3.down;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void HandleRotation()
    {
        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate direction from player to mouse
        Vector2 direction = mousePos - transform.position;

        // Compute angle and apply rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;

        Vector3 spawnPos = firePoint ? firePoint.position : transform.position;

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, firePoint ? firePoint.rotation : transform.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0;

            // Bullet moves in the direction the player is facing
            rb.velocity = firePoint ? firePoint.up * bulletSpeed : transform.up * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("No Rigidbody2D found on bulletPrefab!");
        }
    }
}