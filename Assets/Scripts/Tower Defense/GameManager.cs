using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region dont touch this
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("GameManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public Transform globelParent;
    public Transform projectileParent;

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] public GameObject winScreen;
    [SerializeField] private GameObject conductor;

    [Header("Screen Roots")]
    public GameObject combatRoot;
    public GameObject dialogueRoot;
    public GameObject menuRoot;
    [SerializeField] private GameObject settings;
    public GameObject pauseMenuRoot;
    public GameObject titleRoot;

    [Header("Combat")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] public int _maxHealth = 5;
    [SerializeField] public int _currentHealth = 0;
    [SerializeField] public bool combatRunning = false;
    public TextMeshProUGUI waveCounter;

    [Header("Pause Menu")]
    [SerializeField] public bool isGamePaused = false;
    [SerializeField] private Button restartEncounterButton;
    //[SerializeField] private GameObject pauseMenu;


    [Header("Encounter")]
    public EncounterCreator currentEncounter;
    public bool encounterRunning = false;
    public bool winState = false;
    public bool loseState = false;
    public bool tutorialRunning = false;

    [Header("Dialogue")]
    public float textSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        //make sure when scene starts title root is set active
        titleRoot.SetActive(true);
        menuRoot.SetActive(false);
        combatRoot.SetActive(false);
        dialogueRoot.SetActive(false);
        settings.SetActive(false);
        pauseMenuRoot.SetActive(false);

        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Manages health only while combat is running
        if (combatRunning)
        {
            Health();
        }

        //allows player to use pause menu in combat, level select and dialogue
        if (combatRunning || menuRoot.activeSelf || dialogueRoot.activeSelf || tutorialRunning)
        {
            //When the escape button is pressed the game will pause and open the pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //flips the isGamePaused bool
                isGamePaused = !isGamePaused;

                //game is paused
                if (isGamePaused)
                {
                    PauseGame();
                }
                //game is unpaused
                else
                {
                    ResumeGame();
                }
            }
        }

        if(menuRoot.activeSelf || dialogueRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

    public void LoadTutorial()
    {
        tutorialRunning = true;
        winState = false;
        combatRoot.SetActive(true);

        CombatManager.Instance.enemyTimerObject.SetActive(false);
        CombatManager.Instance.healthBar.SetActive(false);
        CombatManager.Instance.controls.SetActive(false);
        CombatManager.Instance.resources.SetActive(false);
        CombatManager.Instance.towerDisplay.SetActive(false);
        CombatManager.Instance.feverBar.SetActive(false);
        CombatManager.Instance.metronome.SetActive(false);
        CombatManager.Instance.waveCounter.SetActive(false);
        CombatManager.Instance.combo.SetActive(false);

        CursorTD.Instance.tutorialParent.SetActive(true); 


        CursorTD.Instance.movementSequence = false;
        CursorTD.Instance.towerPlacementMenuSequence = false;
        CursorTD.Instance.towerPlaceSequence = false;
        CursorTD.Instance.towerBuffSequence = false;
        CursorTD.Instance.feverModeSequence = false;

        CursorTD.Instance.wasdParent.transform.localPosition = Vector3.zero;
        CursorTD.Instance.wasdParent.SetActive(false);
        CursorTD.Instance.arrowKeyParent.SetActive(false);
        CursorTD.Instance.spaceKeyParent.SetActive(false);

        CursorTD.Instance.InitializeCursor();
        CursorTD.Instance.pauseMovement = false;
        CursorTD.Instance.towerSwap = false;
        CursorTD.Instance.placementMenu.SetActive(false);

        TowerManager.Instance.drumCooldown = false;
        TowerManager.Instance.bassCooldown = false;
        TowerManager.Instance.pianoCooldown = false;
        TowerManager.Instance.guitarCooldown = false;

        TowerManager.Instance.drumCooldownBack = false;
        TowerManager.Instance.bassCooldownBack = false;
        TowerManager.Instance.pianoCooldownBack = false;
        TowerManager.Instance.guitarCooldownBack = false;


        ConductorV2.instance.CountUsIn(80);
        CombatManager.Instance.enemyTotal = 7;


        List<Wave> empty = new List<Wave>();
        EnemySpawner.Instance.currentWaves = empty;
        EnemySpawner.Instance.numberOfEnemiesToSpawn = 7;
        EnemySpawner.Instance.startOnce = false;
        EnemySpawner.Instance.currentNumberOfEnemiesSpawned = 0;

        EnemySpawner.Instance.currentNumberOfEnemiesSpawned = 0;

        CursorTD.Instance.tutorialPopupParent.SetActive(true);
        CursorTD.Instance.tutorialText.text = "Use WASD keys to move the cursor";
    }

    public void LoadEncounter(EncounterCreator encounter)
    {
        tutorialRunning = false;
        currentEncounter = encounter;
        encounterRunning = true;
        dialogueRoot.SetActive(true);
        DialogueManager.Instance.LoadDialogue(currentEncounter.introDialogue);

        CombatManager.Instance.enemyTimerObject.SetActive(true);
        CombatManager.Instance.healthBar.SetActive(true);
        CombatManager.Instance.controls.SetActive(true);
        CombatManager.Instance.resources.SetActive(true);
        CombatManager.Instance.towerDisplay.SetActive(true);
        CombatManager.Instance.feverBar.SetActive(true);
        CombatManager.Instance.metronome.SetActive(true);
        CombatManager.Instance.waveCounter.SetActive(true);
        CombatManager.Instance.combo.SetActive(true);


        
    }

    public void Damage()
    {
        _currentHealth -= 1;
    }

    void Health()
    {
        healthSlider.maxValue = _maxHealth;
        healthSlider.value = _currentHealth;

        if (_currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void PauseGame()
    {
        if (dialogueRoot.activeSelf || menuRoot.activeSelf)
        {
            //set the restart encounter button to disabled
            restartEncounterButton.interactable = false;
        }
        else
        {
            restartEncounterButton.interactable = true;
            
            //conductor.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.None;
        isGamePaused = true;
        pauseMenuRoot.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (combatRunning && !dialogueRoot.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            conductor.SetActive(true);
        }
        
        isGamePaused = false;
        pauseMenuRoot.SetActive(false);
        settings.SetActive(false);
        Time.timeScale = 1;
    }


    public void GameOver()
    {
        loseState = true;
        Cursor.lockState = CursorLockMode.None;
        CombatManager.Instance.EndEncounter();
        gameOverScreen.SetActive(true);
        //conductor.SetActive(false);
        ConductorV2.instance.StopMusic();
    }

    public void WinLevel()
    {
        if (winState) return;
        winState = true;
        CombatManager.Instance.EndEncounter();
        encounterRunning = false;
        Cursor.lockState = CursorLockMode.None;
        //winScreen.SetActive(true);
        dialogueRoot.SetActive(true);
        DialogueManager.Instance.LoadDialogue(currentEncounter.endDialogue);
        //conductor.SetActive(false);
        ConductorV2.instance.StopMusic();
    }

    public void TutorialWinState()
    {
        if (winState) return;
        winState = true;
        CombatManager.Instance.EndEncounter();
        encounterRunning = false;
        Cursor.lockState = CursorLockMode.None;

        CursorTD.Instance.tutorialPopupParent.SetActive(false);

        winScreen.SetActive(true);
        //conductor.SetActive(false);
        ConductorV2.instance.StopMusic();
    }

}
