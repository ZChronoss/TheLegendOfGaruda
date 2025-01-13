using UnityEngine;

public class DeathCollision : MonoBehaviour
{
    [Header("Fall Damage Settings")]
    [SerializeField] private int fallDamage = 999; // Damage to instantly kill the player

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.takeDamage(fallDamage);
            }
        }
    }
}
