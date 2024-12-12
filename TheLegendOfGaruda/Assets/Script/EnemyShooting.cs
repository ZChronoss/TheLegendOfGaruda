using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    private GroundedEnemyMovement controller;
    private float timer;
    public bool shootingDisabled = false;
    public float shootingCooldown = 2f;
    
    void Start(){
        controller = GetComponent<GroundedEnemyMovement>();
    }
    // Update is called once per frame
    void Update()
    {
        if (controller.isChasing && !shootingDisabled){
            timer += Time.deltaTime;

            if(timer>shootingCooldown){
                timer = 0;
                shoot();
            }
        }
    }

    void shoot(){
        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }
}
