using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination;

    // Update is called once per frame
    void Update()
    {
        if (patrolDestination == 0){
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[patrolDestination].position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, patrolPoints[patrolDestination].position) < .2f){
                transform.localScale = new Vector3(-5, 5, 5);
                patrolDestination = 1;
            }
        }else if (patrolDestination == 1){
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[patrolDestination].position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, patrolPoints[patrolDestination].position) < .2f){
                transform.localScale = new Vector3(5, 5, 5);
                patrolDestination = 0;
            }
        }
    }
}
