using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Image characterImage;
    public int index;

    public DialogueList myDialogue = new DialogueList();

    public GameObject dialogueBox;

    public bool dialogueFinished = false;
    public GameObject dialogueSystemParent;

    public float textSpeed = 0.05f;

    Coroutine typing;

    [Header("Log")]
    public GameObject logEntry;
    public Transform logParent;
    public GameObject log;

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
        //set log to inactive so it doesnt show when dialogue is loaded
        log.SetActive(false);

        currentDialogue = desiredDialogue;
        dialogueBox.SetActive(true);
        myDialogue = JsonUtility.FromJson<DialogueList>(currentDialogue.text);
        index = 0;
        _speakerName.text = string.Empty;
        _dialogue.text = string.Empty;

        typing = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in myDialogue.dialogue[index].text.ToCharArray())
        {
            LoadCharacterSprite();

            _speakerName.text = myDialogue.dialogue[index].name;
            _dialogue.text += c;
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }
        
    }

    public void NextLine()
    {
        GameObject _newLog = Instantiate(logEntry, logParent);
        _newLog.GetComponent<DialogueLogEntry>().characterName.text = _speakerName.text;
        _newLog.GetComponent<DialogueLogEntry>().dialogue.text = _dialogue.text;

        if (index < myDialogue.dialogue.Length - 1)
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

    public void LoadCharacterSprite()
    {
        var characterSprite = Resources.Load<Sprite>(myDialogue.dialogue[index].name);

        if (characterSprite == null)
        {
            characterImage.sprite = null;
            characterImage.color = Color.clear;
        }
        else
        {

            characterImage.color = Color.white;
            characterImage.sprite = characterSprite;
        }
    }

    public void FinishLine()
    {
        if(_dialogue.text == myDialogue.dialogue[index].text)
        {
            NextLine();
        }
        else
        {
            StopCoroutine(typing);
            _dialogue.text = myDialogue.dialogue[index].text;
        }
    }

    public void SkipDialogue()
    {
        //end dialogue
        StopCoroutine(typing);

        for (int i = index; i < myDialogue.dialogue.Length; i++)
        {
            GameObject _newLog = Instantiate(logEntry, logParent);
            _newLog.GetComponent<DialogueLogEntry>().characterName.text = myDialogue.dialogue[i].name;
            _newLog.GetComponent<DialogueLogEntry>().dialogue.text = myDialogue.dialogue[i].text;
        }

        //dialogue if its going into a combat
        if (GameManager.Instance.encounterRunning)
        {
            GameManager.Instance.combatRoot.SetActive(true);
            GameManager.Instance.combatRunning = true;
            CombatManager.Instance.LoadEncounter(GameManager.Instance.currentEncounter.combatEncounter);
            GameManager.Instance.dialogueRoot.SetActive(false);
            return;
        }
        //dialogue after combat
        if (GameManager.Instance.winState)
        {
            GameManager.Instance.winScreen.SetActive(true);
            GameManager.Instance.dialogueRoot.SetActive(false);
        }

        dialogueSystemParent.SetActive(false);
    }

}
