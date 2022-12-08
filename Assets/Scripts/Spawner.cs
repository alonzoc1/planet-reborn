using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //The prefab for the enemy object
    public GameObject enemyPrefab;
    
    //The Spawn Zone
    public GameObject spawnZone;

    //The number of enemies to spawn
    public int numEnemies;

    //The interval between each spawn
    public float spawnInterval;

    //The current time until the next spawn
    private float timeUntilNextSpawn;

    //The list of spawned enemy objects
    private List<GameObject> spawnedEnemies;

    void Start()
    {
        //Initialize the list of spawned enemies
        spawnedEnemies = new List<GameObject>();
        //Initialize the time until the next spawn
        timeUntilNextSpawn = 0;
    }

    void Update()
    {
        //Decrement the time until the next spawn
        timeUntilNextSpawn -= Time.deltaTime;

        //Check if it's time to spawn a new enemy
        if (timeUntilNextSpawn <= 0 && spawnedEnemies.Count < numEnemies)
        {
            //Spawn a new enemy
            SpawnEnemy();

            //Reset the time until the next spawn
            timeUntilNextSpawn = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // Get a spawn point
        Vector3 spawnPoint = spawnZone.GetComponent<SpawnZone>().SpawnPoint;
        //Make sure the enemy doesn't spawn inside something
        while (Physics.CheckSphere(spawnPoint, 0.5f))
        {
            spawnPoint = spawnZone.GetComponent<SpawnZone>().SpawnPoint;
        }

        //Create a new enemy object at the random position
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        //Add the enemy to the list of spawned enemies
        spawnedEnemies.Add(enemy);
    }
}
