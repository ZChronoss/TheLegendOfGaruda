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
        healthUI.UpdateHearts(health);

        if(health <= 0){
            Destroy(gameObject);
        }
    }

    void ResetHealth()
    {
        health = 1;
        healthUI.SetMaxHeart(maxHealth);
    }
}
