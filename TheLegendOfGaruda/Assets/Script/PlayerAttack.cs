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
    private Animator animator;
    private TouchingDirections touchingDirections;

    [Header("Garuda Descend")]
    PlayerController playerController;
    PlayerHealth playerHealth;
    public float dashRange = 10f;
    public float dashSpeed = 50f;
    private Transform targetEnemy;
    public int dashDamage = 1;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private bool canDash = true;

    [Header("Enemy Attacked SFX")]
    public AudioClip enemyAttackedSFX;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    public void OnAttack(InputAction.CallbackContext context){
        if(context.started && playerController.CanMove){
            if (playerController.IsFlying)
            {
                targetEnemy = FindNearestEnemy();
                print(targetEnemy);

                if (targetEnemy != null)
                {
                    RaycastHit2D hit = Physics2D.Linecast(transform.position, targetEnemy.position, LayerMask.GetMask("Ground"));
                    if (hit.collider == null)
                    {
                        StartCoroutine(DashCoroutine(targetEnemy));
                    }
                }
            }
            else if (touchingDirections.isGrounded)
            {
                // MARK: Animation
                animator.SetTrigger(AnimationString.attackTrigger);

                hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);
                for (int i = 0; i < hits.Length; i++){
                    RaycastHit2D hit = Physics2D.Linecast(transform.position, hits[i].transform.position, LayerMask.GetMask("Ground"));
                    if (hit.collider == null)
                    {
                        IDamageable enemyHeatlh = hits[i].collider.gameObject.GetComponent<IDamageable>();
                        PurchasableItem purchasableItem = hits[i].collider.gameObject.GetComponent<PurchasableItem>();

                        if (enemyHeatlh != null){
                            IDamageable iDamageable = hits[i].collider.gameObject.GetComponent<IDamageable>();

                            if (iDamageable != null){
                                SFXManager.instance.PlaySFXClip(enemyAttackedSFX, transform, 0.5f);
                                iDamageable.Damage(damageAmount);
                            }
                        }

                        if (purchasableItem != null){
                            purchasableItem.PurchaseItem();
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
        playerHealth.becomeInvulnerable();
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
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, targetPosition);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720f * Time.deltaTime);
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
        yield return new WaitForSeconds(1f);
        playerHealth.removeInvulnerable();

        // Start cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
