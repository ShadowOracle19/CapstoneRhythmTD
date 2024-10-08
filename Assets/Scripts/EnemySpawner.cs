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
        foreach(Transform child in _pathParent)
        {
            placedPath.Add(child.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //path = gameObject.GetComponent<AIPath>().path.vectorPath;
        GetComponent<LineRenderer>().positionCount = placedPath.Count;
        GetComponent<LineRenderer>().SetPositions(placedPath.ToArray());

    }

    public void StartSpawningEnemies()
    {
        if(!startOnce)
        {
            //StartCoroutine("SpawnEnemyOnTick");
            startOnce = true;
        }
    }

    // Just an example OnTick here
    public void OnTick()
    {
        if (!startOnce || GameManager.Instance.isGamePaused) return;
        if (currentNumberOfEnemiesSpawned >= numberOfEnemiesToSpawn)
        {
            allEnemiesSpawned = true;
            return;
        }
            

        if (placedPath.Count != 0)
        {
            Debug.Log("EnemySpawnerTick");
            GameObject enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity, enemyParent);
            Conductor.Instance._intervals.Add(enemy.GetComponent<Enemy>().interval);

            //enemy.GetComponent<Enemy>().path = placedPath;
            currentNumberOfEnemiesSpawned += 1;
        }
        
    }

}
