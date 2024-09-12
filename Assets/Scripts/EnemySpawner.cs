using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Vector3> path;

    public bool startOnce = false;
    public GameObject enemyPrefab;

    public int numberOfEnemiesToSpawn = 0;
    int currentNumberOfEnemiesSpawned = 0;

    

    public Transform enemyParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        path = gameObject.GetComponent<AIPath>().path.vectorPath;
        GetComponent<LineRenderer>().positionCount = path.Count;
        GetComponent<LineRenderer>().SetPositions(path.ToArray());

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
    void OnTick()
    {
        if (currentNumberOfEnemiesSpawned == numberOfEnemiesToSpawn) return;

        if (path.Count != 0)
        {
            Debug.Log("EnemySpawnerTick");
            // GetComponent<AudioSource>().Play();
            GameObject enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity, enemyParent);

            enemy.GetComponent<Enemy>().path = path;
            currentNumberOfEnemiesSpawned += 1;
        }
        
    }

    //IEnumerator SpawnEnemyOnTick()
    //{
    //    for (int i = 0; i < numberOfEnemiesToSpawn; i++)
    //    {
    //        GameObject enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity, enemyParent);

    //        enemy.GetComponent<Enemy>().path = path;
    //        yield return new WaitForSeconds(1f);
    //    }

    //}
}
