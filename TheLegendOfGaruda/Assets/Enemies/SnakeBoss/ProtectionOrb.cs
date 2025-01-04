using System;
using UnityEngine;

public class ProtectionOrb : MonoBehaviour, IItem
{
    [SerializeField] GameObject protectionBarrierPrefab;
    public int amount = 5;
    private GameObject snake;
    private GameObject player;
    public void Awake(){
        snake = GameObject.FindGameObjectWithTag("SnakeBoss");
        player = GameObject.FindGameObjectWithTag("Player");
        if(snake){
            snake.GetComponent<SnakeManager>().target.Insert(0, gameObject);
        }
    }
    public void Collect()
    {
        if(snake){
            snake.GetComponent<SnakeManager>().target.Remove(gameObject);
        }
        if(player.GetComponent<ProtectionBarrierManager>() != null){
            player.GetComponent<ProtectionBarrierManager>().HandleOrbCollected();
        }else{
            player.AddComponent<ProtectionBarrierManager>();
            player.GetComponent<ProtectionBarrierManager>().protectionBarrierPrefab = protectionBarrierPrefab;
            player.GetComponent<ProtectionBarrierManager>().HandleOrbCollected();
        }
        Destroy(gameObject);
    }

    public void Absorb(){
        snake.GetComponent<SnakeManager>().target.Remove(gameObject);
        Destroy(gameObject);
    }
}