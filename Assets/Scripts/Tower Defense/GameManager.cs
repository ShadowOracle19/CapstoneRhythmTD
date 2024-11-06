using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("Combat")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] public int _maxHealth = 5;
    [SerializeField] public int _currentHealth = 0;
    [SerializeField] public bool combatRunning = false;

    [Header("Pause Menu")]
    [SerializeField] public bool isGamePaused = false;
    [SerializeField] private Button restartEncounterButton;
    //[SerializeField] private GameObject pauseMenu;


    [Header("Encounter")]
    public EncounterCreator currentEncounter;
    public bool encounterRunning = false;
    public bool winState = false;
    public bool loseState = false;

    [Header("Dialogue")]
    public float textSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
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
        if (combatRunning || menuRoot.activeSelf || dialogueRoot.activeSelf)
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
        
    }

    public void LoadEncounter(EncounterCreator encounter)
    {
        currentEncounter = encounter;
        encounterRunning = true;
        dialogueRoot.SetActive(true);
        DialogueManager.Instance.LoadDialogue(currentEncounter.introDialogue);
    }

    public void Damage()
    {
        _currentHealth -= 1;
    }

    void Health()
    {
        healthText.text = "Health: " + _currentHealth + "/" + _maxHealth;

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
        Debug.Log("Game Over");
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
        Debug.Log("Game Over");
        //winScreen.SetActive(true);
        dialogueRoot.SetActive(true);
        DialogueManager.Instance.LoadDialogue(currentEncounter.endDialogue);
        //conductor.SetActive(false);
        ConductorV2.instance.StopMusic();
    }

}
