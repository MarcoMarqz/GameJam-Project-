using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponDisplayUI : MonoBehaviour
{
    public TextMeshProUGUI weaponNameText;
    private Coroutine displayRoutine;

    public void ShowWeaponName(string weaponName, float duration = 5f)
    {
        if (displayRoutine != null)
            StopCoroutine(displayRoutine);

        displayRoutine = StartCoroutine(DisplayRoutine(weaponName, duration));
    }

    private IEnumerator DisplayRoutine(string weaponName, float duration)
    {
        weaponNameText.text = "Weapon Granted: " + weaponName;
        weaponNameText.alpha = 1f;
        yield return new WaitForSeconds(duration);
        weaponNameText.alpha = 0f;
    }
}


