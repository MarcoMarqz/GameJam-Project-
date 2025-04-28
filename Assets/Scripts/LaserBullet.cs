using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public float lifetime = 1f; // How long bullet exists before disappearing

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(1); // Deal 1 hit
            }

            Destroy(gameObject); // Destroy bullet after hitting
        }
    }
}
