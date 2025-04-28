using UnityEngine;

public class Sword : WeaponBase
{
    public int damage = 10;
    public float swingCooldown = 0.5f;
    private float nextSwingTime = 0f;

    void Start()
    {
        weaponName = "Sword";
    }

    void Update()
    {
        if (gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.F) && Time.time >= nextSwingTime)
        {
            UseWeapon();
        }
    }

    public override void UseWeapon()
    {
        if (Time.time >= nextSwingTime)
        {
            Debug.Log("Sword swung!");
            transform.Rotate(0, 0, 45);
            Invoke("ResetRotation", 0.2f);
            nextSwingTime = Time.time + swingCooldown;
            DetectHits();
        }
    }

    void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }

    void DetectHits()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy with sword!");

                // ✅ Actually deal damage
                EnemyHealth enemyHealth = hitCollider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}

