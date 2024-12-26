using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform attackTransform;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float damageAmount = 1f;
    private RaycastHit2D[] hits;

    // Update is called once per frame
    // void Update()
    // {
    //     if (UserInput.instance.controls.Attack.Attack.WasPressedThisFrame()){
    //         Attack();
    //     }
    // }

    public void OnAttack(InputAction.CallbackContext context){
        if(context.started){    
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

    private void OnDrawGizmosSelected(){
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }
}
