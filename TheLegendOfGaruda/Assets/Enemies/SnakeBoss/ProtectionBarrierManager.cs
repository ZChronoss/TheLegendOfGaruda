using System;
using UnityEngine;

public class ProtectionBarrierManager : MonoBehaviour
{
    private int orbCount = 0; // Tracks the number of collected orbs
    public GameObject protectionBarrierPrefab; // The prefab for the protection barrier
    private GameObject activeBarrier; // Reference to the active barrier
    private GameObject snake;
    private GameObject player;

    void Awake(){
        snake = GameObject.FindGameObjectWithTag("SnakeBoss");
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void HandleOrbCollected()
    {
        orbCount++;

        // Check if the player has collected 3 orbs
        if (orbCount >= 3)
        {
            ActivateProtectionBarrier();
            orbCount = 0; // Reset the orb count
        }
    }

    private void ActivateProtectionBarrier()
    {
        // Instantiate the protection barrier
        activeBarrier = Instantiate(protectionBarrierPrefab, transform.position, Quaternion.identity);
        player.GetComponent<PlayerHealth>().becomeInvulnerable();
        if(snake){
            snake.GetComponent<SnakeManager>().chasingState = true;
        }
    }
}
