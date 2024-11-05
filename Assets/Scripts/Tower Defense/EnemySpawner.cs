using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Vector3> path;

    public List<Vector3> placedPath;

    public bool startOnce = false;
    public GameObject enemyPrefab;

    public int numberOfEnemiesToSpawn = 0;
    public int currentNumberOfEnemiesSpawned = 0;

    public bool allEnemiesSpawned = false;

    public Transform enemyParent;
    [SerializeField] private Transform _pathParent;

    public bool spawnBeat;

    public float restForBeats = 10;
    public float currentBeat = 0;
    public float enemyTracker = 0;

    public int lastRandomSpawn;

    private void Update()
    {
        
    }

    public void StartSpawningEnemies()
    {
        if(!startOnce)
        {
            startOnce = true;
        }
    }

    public void SpawnUnit()
    {
        if (!startOnce || GameManager.Instance.isGamePaused)
            return;

        //once all enemies are spawned stop spawning them
        if (currentNumberOfEnemiesSpawned >= numberOfEnemiesToSpawn) 
        {
            Debug.Log("spawned enemies");
            allEnemiesSpawned = true;
            return;
        }

        if(enemyTracker == 5)
        {
            currentBeat += 1;
            if(currentBeat == restForBeats)
            {
                currentBeat = 0;
                enemyTracker = 0;
            }
            return;
        }

        //randomly spawn an enemy on the y value from -1,0,1
        Debug.Log(currentNumberOfEnemiesSpawned + " Enemy Spawned");

        int randSpawn = Random.Range(-2, 2);
        if (randSpawn == lastRandomSpawn)
        {
            randSpawn = Random.Range(-2, 2);
        }
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(transform.position.x, transform.position.y + randSpawn), Quaternion.identity, enemyParent);
        lastRandomSpawn = randSpawn;

        ConductorV2.instance.triggerEvent.Add(enemy.GetComponent<Enemy>().trigger);

        currentNumberOfEnemiesSpawned += 1;
        enemyTracker += 1;
        
    }
}
