using UnityEngine;

public class EntitySpawn : MonoBehaviour
{
    public GameObject entity;
    public Transform spawnPos;
    private GameObject spawnedEntity; // Reference to the spawned dragon
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        timer += Time.deltaTime;

        if(timer>2){
            timer = 0;
            if(spawnedEntity == null){
                spawn();
            }
        }
    }

    // Update is called once per frame
    void spawn(){
        spawnedEntity = Instantiate(entity, spawnPos.position, Quaternion.identity);
    }
}
