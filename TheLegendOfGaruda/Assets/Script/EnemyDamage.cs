using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage = 1;
    private PlayerHealth playerHealth;
    public void Start(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
           playerHealth = player.GetComponent<PlayerHealth>();
           if (playerHealth == null)
            {
                Debug.LogError("PlayerHealth component is missing on the Player GameObject!");
            }
        }
        else
        {
            Debug.LogError("No GameObject with the 'Player' tag found in the scene!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player"){
            if (playerHealth != null)
            {
                Debug.Log("Player hit! Applying damage.");
                playerHealth.takeDamage(damage);
            }
            else
            {
                Debug.LogError("playerHealth is null. Damage cannot be applied.");
            }
        }
    }
}
