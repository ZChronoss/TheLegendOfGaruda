using UnityEngine;

public class ProtectionBarrier : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Example: Prevent enemy damage
        if (other.collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
