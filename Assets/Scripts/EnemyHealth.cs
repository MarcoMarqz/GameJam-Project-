using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    private Renderer enemyRenderer;
    private Color originalColor;
    private NavMeshAgent agent;

    public GameObject deathEffectPrefab;
    public GameObject lootPrefab;

    public string sceneToLoadOnDeath = "VictoryScene"; // 👈 Name of the scene you want to load after killing enemy

    void Start()
    {
        currentHealth = maxHealth;
        enemyRenderer = GetComponentInChildren<Renderer>();
        if (enemyRenderer != null)
            originalColor = enemyRenderer.material.color;

        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took " + damageAmount + " damage. Health left: " + currentHealth);

        if (enemyRenderer != null)
            StartCoroutine(FlashRed());

        if (currentHealth <= 0)
            Die();
    }

    private System.Collections.IEnumerator FlashRed()
    {
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            enemyRenderer.material.color = originalColor;
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        if (agent != null)
            agent.isStopped = true;

        if (deathEffectPrefab != null)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        if (lootPrefab != null)
            Instantiate(lootPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);

        SceneManager.LoadScene(sceneToLoadOnDeath);
    }

  
}

