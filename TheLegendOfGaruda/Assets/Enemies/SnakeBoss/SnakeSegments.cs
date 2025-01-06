using UnityEngine;

public class SnakeSegments : MonoBehaviour, IDamageable
{
    private HitFlash HitFlash;
    public BossHealthUI HealthBar;

    private void Start()
    {
        HitFlash = gameObject.GetComponent<HitFlash>();
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

    public void Damage(float damage){
        HitFlash.TriggerFlash(0.05f);
        FindAnyObjectByType<HitStop>().Stop(0.03f);
        transform.GetComponentInParent<SnakeManager>().health -= damage;
        float health = transform.GetComponentInParent<SnakeManager>().health;
        HealthBar.SetHealth(health);
        if (transform.GetComponentInParent<SnakeManager>().health <= 0){
            Destroy(transform.parent.gameObject);
        }
    }
}
