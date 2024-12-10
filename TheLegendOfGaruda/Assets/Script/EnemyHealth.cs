using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;
    public delegate void EntityDeathHandler(EnemyHealth enemy);
    public event EntityDeathHandler OnEntityDeath;
    private void Start() {
        currentHealth = maxHealth;
    }


    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0){
            Die();
        }
    }

    public void Die(){
        OnEntityDeath?.Invoke(this);
        Destroy(gameObject);
    }
}
