using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    CapsuleCollider2D touchingCol;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    
    [SerializeField]
    private bool _isGrounded;
    public bool isGrounded { get {
        return _isGrounded;
    } private set {
        _isGrounded = value;
    } }

    private void Awake() 
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        isGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
    }
}
