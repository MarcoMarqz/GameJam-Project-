using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public List<GameObject> weaponPrefabs; // <-- GameObject not WeaponBase!
    private WeaponBase currentWeapon;
    public Transform weaponHoldPoint;
    public WeaponDisplayUI weaponDisplayUI; // UI display reference

    public void GiveRandomWeapon()
    {
        if (currentWeapon != null) Destroy(currentWeapon.gameObject);

        int index = Random.Range(0, weaponPrefabs.Count);
        GameObject weaponObj = Instantiate(weaponPrefabs[index], weaponHoldPoint.position, weaponHoldPoint.rotation, weaponHoldPoint);
        WeaponBase weapon = weaponObj.GetComponent<WeaponBase>();

        currentWeapon = weapon;

        // Show weapon name on screen for 5 seconds
        if (weaponDisplayUI != null)
            weaponDisplayUI.ShowWeaponName(weapon.weaponName, 5f);
    }

    public void UseCurrentWeapon()
    {
        if (currentWeapon != null)
            currentWeapon.UseWeapon();
    }
}
