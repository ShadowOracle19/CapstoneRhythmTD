using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Wave> currentWaves = new List<Wave>();
    public Wave currentWave;

    public bool startOnce = false;

    public int numberOfEnemiesToSpawn = 0;
    public int currentNumberOfEnemiesSpawned = 0;

    public bool allEnemiesSpawned = false;

    public Transform enemyParent;

    public int lastRandomSpawn;

    [Header("Wave info")]
    public float timeRemainingToWaveStart = 0;
    public int waveIndex = 0;
    public bool allEnemiesSpawnedFromWave = false;
    public int numEnemiesInWave = 0;
    public int delay;

    private void Update()
    {
        if(startOnce && allEnemiesSpawnedFromWave)
        {
            if(timeRemainingToWaveStart >= delay)
            {
                Debug.Log("Are we here");
                //allow enemies to spawn
                if(waveIndex >= currentWaves.Count - 1) //if at the last wave stop running this
                {
                    return;
                }
                currentWave = currentWaves[waveIndex];
                timeRemainingToWaveStart = 0;//set delay for next wave
                allEnemiesSpawnedFromWave = false;
            }
            else
            {
                Debug.Log("Are we here?");
                timeRemainingToWaveStart += Time.deltaTime;
            }
        }
    }

    public void StartSpawningEnemies()
    {
        if(!startOnce)
        {
            startOnce = true;
            timeRemainingToWaveStart = 0;
            waveIndex = 0;
            numEnemiesInWave = 0;
            delay = 0;
            allEnemiesSpawnedFromWave = true;
        }
    }

    public void SpawnUnit()
    {
        if (!startOnce || GameManager.Instance.isGamePaused || allEnemiesSpawnedFromWave)
            return;

        //once all enemies are spawned stop spawning them
        if (currentNumberOfEnemiesSpawned >= numberOfEnemiesToSpawn) 
        {
            Debug.Log("spawned enemies");
            allEnemiesSpawned = true;
            return;
        }


        int randSpawn = Random.Range(-2, 2);
        if (randSpawn == lastRandomSpawn)
        {
            randSpawn = Random.Range(-2, 2);
        }
        GameObject enemy = Instantiate(currentWave.enemy, new Vector3(transform.position.x, transform.position.y + randSpawn), Quaternion.identity, enemyParent);
        lastRandomSpawn = randSpawn;

        ConductorV2.instance.triggerEvent.Add(enemy.GetComponent<Enemy>().trigger);

        currentNumberOfEnemiesSpawned += 1;
        numEnemiesInWave += 1;
        if(numEnemiesInWave == currentWaves[waveIndex].numberOfEnemies)
        {
            waveIndex += 1;
            delay = currentWaves[waveIndex].delay;
            numEnemiesInWave = 0;
            allEnemiesSpawnedFromWave = true;
        }
    }
}
