using UnityEngine;

public class SnakeHead : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject snakeBody;
    private HitFlash HitFlash;
    public BossHealthUI HealthBar;

    private void Awake()
    {
        HitFlash = GetComponent<HitFlash>();
        if(HitFlash == null){
            HitFlash = gameObject.AddComponent<HitFlash>();
        }
        if (HealthBar == null)
        {
            HealthBar = FindFirstObjectByType<BossHealthUI>();
            if (HealthBar == null)
            {
                Debug.LogError("HealthBar is not found in the scene!");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Barrier"))
        {
            transform.GetComponentInParent<IStunnable>().Stun(2);
        }else if(other.collider.CompareTag("Orb")){
            Debug.Log("Orb absorbed");
            other.collider.GetComponent<ProtectionOrb>().Absorb();
            transform.GetComponentInParent<SnakeManager>().AddBodyParts();
        }
    }

    public void Damage(float damage)
    {
        HitFlash.TriggerFlash(0.05f);
        FindAnyObjectByType<HitStop>().Stop(0.03f);

        SnakeManager snakeManager = transform.GetComponentInParent<SnakeManager>();
        if (snakeManager != null)
        {
            snakeManager.health -= damage;
            HealthBar.SetHealth(snakeManager.health);

            if (snakeManager.health <= 0)
            {
                if (snakeManager.timelineDirector != null)
                {
                    snakeManager.timelineDirector.Play();
                }

                Destroy(transform.parent.gameObject);
            }
        }
    }
}
