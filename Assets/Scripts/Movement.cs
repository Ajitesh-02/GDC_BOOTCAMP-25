using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f; // controls smoothness
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private Transform firePoint;

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
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Smooth rotation using Lerp
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void HandleShooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    void Shoot()
    {
        if (!bulletPrefab) return;

        Vector3 spawnPos = firePoint ? firePoint.position : transform.position;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, firePoint ? firePoint.rotation : transform.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0;
            rb.velocity = (firePoint ? firePoint.up : transform.up) * bulletSpeed;
        }
    }
}