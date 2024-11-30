using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int health;

    public HealthUI healthUI;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthUI.SetMaxHeart(maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            takeDamage(1);
        }
    }

    public void takeDamage(int damage){
        health -= damage;
        healthUI.UpdateHearts(health);

        if(health<=0){
            Destroy(gameObject);
        }
    }
}
