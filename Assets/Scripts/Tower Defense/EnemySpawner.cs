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



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        {
            Debug.Log("Stop Spawning");
            return;
        }
        if (currentNumberOfEnemiesSpawned >= numberOfEnemiesToSpawn)
        {
            Debug.Log("spawned enemies");
            allEnemiesSpawned = true;
            return;
        }
        Debug.Log("spawn enemies");
        //randomly spawn an enemy on the y value from -1,0,1
        int randSpawn = Random.Range(-1, 2);
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(transform.position.x, transform.position.y + randSpawn), Quaternion.identity, enemyParent);
        ConductorV2.instance.triggerEvent.Add(enemy.GetComponent<Enemy>().trigger);

        currentNumberOfEnemiesSpawned += 1;
        
    }

}
