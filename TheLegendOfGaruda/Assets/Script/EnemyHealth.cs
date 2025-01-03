using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable, IDataPersistence
{
    [SerializeField] private String id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    [SerializeField] private float maxHealth = 3f;
    private float currentHealth;
    public delegate void EntityDeathHandler(EnemyHealth enemy);
    public event EntityDeathHandler OnEntityDeath;
    private HitFlash HitFlash;
    private Boolean dead = false;

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
            if (UnityEngine.Random.Range(0f, 100f) <= item.dropChance)
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

    public void LoadData(GameData data)
    {
        data.enemies.TryGetValue(id, out dead);
        if (dead) 
        {
            Destroy(gameObject);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.enemies.ContainsKey(id))
        {
            data.enemies.Remove(id);
        }
        data.enemies.Add(id, dead);
    }
}
