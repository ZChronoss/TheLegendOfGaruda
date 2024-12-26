using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damageAmount = 1f;
    private RaycastHit2D[] hits;

    [Header("Garuda Descend")]
    PlayerController playerController;
    PlayerHealth playerHealth;
    public float dashRange = 10f;
    public float dashSpeed = 50f;
    private Transform targetEnemy;
    public int dashDamage = 1;
    public float dashCooldown = 2f;

    private bool isDashing = false;
    private bool canDash = true;

    // Update is called once per frame
    // void Update()
    // {
    //     if (UserInput.instance.controls.Attack.Attack.WasPressedThisFrame()){
    //         Attack();
    //     }
    // }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void OnAttack(InputAction.CallbackContext context){
        if(context.started){
            if (playerController.isFlying)
            {
                targetEnemy = FindNearestEnemy();
                print(targetEnemy);

                if (targetEnemy != null)
                {
                    StartCoroutine(DashCoroutine(targetEnemy));
                    StartCoroutine(playerHealth.BecomeTemporarilyInvincible(2f));
                }
            }
            else
            {
                hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);
                print(hits.Length);
                for (int i = 0; i < hits.Length; i++){
                    IDamageable enemyHeatlh = hits[i].collider.gameObject.GetComponent<IDamageable>();

                    if (enemyHeatlh != null){
                        IDamageable iDamageable = hits[i].collider.gameObject.GetComponent<IDamageable>();

                        if (iDamageable != null){
                            iDamageable.Damage(damageAmount);
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }

    Transform FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, dashRange, attackableLayer);
        Transform nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            Vector2 directionToEnemy = hit.transform.position - transform.position;
            float distanceToEnemy = directionToEnemy.magnitude;

            // Perform a raycast to check for obstacles
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, directionToEnemy.normalized, distanceToEnemy, attackableLayer);

            // Check if the raycast hits anything other than the enemy
            if (raycastHit.collider == null || raycastHit.collider.transform == hit.transform)
            {
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    nearestEnemy = hit.transform;
                }
            }
        }

        return nearestEnemy;
    }


    private IEnumerator DashCoroutine(Transform enemy)
    {
        canDash = false;
        isDashing = true;

        Vector2 startPosition = transform.position;
        Vector2 targetPosition = enemy.position;

        float elapsedTime = 0f;
        float dashDuration = Vector2.Distance(startPosition, targetPosition) / dashSpeed;

        // Disable gravity while dashing (if applicable)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
        }

        while (elapsedTime < dashDuration)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / dashDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Snap to target position
        transform.position = targetPosition;

        // Apply damage to the enemy
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(damageAmount);
        }

        // Re-enable gravity
        if (rb != null)
        {
            rb.gravityScale = 1; // Set this to your normal gravity scale
        }

        isDashing = false;


        // Start cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
