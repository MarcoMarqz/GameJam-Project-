using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;                 // Assign your player in the Inspector
    public float followRadius = 15f;         // How far away the enemy can sense/follow
    public float attackRadius = 8f;          // When to stop & shoot instead of chase
    public float moveSpeed = 4f;             // Enemy move speed
    public GameObject bulletPrefab;          // Assign in Inspector (sphere/cube with Rigidbody)
    public Transform firePoint;              // Where bullets are spawned (empty child in front of enemy)
    public float bulletSpeed = 14f;
    public float shootCooldown = 1f;         // Time between shots
    public int health = 50;                  // Enemy health

    private float shootTimer = 0f;
    private Rigidbody rb;                    // Reference to the enemy's Rigidbody

    void Start()
    {
        // If player reference is not set, try to find it automatically
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
                Debug.LogWarning("Enemy script: Player not assigned and not found with 'Player' tag!");
        }

        // Get rigidbody if available for physics-based movement
        rb = GetComponent<Rigidbody>();

        // Initialize the shoot timer
        shootTimer = Random.Range(0f, shootCooldown); // Randomize initial fire time
    }

    void Update()
    {
        if (player == null) return;

        // Handle shooting timer
        if (shootTimer > 0)
            shootTimer -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, player.position);

        // Always face the player when within follow radius
        if (distance <= followRadius)
            FacePlayer();

        // Movement and shooting behaviors
        if (distance <= attackRadius)
        {
            // Stop and shoot at player
            ShootAtPlayer();
        }
        else if (distance <= followRadius)
        {
            // Move towards the player
            MoveTowardsPlayer();
        }
        // else, do nothing if out of radius
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // If using physics (recommended), use forces
        if (rb != null && !rb.isKinematic)
        {
            rb.velocity = direction * moveSpeed;
        }
        else
        {
            // Otherwise use Transform-based movement
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void FacePlayer()
    {
        Vector3 lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0; // Keep only horizontal rotation
        if (lookDir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10 * Time.deltaTime);
        }
    }

    void ShootAtPlayer()
    {
        // Return if cooldown not finished or missing components
        if (shootTimer > 0 || bulletPrefab == null || firePoint == null) return;

        // Reset cooldown
        shootTimer = shootCooldown;

        // Create and fire bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Add script to auto-destroy bullet after time
        Destroy(bullet, 5f);

        // Set bullet velocity
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            // Zero out any existing velocity and add our controlled velocity
            bulletRb.velocity = Vector3.zero;
            bulletRb.AddForce(firePoint.forward * bulletSpeed, ForceMode.VelocityChange);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            // Optional: Add death effects here
            // Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    // Optional: Visualize the follow and attack radii in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}