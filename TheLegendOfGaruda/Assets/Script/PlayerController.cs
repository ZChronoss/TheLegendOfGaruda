using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float jumpImpulse = 10f;
    public float airSpeed = 5f;
    // make dodgeSpeed, flySpeed (butuh apa lagi)

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

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context) 
    {
        if (context.started && touchingDirections.isGrounded) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
        }
    }

}
