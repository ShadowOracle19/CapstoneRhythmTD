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

    public TextMeshProUGUI enemiesSpawnIn;
    public int enemyTimerMax = 30;
    public int enemyTimer = 40;
    bool switchColor = false;

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

    [Header("Combat UI")]
    public GameObject enemyTimerObject;
    public GameObject healthBar;
    public GameObject controls;
    public GameObject resources;
    public GameObject towerDisplay;
    public GameObject feverBar;
    public GameObject metronome;
    public GameObject waveCounter;
    public GameObject combo;
    public GameObject knockEmDead;

    public GameObject grid;
    public int pianoResourceGen = 5;
    public bool firstPianoTap = true;
    public CursorTD cursor;
    public _BeatResult resultForPiano;
    int pianoGenMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        //LoadEncounter(currentEncounter);
        grid = transform.GetChild(10).transform.GetChild(0).gameObject;
        cursor = transform.GetChild(11).GetComponent<CursorTD>();
    }


    public void RestartEncounter()
    {
        EndEncounter();
        LoadEncounter(currentEncounter);
    }

    //play this when loading up an encounter
    public void LoadEncounter(CombatMaker encounter)
    {
        GameManager.Instance.playerInputManager.SetActive(true);
        GameManager.Instance.menuMusic.Stop();
        GameManager.Instance.winScreen.SetActive(false);
        GameManager.Instance.winState = false;
        GameManager.Instance.gameOverScreen.SetActive(false);
        GameManager.Instance.loseState = false;
        currentEncounter = encounter;
        GameManager.Instance._currentHealth = GameManager.Instance._maxHealth;
        ConductorV2.instance.CountUsIn(currentEncounter.encounterBPM);


        allEnemiesSpawned = false;

        enemyTotal = 0;
        foreach (var item in currentEncounter.waves)
        {
            enemyTotal += item.numberOfEnemies;
        }
        enemySpawners.numberOfEnemiesToSpawn = enemyTotal;
        enemySpawners.startOnce = false;
        enemySpawners.currentNumberOfEnemiesSpawned = 0;
        enemySpawners.currentWaves = currentEncounter.waves;

        resourceNum = 24;
        enemyTimer = enemyTimerMax;
        enemiesSpawnIn.gameObject.SetActive(true);

        //Conductor.Instance.bass.volume = 0;
        //Conductor.Instance.piano.volume = 0;
        //Conductor.Instance.guitarH.volume = 0;
        //Conductor.Instance.guitarM.volume = 0;
        //Conductor.Instance.drums.volume = 0;

        CursorTD.Instance.InitializeCursor();
        CursorTD.Instance.pauseMovement = false;
        CursorTD.Instance.towerSwap = false;
        CursorTD.Instance.placementMenu.SetActive(false);

        TowerManager.Instance.ResetTowerManager();
        TowerManager.Instance.SetupResourceBars();

        CursorTD.Instance.tutorialParent.SetActive(false);
        CursorTD.Instance.tutorialPopupParent.SetActive(false);

        


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
        enemySpawners.startOnce = false;
        CursorTD.Instance.pauseMovement = true;
        Cursor.lockState = CursorLockMode.Locked;

        GameManager.Instance.menuMusic.Play();
        GameManager.Instance.playerInputManager.SetActive(false);
        ConductorV2.instance.drums.volume = 0;
        ConductorV2.instance.bass.volume = 0;
        ConductorV2.instance.piano.volume = 0;
        ConductorV2.instance.guitarH.volume = 0;
        ConductorV2.instance.guitarM.volume = 0;

        FeverSystem.Instance.feverBarNum = 0;
        ComboManager.Instance.ResetCombo();
        ComboManager.Instance.highestCombo = 0;

        ConductorV2.instance.StopMusic();
        GameManager.Instance.tutorialRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        TowerManager.Instance.TowerCost();

        //resource stuff
        resourceNum = Mathf.Clamp(resourceNum, 0, maxResource);
        resourceSlider.value = resourceNum;
        resourceText.text = resourceNum.ToString();
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) { resultForPiano = cursor.CheckOnBeat(); }
        if(cursor.tile != null && cursor.tile.placedTower != null)
        {
            if (cursor.tile.placedTower.name == "Piano(Clone)")
            {
                if (firstPianoTap) {
                    switch (resultForPiano)
                    {
                        case _BeatResult.perfect:
                            pianoGenMultiplier = 5;
                            break;
                        case _BeatResult.great:
                            pianoGenMultiplier = 3;
                            break;
                        case _BeatResult.early:
                            pianoGenMultiplier = 1;
                            break;
                        case _BeatResult.late:
                            pianoGenMultiplier = 1;
                            break;
                        case _BeatResult.miss:
                            pianoGenMultiplier = 0;
                            break;
                        default:
                            pianoGenMultiplier = 1;
                            break;
                    }
                    print(pianoGenMultiplier);
                    if (resourceNum + (pianoResourceGen * pianoGenMultiplier) < maxResource)
                    {
                        resourceNum += pianoResourceGen * pianoGenMultiplier;
                    }
                    else
                    {
                        resourceNum = 100;
                    }
                    print("generated" + resourceNum);
                    firstPianoTap = false;
                }
            }
            else
            {
                firstPianoTap = true;
            }
        }
        else
        {
            firstPianoTap = true;
        }
            /*for (int i = 0; i < grid.transform.childCount; i++){
                if (grid.transform.GetChild(i).GetComponent<Tile>().placedTower != null)
                {
                    print(grid.transform.GetChild(i).GetComponent<Tile>().placedTower.name);
                    if (grid.transform.GetChild(i).GetComponent<Tile>().placedTower.name == "Piano")
                    {
                        print("cringe");
                    }
                }
            }*/



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
        if(allEnemiesSpawned && enemyTotal == 0 && GameManager.Instance._currentHealth != 0 && !GameManager.Instance.tutorialRunning)
        {
            GameManager.Instance.WinLevel();
        }

        if(allEnemiesSpawned && enemyTotal == 0 && GameManager.Instance._currentHealth != 0 && GameManager.Instance.tutorialRunning)
        {
            GameManager.Instance.TutorialWinState();
        }

        //delays enemy spawning
        DelayTimer();
    }

    void DelayTimer()
    {
        enemiesSpawnIn.text = "Enemies Spawn in " + enemyTimer;
        //Start spawning enemies on the 10th bar
        if (ConductorV2.instance.numberOfBeats >= enemyTimerMax)
        {
            enemiesSpawnIn.gameObject.SetActive(false);
            enemySpawners.StartSpawningEnemies();
        }

    }
    public void BeatCountdown()
    {
        enemyTimer -= 1;
        if (switchColor)
            enemiesSpawnIn.color = Color.red;
        else
            enemiesSpawnIn.color = Color.blue;
        switchColor = !switchColor;
    }

    public void GenerateResource()
    {
        if (GameManager.Instance.tutorialRunning && CursorTD.Instance.movementSequence)
            return;

        if (GameManager.Instance.tutorialRunning && resourceNum >= 24 && !CursorTD.Instance.towerPlaceSequence && !CursorTD.Instance.towerBuffSequence && !CursorTD.Instance.feverModeSequence && !CursorTD.Instance.towerPlacementMenuSequencePassed)
        {
            CursorTD.Instance.tutorialPopupParent.SetActive(true);
            CursorTD.Instance.tutorialText.text = "Now you have enough magic press space to open the tower place menu!";
            CursorTD.Instance.towerPlacementMenuSequence = true;
            return;
        }
        resourceNum += 1;
    }
     
    public void SpawnBeat()
    {
        if (GameManager.Instance.winState || GameManager.Instance.loseState || GameManager.Instance.isGamePaused) return;

        GameObject beat = Instantiate(beatPrefab, beatSpawnPoint.position, Quaternion.identity, beatParent);
        beat.GetComponent<BeatMover>().endPos = beatEndPoint;
    }
}
