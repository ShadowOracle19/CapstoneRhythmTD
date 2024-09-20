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

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int _maxHealth = 5;
    [SerializeField] private int _currentHealth = 0;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Health();
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
