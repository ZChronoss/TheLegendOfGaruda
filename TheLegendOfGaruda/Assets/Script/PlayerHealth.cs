using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int health;
    public float invincibilityDuration = 1.5f;
    private float invincibilityDeltaTime = 0.4f;
    private SpriteRenderer spriteRenderer;
    public HealthUI healthUI;
    private HitFlash HitFlash;
    private bool isInvincible = false;

    void Awake()
    {
        healthUI = FindAnyObjectByType<HealthUI>();
        // MARK: Set current healthnya ada di function ResetHealth() 
        ResetHealth();
        // healthUI.UpdateHearts(health);
        spriteRenderer = GetComponent<SpriteRenderer>();
        HitFlash = GetComponent<HitFlash>();
        if(HitFlash == null){
            HitFlash = gameObject.AddComponent<HitFlash>();
        }
    }

    public void Heal(int amount)
    {
        health += amount;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
         
        healthUI.UpdateHearts(health);
    }

    public void takeDamage(int damage){
        if(!isInvincible){
            HitFlash.TriggerFlash(0.1f);
            FindAnyObjectByType<HitStop>().Stop(0.05f);
            health -= damage;
            if (healthUI){
                healthUI.UpdateHearts(health);
            }
            if(health <= 0){
                Destroy(gameObject);
            }
            StartCoroutine(BecomeTemporarilyInvincible(invincibilityDuration));
        }
    }

    public IEnumerator BecomeTemporarilyInvincible(float invincibilityDuration)
    {
        isInvincible = true;

        for (float i = 0; i < invincibilityDuration; i += invincibilityDeltaTime)
        {
            HitFlash.TriggerFlash(0.2f);
            yield return new WaitForSeconds(invincibilityDeltaTime);
        }
        isInvincible = false;
    }

    void ResetHealth()
    {
        // TODO: Ini cuma buat test healing system, kalo udah mau release jangan lupa di max Health
        health = maxHealth;
        if(healthUI){
            healthUI.SetMaxHeart(maxHealth);
        }
    }
}
