using UnityEngine;

public class SnakeSegments : MonoBehaviour, IDamageable
{
    public void Damage(float damage){
        transform.GetComponentInParent<SnakeManager>().health -= damage;
        if (transform.GetComponentInParent<SnakeManager>().health <= 0){
            Destroy(transform.parent.gameObject);
        }
    }
}
