using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverSystem : MonoBehaviour
{
    #region dont touch this
    private static FeverSystem _instance;
    public static FeverSystem Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("FeverSystem Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public Slider feverBar;
    public bool feverModeActive = false;
    public int feverBarNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        feverBarNum = Mathf.Clamp(feverBarNum, 0, 100);
        feverBar.value = feverBarNum;
        if(feverBarNum == 0)
        {
            feverModeActive = false;
        }
    }

    public void ActivateFeverMode()
    {
        if(feverBarNum == 100)
        {
            feverModeActive = true;

            if(GameManager.Instance.tutorialRunning && CursorTD.Instance.feverModeSequence)
            {
                CursorTD.Instance.feverModeSequence = false;
                CursorTD.Instance.arrowKeyParent.SetActive(false);

                CursorTD.Instance.tutorialText.text = "Good job now just finish off this last wave of enemies!";
            }

        }
    }

    public void FeverBarBeat()
    {
        if (feverModeActive)
        {
            feverBarNum -= 5;
        }
        else
        {
            feverBarNum += 1 * ComboManager.Instance.currentMultiplier;

            if (GameManager.Instance.tutorialRunning && CursorTD.Instance.feverModeSequence && feverBarNum >= 99 && !EnemySpawner.Instance.allEnemiesSpawned)
            {
                CursorTD.Instance.tutorialText.text = "Quick activate fever mode by pressing the S key!";

                EnemySpawner.Instance.ForceEnemySpawn(EnemySpawner.Instance.spawnTiles[0].transform.position.y, EnemyType.Walker);
                EnemySpawner.Instance.ForceEnemySpawn(EnemySpawner.Instance.spawnTiles[1].transform.position.y, EnemyType.Walker);
                EnemySpawner.Instance.ForceEnemySpawn(EnemySpawner.Instance.spawnTiles[2].transform.position.y, EnemyType.Walker);
                EnemySpawner.Instance.ForceEnemySpawn(EnemySpawner.Instance.spawnTiles[3].transform.position.y, EnemyType.Walker);
                EnemySpawner.Instance.ForceEnemySpawn(EnemySpawner.Instance.spawnTiles[4].transform.position.y, EnemyType.Walker);
                EnemySpawner.Instance.allEnemiesSpawned = true;
            }

        }

    }
}
