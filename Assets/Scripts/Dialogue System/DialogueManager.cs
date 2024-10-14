using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region JSON Variables

    [System.Serializable]
    public class Dialogue
    {
        public string name;
        public string text;
    }

    [System.Serializable]
    public class DialogueList
    {
        public Dialogue[] dialogue;
    }
    #endregion

    #region dont touch this
    private static DialogueManager _instance;
    public static DialogueManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("DialogueManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public TextAsset currentDialogue;


    public TextMeshProUGUI _speakerName;
    public TextMeshProUGUI _dialogue;
    public int index;

    public DialogueList myDialogue = new DialogueList();

    public GameObject dialogueBox;

    public bool dialogueFinished = false;
    public GameObject dialogueSystemParent;

    Coroutine typing;

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadDialogue(TextAsset desiredDialogue)
    {
        currentDialogue = desiredDialogue;
        dialogueBox.SetActive(true);
        myDialogue = JsonUtility.FromJson<DialogueList>(currentDialogue.text);
        index = 0;
        _speakerName.text = string.Empty;
        _dialogue.text = string.Empty;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in myDialogue.dialogue[index].text.ToCharArray())
        {
            _speakerName.text = myDialogue.dialogue[index].name;
            _dialogue.text += c;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void NextLine()
    {
        if(index < myDialogue.dialogue.Length - 1)
        {
            index++;
            _dialogue.text = string.Empty;
            typing = StartCoroutine(TypeLine());
        }
        else //dialogue finished
        {
            //end dialogue
            StopCoroutine(typing);
            //dialogue if its going into a combat
            if(GameManager.Instance.encounterRunning)
            {
                GameManager.Instance.combatRoot.SetActive(true);
                GameManager.Instance.combatRunning = true;
                CombatManager.Instance.LoadEncounter(GameManager.Instance.currentEncounter.combatEncounter);
                GameManager.Instance.dialogueRoot.SetActive(false);
                return;
            }
            //dialogue after combat
            if(GameManager.Instance.winState)
            {
                GameManager.Instance.winScreen.SetActive(true);
                GameManager.Instance.dialogueRoot.SetActive(false);
            }

            dialogueSystemParent.SetActive(false);
            return;
        }
    }

}
