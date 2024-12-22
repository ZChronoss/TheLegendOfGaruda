using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;
    public delegate void EntityDeathHandler(EnemyHealth enemy);
    public event EntityDeathHandler OnEntityDeath;
    private HitFlash HitFlash;
    private void Awake()
    {
        currentHealth = maxHealth;
        HitFlash = GetComponent<HitFlash>();
        if(HitFlash == null){
            HitFlash = gameObject.AddComponent<HitFlash>();
        }
    }

    public void Damage(float damageAmount)
    {
        HitFlash.TriggerFlash(0.05f);
        FindAnyObjectByType<HitStop>().Stop(0.03f);
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
