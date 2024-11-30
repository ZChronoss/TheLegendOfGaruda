using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyHomingBullet : MonoBehaviour
{
    public int damage = 1;
    public float speed = 5f;
    public float rotateSpeed = 200f; 
    private GameObject player;
    private Rigidbody2D rb;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        if (player != null)
        {
            Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle); // Rotate the bullet to face the player
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer>5){
            Destroy(gameObject);
        }
    }

    private void FixedUpdate(){
        Vector2 direction = (transform.position - player.transform.position).normalized;
        float value = Vector3.Cross(direction, transform.right).z;

        rb.angularVelocity = rotateSpeed * value;
        
        rb.linearVelocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerHealth>().takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
