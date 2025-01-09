using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage = 1;
    private PlayerHealth playerHealth;

    private void Start()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth == null)
            {
                Debug.LogWarning("PlayerHealth is null. Attempting to find the player again.");
                FindPlayer();
            }

            if (playerHealth != null)
            {
                Debug.Log("Player hit! Applying damage.");
                playerHealth.takeDamage(damage);
            }
        }
    }
}
