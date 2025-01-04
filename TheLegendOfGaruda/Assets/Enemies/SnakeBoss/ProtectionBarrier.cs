using UnityEngine;

public class ProtectionBarrier : MonoBehaviour
{
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate(){
        transform.position = player.transform.position;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            player.GetComponent<PlayerHealth>().removeInvulnerable();
        }
    }
}
