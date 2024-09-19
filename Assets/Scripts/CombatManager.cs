using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private float enemySpawnDelay = 30.0f;
    public float timeRemaining = 0;
    public bool spawnerDelayRunning = false;

    [SerializeField] private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = enemySpawnDelay;
        spawnerDelayRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnerDelayRunning)
        {
            if(timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                spawnerDelayRunning = false;
                foreach (var spawner in enemySpawners)
                {
                    spawner.StartSpawningEnemies();
                }
            }
        }
    }
}
