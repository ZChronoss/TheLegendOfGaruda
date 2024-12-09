using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int health;

    private SpriteRenderer spriteRenderer;

    public HealthUI healthUI;

    // Start is called before the first frame update
    void Start()
    {
        healthUI = FindAnyObjectByType<HealthUI>();
        // MARK: Set current healthnya ada di function ResetHealth() 
        ResetHealth();
        healthUI.UpdateHearts(health);
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            takeDamage(1);
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
        health -= damage;
        if (healthUI){
            healthUI.UpdateHearts(health);
        }

        if(health <= 0){
            Destroy(gameObject);
        }
    }

    void ResetHealth()
    {
        // TODO: Ini cuma buat test healing system, kalo udah mau release jangan lupa di max Health
        health = maxHealth;
        healthUI.SetMaxHeart(maxHealth);
    }
}
