using UnityEngine;

public class SnakeSegments : MonoBehaviour, IDamageable
{
    private HitFlash HitFlash;

    private void Start()
    {
        HitFlash = gameObject.GetComponent<HitFlash>();
        if(HitFlash == null){
            HitFlash = gameObject.AddComponent<HitFlash>();
        }
    }

    public void Damage(float damage){
        HitFlash.TriggerFlash(0.05f);
<<<<<<< HEAD
        FindAnyObjectByType<HitStop>().Stop(0.03f);
=======
        FindAnyObjectByType<HitStop>().Stop(0.05f);
>>>>>>> main
        transform.GetComponentInParent<SnakeManager>().health -= damage;
        if (transform.GetComponentInParent<SnakeManager>().health <= 0){
            Destroy(transform.parent.gameObject);
        }
    }
}
