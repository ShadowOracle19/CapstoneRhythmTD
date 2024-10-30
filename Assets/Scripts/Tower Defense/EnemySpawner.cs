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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ConductorV2.instance.loopPositionInBeats == 4)
        {
            SpawnUnit();
        }

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
        if (!startOnce || GameManager.Instance.isGamePaused) return;
        if (currentNumberOfEnemiesSpawned >= numberOfEnemiesToSpawn)
        {
            allEnemiesSpawned = true;
            return;
        }
        //randomly spawn an enemy on the y value from -1,0,1
        int randSpawn = Random.Range(-1, 2);
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(transform.position.x, transform.position.y + randSpawn), Quaternion.identity, enemyParent);

        //enemy.GetComponent<Enemy>().path = placedPath;
        currentNumberOfEnemiesSpawned += 1;
        
    }

}
