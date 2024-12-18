using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    [Header("Walk")]
    public float walkSpeed = 10f;

    [Header("Jump")]
    public float jumpImpulse = 15f;
    public float airSpeed;

    [Header("Dash")]
    public float dashSpeed = 50f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    bool isDashing = false;
    bool canDash = true;
    TrailRenderer trailRenderer;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 30f;
    public float fallSpeedMultiplier = 2f;

    [Header("Fly")]
    public float flySpeed = 15f;
    public float flySteer = 30f;
    public float flyDuration = 1f;
    bool isFlying = false;

    Vector2 moveInput;

    TouchingDirections touchingDirections;

    Rigidbody2D rb;


    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && !touchingDirections.isOnWall && !DialogueManager.isActive) 
            {
                if (touchingDirections.isGrounded)
                {
                    return walkSpeed;
                }
                else
                {
                    return airSpeed;
                }
            }
            else
            {
                // idle
                return 0;
            }
            
        }
    }

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            //animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            // Flip only if the value is new
            if (_isFacingRight != value)
            {
                // Flip the local scale to make the player faceing the opposite direction
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        trailRenderer = GetComponent<TrailRenderer>();

        airSpeed = walkSpeed;
    }


    /// <summary>
    /// UPDATE VS FIXED UPDATE
    /// 
    /// FixedUpdate is used for being in-step with the physics engine, so anything that needs to be applied to a rigidbody should happen in FixedUpdate.
    /// 
    /// 
    /// Update, on the other hand, works independantly of the physics engine. This can be benificial if a user's framerate were to drop but you need a certain calculation to keep executing, like if you were updating a chat or voip client, you would want regular old update.
    /// https://learn.unity.com/tutorial/update-and-fixedupdate
    /// </summary>
    public void FixedUpdate()
    {
        if (isFlying) {
            return;
        }
        Gravity();
        // Biar ga bisa jalan pas lg dashing
        if (isDashing)
        {
            return;
        }
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            // Facing right 
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // Facing left
            IsFacingRight = false;
        }
    }

    private void Gravity()
    {
        if(rb.linearVelocity.y < 0)
        {
            // make fall increasingly faster
            rb.gravityScale = baseGravity * fallSpeedMultiplier;

            // cap max fall speed
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }else
        {
            rb.gravityScale = baseGravity;
        }
    }

    // MARK: - Player's control

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context) 
    {
        // kalo pencet ditahan bakal max jump height
        if (context.performed && touchingDirections.isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
        }
        else if (context.canceled)
        {
            // kalo light tap bakal setengahnya
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    // co routine itu kek code yang jalan di multiple frame, jadi kek bisa nunggu di satu titik baru lanjut
    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;
        trailRenderer.emitting = true;

        float dashDirection = IsFacingRight ? 1 : -1;

        if (isFlying)
        {
            isFlying = false;
        }

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        isDashing = false;
        trailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public void OnFly(InputAction.CallbackContext context) {
        if (!touchingDirections.isGrounded && context.performed && !isFlying) {
            StartCoroutine(FlyCoroutine());
        }
    }

    private IEnumerator FlyCoroutine()
    {
        isFlying = true;
        rb.gravityScale = 0;

        float elapsedTime = 0;
        Vector2 velocity = new Vector2((IsFacingRight ? 1 : -1) * flySpeed, 0f); // Initialize with current velocity

        while (elapsedTime < flyDuration)
        {
            if (!isFlying)
            {
                break;
            }

            if (moveInput != Vector2.zero)
            {
                // Normalize the input to get a direction
                Vector2 desiredDirection = moveInput.normalized;

                // Smoothly interpolate velocity towards the desired direction
                velocity = Vector2.Lerp(velocity, desiredDirection * flySpeed, Time.deltaTime * flySteer);
                // Update the rigidbody velocity
            }
            rb.linearVelocity = velocity;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // End of flight
        rb.gravityScale = baseGravity;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y); // Retain vertical velocity for falling
        isFlying = false;
    }
}
