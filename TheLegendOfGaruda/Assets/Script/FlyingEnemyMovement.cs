using UnityEngine;

public class FlyingEnemyMovement : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 1f; // Amplitude of the sine wave
    public float frequency = 1f; // Frequency of the sine wave
    public float aggroAreaSize = 5f;
    private Transform player;
    public Transform[] patrolPoints; // Array of patrol points
    private int currentPatrolIndex = 0; // Current patrol point index
    private int patrolDirection = 1; // 1 for forward, -1 for backward
    private Vector2 patrolTarget; // Current patrol target
    private bool _isFacingRight = false;
    public bool isChasing = false;
    private float startY; // The initial Y position for sine wave movement

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
        if (patrolPoints.Length == 0)
        {
            Debug.LogError("No patrol points assigned!");
            enabled = false;
            return;
        }

        patrolTarget = patrolPoints[currentPatrolIndex].position; // Start patrolling to the first point
    }

    private void Update()
    {
        if (player == null) return;

        if (Vector2.Distance(transform.position, player.position) < aggroAreaSize) {
            isChasing = true;
        }

        if (isChasing)
        {
            Chase();
        }
        else
        {
            Patrol();
        }

        Flip();
    }

    private void Chase()
    {
        // Move directly towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    private void Patrol()
    {
        // Horizontal movement towards the current patrol target
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector2(patrolTarget.x, patrolTarget.y + Mathf.Sin(Time.time * frequency) * amplitude),
            speed * Time.deltaTime
        );

        // Check if the enemy reaches the current patrol point
        if (Vector2.Distance(transform.position, patrolTarget) < 1f)
        {
            // Update patrol index and direction
            if (patrolDirection == 1 && currentPatrolIndex == patrolPoints.Length - 1)
            {
                patrolDirection = -1; // Reverse direction
            }
            else if (patrolDirection == -1 && currentPatrolIndex == 0)
            {
                patrolDirection = 1; // Forward direction
            }

            currentPatrolIndex += patrolDirection; // Move to the next patrol point
            patrolTarget = patrolPoints[currentPatrolIndex].position; // Update target
            startY = transform.position.y; // Reset sine wave baseline
        }
    }

    private void Flip()
    {
        if (isChasing){
            if(player){
                IsFacingRight = transform.position.x < player.position.x;
            }else{
                isChasing = !isChasing;
            }
        }else{
            IsFacingRight = transform.position.x < patrolTarget.x;
        }
    }
}