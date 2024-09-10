using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Vector3> path;

    public bool startOnce = false;
    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        path = gameObject.GetComponent<AIPath>().path.vectorPath;

        if(path != null)
        {
            StartSpawningEnemies();
        }
    }

    public void StartSpawningEnemies()
    {
        if(!startOnce)
        {
            StartCoroutine("SpawnEnemyOnTick");
            startOnce = true;
        }
    }

    IEnumerator SpawnEnemyOnTick()
    {
        while (true)
        {
            GameObject enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);

            enemy.GetComponent<Enemy>().path = path;
            yield return new WaitForSeconds(1);
        }

    }
}
