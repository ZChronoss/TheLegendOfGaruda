using Unity.Mathematics;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 1;
    private GameObject player;
    private Rigidbody2D rb;
    public float speed;
    public float accuracy = 0f;
    private float timer;
    public float bulletExpireTime = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        if(player==null)
        {
            return;
        }

        Vector3 direction = player.transform.position - transform.position;
        float angleOffset = UnityEngine.Random.Range(-accuracy, accuracy);
        float rot = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
        Vector2 inaccurateDirection = new Vector2(
            Mathf.Cos(rot * Mathf.Deg2Rad),
            Mathf.Sin(rot * Mathf.Deg2Rad)
        );
        
        rb.linearVelocity = inaccurateDirection.normalized * speed;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer>bulletExpireTime){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerHealth>().takeDamage(damage);
            Destroy(gameObject);
        }else if (other.gameObject.CompareTag("Ground")){
            Destroy(gameObject);
        }
    }
}
