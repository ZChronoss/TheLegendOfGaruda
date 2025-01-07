using UnityEngine;

public class DragonSpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // The enemy prefab to spawn
    [SerializeField] private mainBossDragon bossDragon; // Reference to the mainBossDragon script
    [SerializeField] private Transform[] spawnPoints; // Array of spawn points
    public float timeInterval = 5f; // Time interval between spawns
    private float timer; // Timer to track spawn intervals
    private bool[] isOccupied; // Array to track if a spawn point is occupied

    private void Start()
    {
        // Initialize the occupancy array based on the number of spawn points
        isOccupied = new bool[spawnPoints.Length];
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Spawn an enemy every 2 seconds
        if (timer > timeInterval)
        {
            timer = 0f; // Reset the timer
            Spawn(); // Call the Spawn method
        }
    }

    private void Spawn()
    {
        // Find available spawn points
        int availableCount = 0;
        foreach (bool occupied in isOccupied)
        {
            if (!occupied) availableCount++;
        }

        // If no spawn points are available, don't spawn
        if (availableCount == 0) return;

        // Select a random spawn point that is not occupied
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, spawnPoints.Length);
        } while (isOccupied[randomIndex]);

        // Mark the spawn point as occupied
        isOccupied[randomIndex] = true;

        // Instantiate the enemy at the chosen spawn point
        GameObject enemy = Instantiate(enemyPrefab, spawnPoints[randomIndex].position, Quaternion.identity);

        // Get the EnemyHealth component and register it with the boss
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            bossDragon.RegisterEnemy(enemyHealth);

            // Unmark the spawn point as occupied when the enemy dies
            enemyHealth.OnEntityDeath += (EnemyHealth e) => { isOccupied[randomIndex] = false; };
        }
    }
}
