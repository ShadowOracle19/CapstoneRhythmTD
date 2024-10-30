using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private EnemySpawner enemySpawners;

    public CombatMaker currentEncounter;

    [SerializeField] public Transform enemiesParent;
    [SerializeField] public Transform towersParent;
    [SerializeField] public Transform projectilesParent;

    [Header("Resources")]
    public int resourceNum;
    public int maxResource = 100;
    public Slider resourceSlider;
    public TextMeshProUGUI resourceText;

    [Header("beat indicator")]
    public GameObject beatPrefab;
    public Transform beatParent;
    public Transform beatSpawnPoint;
    public Transform beatEndPoint;

    // Start is called before the first frame update
    void Start()
    {
        //LoadEncounter(currentEncounter);
    }

   

    public void RestartEncounter()
    {
        EndEncounter();
        LoadEncounter(currentEncounter);
    }

    //play this when loading up an encounter
    public void LoadEncounter(CombatMaker encounter)
    {
        GameManager.Instance.winState = false;
        currentEncounter = encounter;
        GameManager.Instance._currentHealth = GameManager.Instance._maxHealth;
        ConductorV2.instance.gameObject.SetActive(true);

        allEnemiesSpawned = false;

        enemyTotal = currentEncounter.enemyTotal;
        enemySpawners.numberOfEnemiesToSpawn = enemyTotal;
        enemySpawners.startOnce = false;
        enemySpawners.currentNumberOfEnemiesSpawned = 0;

        resourceNum = 10;

        //Conductor.Instance.bass.volume = 0;
        //Conductor.Instance.piano.volume = 0;
        //Conductor.Instance.guitarH.volume = 0;
        //Conductor.Instance.guitarM.volume = 0;
        //Conductor.Instance.drums.volume = 0;

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
        foreach(Transform child in beatParent)
        {
            Destroy(child.gameObject);
        }
        Cursor.lockState = CursorLockMode.None;
        
    }

    // Update is called once per frame
    void Update()
    {
        //resource stuff
        resourceNum = Mathf.Clamp(resourceNum, 0, maxResource);
        resourceSlider.value = resourceNum;
        resourceText.text = resourceNum.ToString();

        //checks if all enemies have spawned
        if (!enemySpawners.allEnemiesSpawned)
        {
            allEnemiesSpawned = false;
            
        }
        else
        {
            allEnemiesSpawned = true;
        }
        
        //checks if all enemies have died or player health hasnt reached zero to give a win state
        if(allEnemiesSpawned && enemyTotal == 0 && GameManager.Instance._currentHealth != 0)
        {
            GameManager.Instance.WinLevel();
        }

        //delays enemy spawning
        DelayTimer();
    }

    void DelayTimer()
    {
        //Start spawning enemies on the 10th bar
        if (ConductorV2.instance.completedLoops >= 10)
        {
            enemySpawners.StartSpawningEnemies();
        }

    }

    public void GenerateResource()
    {
        resourceNum += 1;
    }
     
    public void SpawnBeat()
    {
        if (GameManager.Instance.winState || GameManager.Instance.loseState || GameManager.Instance.isGamePaused) return;
        GameObject beat = Instantiate(beatPrefab, beatSpawnPoint.position, Quaternion.identity, beatParent);
        beat.GetComponent<BeatMover>().endPos = beatEndPoint;
    }
}
