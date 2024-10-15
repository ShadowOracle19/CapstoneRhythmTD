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

    public CombatMaker currentEncounter;

    [SerializeField] public Transform enemiesParent;
    [SerializeField] public Transform towersParent;
    [SerializeField] public Transform projectilesParent;

    // Start is called before the first frame update
    void Start()
    {
        //LoadEncounter(currentEncounter);
    }

    public void RestartEncounter()
    {
        LoadEncounter(currentEncounter);
    }

    //play this when loading up an encounter
    public void LoadEncounter(CombatMaker encounter)
    {
        currentEncounter = encounter;
        GameManager.Instance._currentHealth = GameManager.Instance._maxHealth;
        Conductor.Instance.gameObject.SetActive(true);
        timeRemaining = enemySpawnDelay;
        spawnerDelayRunning = true;
        allEnemiesSpawned = false;
        enemyTotal = currentEncounter.enemyTotal;

        foreach (var spawner in enemySpawners)
        {
            spawner.numberOfEnemiesToSpawn = enemyTotal;
            spawner.startOnce = false;
            spawner.currentNumberOfEnemiesSpawned = 0;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void EndEncounter()
    {
        //remove enemies
        foreach (Transform child in enemiesParent)
        {
            child.gameObject.GetComponent<Enemy>().RemoveEnemy();
        }
        //remove towers
        foreach (Transform child in towersParent)
        {
            child.gameObject.GetComponent<Tower>().RemoveTower();
        }
        //remove enemies
        foreach (Transform child in projectilesParent)
        {
            child.gameObject.GetComponent<Projectile>().RemoveProjectile();
        }
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.winState = false;
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

        //delays enemy spawning
        DelayTimer();
    }

    void DelayTimer()
    {
        if (spawnerDelayRunning)
        {
            if (timeRemaining > 0)
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
