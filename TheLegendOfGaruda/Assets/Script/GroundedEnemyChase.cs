using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    private bool isChasing = false;
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
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if grounded
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        // Check for jump conditions
        RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(IsFacingRight ? 1 : -1, 0, 0), Vector2.down, 5f, groundLayer);
        RaycastHit2D wallAhead = Physics2D.Raycast(transform.position, IsFacingRight ? Vector2.right : Vector2.left, 1f, groundLayer);

        if (Vector2.Distance(transform.position, player.position) < 3f) {
            isChasing = true;
        }

        if (!isChasing){
            if (!gapAhead.collider) {
                IsFacingRight = !IsFacingRight;
            }
        } else {
            IsFacingRight = player.position.x > transform.position.x;
            if (!gapAhead.collider || wallAhead.collider)
            {
                shouldJump = true;
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
}
