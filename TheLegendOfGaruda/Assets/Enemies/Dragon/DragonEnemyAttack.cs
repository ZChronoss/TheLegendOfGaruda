using UnityEngine;

public class DragonEnemyAttack : MonoBehaviour
{
    private EnemyShooting controller;
    private float timer;
    public float dragonBreathCooldown = 10f;
    public float dragonBreathDuration = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<EnemyShooting>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.shootingDisabled){
            timer += Time.deltaTime;

            if(timer>dragonBreathDuration){
                timer = 0;
                controller.shootingDisabled = !controller.shootingDisabled;
            }
        }else{
            timer += Time.deltaTime;

            if(timer>dragonBreathCooldown){
                timer = 0;
                controller.shootingDisabled = !controller.shootingDisabled;
            }
        }
    }
}
