using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject conductor;
    [SerializeField] private GameObject settings;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] public int _maxHealth = 5;
    [SerializeField] public int _currentHealth = 0;

    [SerializeField] public bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Health();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;

            //game is paused
            if(isGamePaused)
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

    public void Damage()
    {
        _currentHealth -= 1;
    }

    void Health()
    {
        healthText.text = _currentHealth + "/" + _maxHealth;

        if (_currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        pauseMenu.SetActive(true);
        conductor.SetActive(false);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        pauseMenu.SetActive(false);
        conductor.SetActive(true);
        settings.SetActive(false);
        Time.timeScale = 1;
    }


    public void GameOver()
    {
        Debug.Log("Game Over");
        gameOverScreen.SetActive(true);
        conductor.SetActive(false);
    }

    public void WinLevel()
    {
        Debug.Log("Game Over");
        winScreen.SetActive(true);
        conductor.SetActive(false);
    }
}
