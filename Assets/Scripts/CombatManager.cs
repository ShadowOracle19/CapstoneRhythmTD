using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    #region dont touch this
    private static CombatManager _instance;
    public static CombatManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("CombatManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    [SerializeField] private float enemySpawnDelay = 30.0f;
    public float timeRemaining = 0;
    public bool spawnerDelayRunning = false;
    public bool allEnemiesSpawned = false;
    public int enemyTotal = 0;
    [SerializeField] private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = enemySpawnDelay;
        spawnerDelayRunning = true;

        foreach (var spawner in enemySpawners)
        {
            enemyTotal += spawner.numberOfEnemiesToSpawn;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var spawner in enemySpawners)
        {
            if(!spawner.allEnemiesSpawned)
            {
                allEnemiesSpawned = false;
                break;
            }
            else
            {
                allEnemiesSpawned = true;
            }
        }

        if(allEnemiesSpawned && enemyTotal == 0 && GameManager.Instance._currentHealth != 0)
        {
            GameManager.Instance.WinLevel();
        }

        if (spawnerDelayRunning)
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
