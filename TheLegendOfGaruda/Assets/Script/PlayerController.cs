using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour, IDataPersistence
{
    [Header("Walk")]
    public float walkSpeed = 10f;

    [Header("Jump")]
    public float jumpImpulse = 16f;
    public float airSpeed = 10f;
    // MARK: Coyote Jump
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    // MARK: Jump Buffer
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    bool hasPressedJumpButton = false;

    [Header("Dash")]
    public float dashSpeed = 50f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    bool isDashing = false;
    bool canDash = true;
    TrailRenderer trailRenderer;
    [SerializeField] AudioClip dashFX;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 50f;
    public float fallSpeedMultiplier = 2f;

    [Header("Fly")]
    public float flySpeed = 15f;
    public float flySteer = 30f;
    public float flyDuration = 1f;
    private bool _isFlying = false;
    public bool canFly = true;
    [SerializeField] AudioClip wingFlapFX;
    [SerializeField] AudioClip flyFX;

    Vector2 moveInput;

    TouchingDirections touchingDirections;

    Rigidbody2D rb;

    Animator animator;
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !DialogueManager.isActive)
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
            else
            {
                // lock movement
                return 0;
            }
            
            
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationString.canMove);
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
            animator.SetBool(AnimationString.isMoving, value);
        }
    }

    public bool IsFlying
    {
        get
        {
            return _isFlying;
        }

        private set
        {
            _isFlying = value;
            animator.SetBool(AnimationString.isFlying, value);
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

    PlayerHealth playerHealth;

    public void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        trailRenderer = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // ini kalo loncat tp ga nyentuh tanah < 0.2 second bisa loncat
        if (touchingDirections.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        if (!hasPressedJumpButton)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
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
        if (_isFlying) {
            return;
        }
        Gravity();
        // Biar ga bisa jalan pas lg dashing
        if (isDashing)
        {
            return;
        }
        rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);

        animator.SetFloat(AnimationString.yVelocity, rb.linearVelocity.y);
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
        if(CanMove)
        {
            if (context.started)
            {
                hasPressedJumpButton = true;
                jumpBufferCounter = jumpBufferTime;
            }
            // kalo pencet ditahan bakal max jump height
            if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
            {
                animator.SetTrigger(AnimationString.jump);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
                jumpBufferCounter = 0f;
                hasPressedJumpButton = false;
            }
            else if (context.canceled && Vector2.Dot(rb.linearVelocity, Vector2.up) > 0)
            {
                // kalo light tap bakal setengahnya
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);

                coyoteTimeCounter = 0f;
                hasPressedJumpButton = false;
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash && CanMove)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    // co routine itu kek code yang jalan di multiple frame, jadi kek bisa nunggu di satu titik baru lanjut
    private IEnumerator DashCoroutine()
    {
        playerHealth.becomeInvulnerable();
        canDash = false;
        isDashing = true;
        trailRenderer.emitting = true;
        SFXManager.instance.PlaySFXClip(dashFX, transform, 0.5f);

        animator.SetTrigger(AnimationString.dash);

        float dashDirection = IsFacingRight ? 1 : -1;

        if (_isFlying)
        {
            _isFlying = false;
        }

        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

        isDashing = false;
        playerHealth.removeInvulnerable();
        trailRenderer.emitting = false;

        yield return new WaitForSeconds(dashCooldown);
        yield return new WaitUntil(isGrounded);
        canDash = true;
    }

    bool isGrounded(){
        return touchingDirections.isGrounded;
    }

    public void OnFly(InputAction.CallbackContext context) {
        if (!touchingDirections.isGrounded && context.performed && canFly && CanMove) {
            StartCoroutine(FlyCoroutine());
        }
    }

    private IEnumerator FlyCoroutine()
    {
        canFly = false;
        IsFlying = true;
        rb.gravityScale = 0;

        float elapsedTime = 0;
        Vector2 velocity = new Vector2((IsFacingRight ? 1 : -1) * flySpeed, 0f); // Initialize with current velocity

        SFXManager.instance.PlaySFXClip(wingFlapFX, transform, 0.5f);
        //SFXManager.instance.PlaySFXClip(flyFX, transform, 0.5f, flyDuration);

        while (elapsedTime < flyDuration)
        {
            if (!_isFlying)
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

            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, rb.linearVelocity.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720f * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // End of flight
        rb.gravityScale = baseGravity;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y); // Retain vertical velocity for falling
        IsFlying = false;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector2.up);

        yield return new WaitUntil(isGrounded);
        canFly = true;
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
