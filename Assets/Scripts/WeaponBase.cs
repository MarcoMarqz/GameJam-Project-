using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName;
    public abstract void UseWeapon(); // Called to use the weapon (shoot, stab, etc.)
}
