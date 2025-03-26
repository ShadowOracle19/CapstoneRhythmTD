using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI tutorialText;
    public bool pressKeyToNextDialogue = false;

    public List<TutorialDialogue> tutorialDialogue = new List<TutorialDialogue>();

    [SerializeField]private GameObject pressKeyToNextPopup;
    public int index = 0;

    public void LoadTutorial()
    {
        tutorialPopup.SetActive(true);
        index = 0;
        tutorialText.text = tutorialDialogue[index].text;
        pressKeyToNextDialogue = tutorialDialogue[index].isSkippableText;
    }


    // Update is called once per frame
    void Update()
    {
        pressKeyToNextPopup.SetActive(pressKeyToNextDialogue);

        if(pressKeyToNextDialogue && Input.anyKeyDown && !ConductorV2.instance.countingIn)
        {
            LoadNextTutorialDialogue();
        }
    }


    public void LoadNextTutorialDialogue()
    {
        index += 1; 
        if (index > tutorialDialogue.Count - 1)
        {
            tutorialPopup.SetActive(false);
            pressKeyToNextDialogue = false;
            GameManager.Instance.WinLevel();
            return;
        }
        tutorialText.text = tutorialDialogue[index].text;
        pressKeyToNextDialogue = tutorialDialogue[index].isSkippableText;

        
    }
}

[System.Serializable]
public class TutorialDialogue
{
    [TextArea(5, 20)]
    public string text;
    public bool isSkippableText;
}
