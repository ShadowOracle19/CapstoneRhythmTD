using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    #region dont touch this
    private static TutorialManager _instance;
    public static TutorialManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("TutorialManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public GameObject tutorialPopup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTutorialPopup()
    {

    }
}
