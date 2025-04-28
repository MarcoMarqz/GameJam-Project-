using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float maxDistance = 15f;
    public float returnSpeed = 18f;
    public int damage = 30;
    public Transform player;

    private Vector3 startPos;
    private bool returning = false;

    void Start() { startPos = transform.position; }

    void Update()
    {
        if (!returning)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Vector3.Distance(startPos, transform.position) > maxDistance)
                returning = true;
        }
        else
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * returnSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, player.position) < 1f)
                Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(damage);
        }
    }
}

