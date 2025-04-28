using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : WeaponBase  // Inherits from WeaponBase for compatibility with WeaponManager
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletspeed = 10;

    public AudioClip shootSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Only shoot if THIS GUN is active
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || bulletSpawnPoint == null)
        {
            Debug.LogError("Gun: bulletPrefab or bulletSpawnPoint is not assigned!");
            return;
        }
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * bulletspeed;
        }
        else
        {
            Debug.LogError("Gun: bulletPrefab is missing a Rigidbody!");
        }

        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    // Optional: required by WeaponBase, even if not used
    public override void UseWeapon()
    {
        // You can call Shoot() here if you want compatibility
        Shoot();
    }
}
