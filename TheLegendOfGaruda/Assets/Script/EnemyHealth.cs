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

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

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
        foreach (LootItem item in lootTable)
        {
            if (Random.Range(0f, 100f) <= item.dropChance)
            {
                InstantiateLoot(item.itemPrefab);
            }
        }

        OnEntityDeath?.Invoke(this);
        Destroy(gameObject);
    }

    void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
        }
    }
}
