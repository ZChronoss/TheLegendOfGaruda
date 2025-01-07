using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;
    private GroundedEnemyMovement controller;
    private FlyingEnemyMovement flyingController;
    private bool flyingEnemy = false;
    private float timer;
    public bool shootingDisabled = false;
    public float shootingCooldown = 2f;
    
    void Start()
    {
        controller = GetComponent<GroundedEnemyMovement>();
        if (!controller) 
        {
            flyingController = GetComponent<FlyingEnemyMovement>();
            flyingEnemy = true;
        }
    }

    void Update()
    {
        if (CanShoot()) 
        {
            timer += Time.deltaTime;

            if (timer > shootingCooldown) 
            {
                timer = 0;
                shoot();
            }
        }
    }

    private bool CanShoot()
    {
        if (flyingEnemy && flyingController != null)
            return flyingController.isChasing && !shootingDisabled;

        if (!flyingEnemy && controller != null)
            return controller.isChasing && !shootingDisabled;

        return false;
    }

    void shoot()
    {
        if (bullet && bulletPos)
        {
            Instantiate(bullet, bulletPos.position, Quaternion.identity);
        }
    }
}
