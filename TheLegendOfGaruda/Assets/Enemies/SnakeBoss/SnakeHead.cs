using UnityEngine;

public class SnakeHead : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject snakeBody;
    private HitFlash HitFlash;

    private void Awake()
    {
        HitFlash = GetComponent<HitFlash>();
        if(HitFlash == null){
            HitFlash = gameObject.AddComponent<HitFlash>();
        }
    }

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
        HitFlash.TriggerFlash(0.05f);
        FindAnyObjectByType<HitStop>().Stop(0.03f);
        transform.GetComponentInParent<SnakeManager>().health -= damage*10;
        if (transform.GetComponentInParent<SnakeManager>().health <= 0){
            Destroy(transform.parent.gameObject);
        }
    }
}
