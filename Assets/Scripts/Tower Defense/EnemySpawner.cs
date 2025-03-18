using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region dont touch this
    private static EnemySpawner _instance;
    public static EnemySpawner Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("EnemySpawner Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public List<Wave> currentWaves = new List<Wave>();

    public bool startOnce = false;

    public int numberOfEnemiesToSpawn = 0;
    public int currentNumberOfEnemiesSpawned = 0;

    public bool allEnemiesSpawned = false;

    public Transform enemyParent;

    public int lastRandomSpawn;

    public List<GameObject> spawnTiles = new List<GameObject>();

    [Header("Wave info")]
    public float timeRemainingToWaveStart = 0;
    public int waveIndex = 0;
    public bool allEnemiesSpawnedFromWave = false;
    public int numEnemiesInWave = 0;
    public int delay;

    [Header("EnemyType")]
    public GameObject walkerEnemy;
    public GameObject wispEnemy;

    private void Update()
    {
        GameManager.Instance.waveCounter.text = "Wave " + waveIndex + "/" + currentWaves.Count;
        if(startOnce && allEnemiesSpawnedFromWave)
        {
            if(timeRemainingToWaveStart >= delay)
            {
                //allow enemies to spawn
                if(waveIndex >= currentWaves.Count) //if at the last wave stop running this
                {
                    return;
                }
                timeRemainingToWaveStart = 0;//set delay for next wave
                allEnemiesSpawnedFromWave = false;
            }
            else
            {
                timeRemainingToWaveStart += Time.deltaTime;
            }
        }
    }

    public void ForceEnemySpawn(float ypos, EnemyType enemyType)
    {
        currentNumberOfEnemiesSpawned += 1;
        GameObject enemy = null;
        switch (enemyType)
        {
            case EnemyType.Wisp:
                enemy = Instantiate(wispEnemy, new Vector3(transform.position.x, ypos), Quaternion.identity, enemyParent);
                break;
            case EnemyType.Walker:
                enemy = Instantiate(walkerEnemy, new Vector3(transform.position.x, ypos), Quaternion.identity, enemyParent);
                break;
            default:
                break;
        }
        
        
        ConductorV2.instance.triggerEvent.Add(enemy.GetComponent<Enemy>().trigger);

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
            allEnemiesSpawned = true;
            return;
        }
        
        for (int i = 0; i < currentWaves[waveIndex].numberOfEnemies; i++)
        {
            int randSpawn = Random.Range(0, spawnTiles.Count);
            if (randSpawn == lastRandomSpawn)
            {
                randSpawn = Random.Range(0, spawnTiles.Count);
            }
            GameObject enemy = Instantiate(currentWaves[waveIndex].enemy, new Vector3(transform.position.x, spawnTiles[randSpawn].transform.position.y), Quaternion.identity, enemyParent);
            lastRandomSpawn = randSpawn;

            ConductorV2.instance.triggerEvent.Add(enemy.GetComponent<Enemy>().trigger);

            currentNumberOfEnemiesSpawned += 1;

            numEnemiesInWave += 1;
        }
        


        if (GameManager.Instance.tutorialRunning)
            return;

        
        if(numEnemiesInWave == currentWaves[waveIndex].numberOfEnemies)
        {
            waveIndex += 1;
            delay = currentWaves[waveIndex].delay;
            numEnemiesInWave = 0;
            allEnemiesSpawnedFromWave = true;
        }
    }
}

public enum EnemyType
{
    Wisp, Walker
}
