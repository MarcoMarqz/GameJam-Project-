using UnityEngine;
using UnityEngine.AI; // Needed for NavMeshAgent

public class Enemy : MonoBehaviour
{
    public Transform player;              // Reference to the player
    public float moveSpeed = 3f;           // Movement speed (optional, NavMesh controls this mostly)
    public float shootInterval = 2f;       // How often enemy shoots (in seconds)
    public GameObject bulletPrefab;        // What bullet to shoot
    public Transform bulletSpawnPoint;     // Where bullets spawn
    public float bulletSpeed = 10f;         // Speed of bullets

    private NavMeshAgent agent;
    private float shootTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("EnemyAI: No NavMeshAgent found on this enemy!");
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // Move toward the player
        agent.SetDestination(player.position);

        // Smoothly face the player (only horizontal, no looking up/down)
        Vector3 lookDirection = (player.position - transform.position);
        lookDirection.y = 0; // Stay flat
        if (lookDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }

        // Handle shooting at the player
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    void ShootAtPlayer()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null)
            return;

        // Calculate the direction
        Vector3 shootDirection = (player.position - bulletSpawnPoint.position).normalized;

        // Create a rotation that looks toward the player
        Quaternion shootRotation = Quaternion.LookRotation(shootDirection);

        // Spawn bullet with correct rotation
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, shootRotation);

        // (Optional) If using Rigidbody velocity, can still apply force if you want faster movement
        /*
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = shootDirection * bulletSpeed;
        }
        */
    }

}
