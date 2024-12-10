using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemyMovement : MonoBehaviour
{
    private Transform player;
    public float chaseSpeed = 2f;
    public float jumpForce = 5f;
    public float aggroAreaSize = 3f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    public bool isChasing = false;
    private bool _isFacingRight = false;
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            // Flip only if the value changes
            if (_isFacingRight != value)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            _isFacingRight = value;
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if grounded
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 2f, groundLayer);

        // Check for jump conditions
        RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(IsFacingRight ? 1 : -1, 0, 0), Vector2.down, 5f, groundLayer);
        RaycastHit2D wallAhead = Physics2D.Raycast(transform.position, IsFacingRight ? Vector2.right : Vector2.left, 1f, groundLayer);

        if (player){
            if (Vector2.Distance(transform.position, player.position) < aggroAreaSize) {
                isChasing = true;
            }
        }

        if (!isChasing){
            if (!gapAhead.collider || wallAhead.collider) {
                IsFacingRight = !IsFacingRight;
            }
        } else {
            if (player){
                IsFacingRight = player.position.x > transform.position.x;
                if (!gapAhead.collider || wallAhead.collider)
                {
                    shouldJump = true;
                }
            }else{
                isChasing=!isChasing;
            }
        }
    }

    private void FixedUpdate()
    {
        // Move horizontally at a constant speed
        float direction = IsFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);

        // Perform jump if conditions are met
        if (isGrounded && shouldJump)
        {
            shouldJump = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // private void OnDrawGizmosSelected(){
    //     Gizmos.DrawWireSphere(transform.position, 1f);
    // }
}
