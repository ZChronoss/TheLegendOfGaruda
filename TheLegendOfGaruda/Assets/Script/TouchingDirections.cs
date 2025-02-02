using System.Linq;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;

    CapsuleCollider2D touchingCol;
    Animator animator;

    public RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    
    [SerializeField]
    private bool _isGrounded;
    public bool isGrounded 
    { 
        get 
        {
            return _isGrounded;
        } 
        private set 
        {
            _isGrounded = value;
            animator.SetBool(AnimationString.isGrounded, value);
        } 
    }

    [SerializeField]
    private bool _isOnWall;
    public bool isOnWall
    { 
        get 
        {
        return _isOnWall;
        } 
        private set 
        {
            _isOnWall = value;
        } 
    }

    private void Awake() 
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        castFilter = new ContactFilter2D();
        castFilter.SetLayerMask(LayerMask.GetMask("Ground"));   
    }

    void Update()
    {
        isGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        isOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
    }
}
