using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    #region dont touch this
    private static ComboManager _instance;
    public static ComboManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("ComboManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public int currentCombo;
    public int highestCombo;
    public int currentMultiplier;
    public int streak;

    // Start is called before the first frame update
    void Start()
    {
        currentMultiplier = 1;
    }

    // Update is called once per frame
    void Update()
    {
        currentMultiplier = Mathf.Clamp(currentMultiplier, 1, 5);
    }

    public void IncreaseCombo()
    {
        streak += 1;
        if(streak == 10)
        {
            currentMultiplier += 1;
            streak = 0;
        }
        currentCombo += 1 * currentMultiplier;
    }

    public void ResetCombo()
    {
        if(currentCombo > highestCombo)
        {
            highestCombo = currentCombo;
        }
        currentCombo = 0;
        streak = 0;
        currentMultiplier = 1;
    }
}