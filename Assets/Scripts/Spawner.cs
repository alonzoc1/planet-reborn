using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //The prefab for the enemy object
    public GameObject enemyPrefab;
    
    //The Spawn Zone
    public SpawnZone spawnZone;

    //The number of enemies to spawn
    public int numEnemies;

    //The interval between each spawn
    public float spawnInterval;

    //The current time until the next spawn
    private float timeUntilNextSpawn;

    //The list of spawned enemy objects
    private List<GameObject> spawnedEnemies;
    
    //If there is an active coroutine trying to spawn an enemy
    private bool spawnWaiting;
    
    void Start()
    {
        //Initialize the list of spawned enemies
        spawnedEnemies = new List<GameObject>();
        //Initialize the time until the next spawn
        timeUntilNextSpawn = 0;
        spawnWaiting = false;
    }

    void Update()
    {
        //Decrement the time until the next spawn
        timeUntilNextSpawn -= Time.deltaTime;

        //Check if it's time to spawn a new enemy
        if (timeUntilNextSpawn <= 0) {
            CleanEnemyList();
        }
        if (timeUntilNextSpawn <= 0 && spawnedEnemies.Count < numEnemies && !spawnWaiting)
        {
            //Spawn a new enemy
            StartCoroutine(SpawnEnemy());
        }
    }

    private void CleanEnemyList() {
        // Go through our created enemies and remove dead ones
        spawnedEnemies.RemoveAll(o => o == null);
    }

    IEnumerator SpawnEnemy() {
        spawnWaiting = true;
        // Get a spawn point
        Vector3 spawnPoint = spawnZone.SpawnPoint;
        //Make sure the enemy doesn't spawn inside something
        while (Physics.CheckSphere(spawnPoint, 0.5f))
        {
            spawnPoint = spawnZone.SpawnPoint;
            yield return new WaitForSeconds(1f); // wait a bit to avoid being locked by this while
        }

        //Create a new enemy object at the random position
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

        //Add the enemy to the list of spawned enemies
        spawnedEnemies.Add(enemy);
        timeUntilNextSpawn = spawnInterval;
        spawnWaiting = false;
    }
}
