using System.Collections.Generic;
using UnityEngine;

public class mainBossDragon : MonoBehaviour
{
    [SerializeField] private float maxHealth = 3f;

    private float currentHealth;

    // List to track spawned enemies
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void RegisterEnemy(EnemyHealth enemy)
    {
        // Add the enemy's GameObject to the list of spawned enemies
        // spawnedEnemies.Add(enemy.gameObject);

        // Subscribe to the enemy's death event
        // enemy.OnEntityDeath += OnEntityDied;
    }

    private void OnEntityDied(EnemyHealth enemy)
    {
        // Unsubscribe from the event
        enemy.OnEntityDeath -= OnEntityDied;

        // Remove the enemy from the list
        spawnedEnemies.Remove(enemy.gameObject);

        // Reduce boss health
        Damage(1);
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Destroy all spawned enemies
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        // Clear the list to avoid references to destroyed objects
        spawnedEnemies.Clear();

        // Destroy the boss
        Destroy(gameObject);
    }
}