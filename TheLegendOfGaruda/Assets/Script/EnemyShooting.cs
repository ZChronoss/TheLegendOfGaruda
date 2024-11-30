using Unity.Mathematics;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    public GroundedEnemyMovement controller;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isChasing){
            timer += Time.deltaTime;

            if(timer>2){
                timer = 0;
                shoot();
            }
        }
    }

    void shoot(){
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
