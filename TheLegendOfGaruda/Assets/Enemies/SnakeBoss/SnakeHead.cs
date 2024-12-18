using UnityEngine;

public class SnakeHead : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject snakeBody;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Barrier"))
        {
            transform.GetComponentInParent<IStunnable>().Stun(2);
        }else if(other.collider.CompareTag("Orb")){
            other.collider.GetComponent<ProtectionOrb>().Absorb();
            transform.GetComponentInParent<SnakeManager>().AddBodyParts();
        }
    }

    public void Damage(float damage){
        transform.GetComponentInParent<SnakeManager>().health -= damage*10;
        if (transform.GetComponentInParent<SnakeManager>().health <= 0){
            Destroy(transform.parent.gameObject);
        }
    }
}
