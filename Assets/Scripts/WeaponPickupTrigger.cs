using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupTrigger : MonoBehaviour
{
    public WeaponManager weaponManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            weaponManager.GiveRandomWeapon();
            // Optionally: gameObject.SetActive(false);
        }
    }
}

