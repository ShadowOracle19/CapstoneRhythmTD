using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region JSON Variables

    [System.Serializable]
    public class Dialogue
    {
        public string name;
        public string emotion;
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
    public Image secondCharacterImage;
    public int index;

    public DialogueList myDialogue = new DialogueList();

    public GameObject talkingDialogueBox;
    public GameObject descriptiveDialogueBox;
    public GameObject previousTalkingDialogueBox;
    private Sprite previousCharacter;

    public bool dialogueFinished = false;
    public GameObject dialogueSystemParent;

    public float textSpeed = 0.05f;
    public float defaultTextSpeed = 0.05f;
       
    // Contains a reference to the object last selected before opening a dialogue sequence
    public GameObject lastActiveObject;

    private EventSystem eventSystem;

    Coroutine typing;

    [Header("Log")]
    public GameObject logEntry;
    public Transform logParent;
    public GameObject log;
    public bool pauseDialogue = false;

    
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;

        
        

        
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
        myDialogue = JsonUtility.FromJson<DialogueList>(currentDialogue.text);
        index = 0;
        _speakerName.text = string.Empty;
        _dialogue.text = string.Empty;

        //MenuEventManager.Instance.DialogueOpen();

        typing = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        string dialogueText = myDialogue.dialogue[index].text;
        LoadCharacterSprite();
        _speakerName.text = myDialogue.dialogue[index].name;
        for (int i = 0; i < dialogueText.Length; i++)
        {
            while (pauseDialogue)
            {
                yield return new WaitForSeconds(GameManager.Instance.textSpeed);
            }
            if (dialogueText[i] == '<')
                _dialogue.text += GetCompleteRichTextTag(ref i);
            else
                _dialogue.text += dialogueText[i];
            PlayCharacterAudio();
            yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        }

        //foreach (char c in dialogueText)
        //{
        //    LoadCharacterSprite();

        //    _speakerName.text = myDialogue.dialogue[index].name;
        //    _dialogue.text += c;
        //    //if (c == '<'){
        //    //    GameManager.Instance.textSpeed = 0f;
        //    //}
        //    //else if (c == '>'){
        //    //    GameManager.Instance.textSpeed = defaultTextSpeed;
        //    //}
        //    yield return new WaitForSeconds(GameManager.Instance.textSpeed);
        //}

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

                CombatManager.Instance.enemyTimerObject.SetActive(true);
                CombatManager.Instance.healthBar.SetActive(true);
                CombatManager.Instance.controls.SetActive(true);
                CombatManager.Instance.resources.SetActive(true);
                CombatManager.Instance.towerDisplay.SetActive(true);
                CombatManager.Instance.feverBar.SetActive(true);
                CombatManager.Instance.metronome.SetActive(true);
                CombatManager.Instance.waveCounter.SetActive(true);
                CombatManager.Instance.combo.SetActive(true);

                return;
            }
            //dialogue after combat
            if(GameManager.Instance.winState)
            {
                GameManager.Instance.winScreen.SetActive(true);
                MenuEventManager.Instance.WinScreenOpen();
                GameManager.Instance.dialogueRoot.SetActive(false);
            }
            // Sets the active object to the object last active before dialogue started
            else if (GameManager.Instance.menuRoot.activeSelf)
            {
                eventSystem.SetSelectedGameObject(lastActiveObject);
            }

            dialogueSystemParent.SetActive(false);

            return;
        }
    }

    public void LoadCharacterSprite()
    {
        var characterSprite = Resources.Load<Sprite>($"Characters/{myDialogue.dialogue[index].name}/{myDialogue.dialogue[index].name}_{myDialogue.dialogue[index].emotion}");



        if (characterSprite == null)
        {
            descriptiveDialogueBox.SetActive(true);
            talkingDialogueBox.SetActive(false);
            previousTalkingDialogueBox.SetActive(false);

            characterImage.sprite = null;
            characterImage.color = Color.clear;

            secondCharacterImage.sprite = null;
            secondCharacterImage.color = Color.clear;
        }
        else
        {
            descriptiveDialogueBox.SetActive(false);
            talkingDialogueBox.SetActive(true);

            if(index != 0 && myDialogue.dialogue[index].name != myDialogue.dialogue[index - 1].name && myDialogue.dialogue[index].name != string.Empty)
            {
                secondCharacterImage.sprite = previousCharacter;
                secondCharacterImage.color = Color.white;
            }

            characterImage.color = Color.white;
            characterImage.sprite = characterSprite;
        }
        previousCharacter = characterSprite;
    }
    private void PlayCharacterAudio()
    {
        if (audioSource.isPlaying)
            return;
        int randNum = Random.Range(1, 6); //gets random number between 1 and 5
        var _characterSpeaking = Resources.Load<AudioClip>($"audio/{myDialogue.dialogue[index].name}/{myDialogue.dialogue[index].name}{randNum}");

        if (_characterSpeaking == null)
            return;
        else
        {
            GetComponent<AudioSource>().clip = _characterSpeaking;
            audioSource.Play();
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
            MenuEventManager.Instance.WinScreenOpen();
            GameManager.Instance.dialogueRoot.SetActive(false);
        }

        // Sets the active object to the object last active before dialogue started
        if (GameManager.Instance.menuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(lastActiveObject);
        }

        dialogueSystemParent.SetActive(false);
    }

    public void SetLastActiveObject(GameObject currentlyActiveObject)
    {
        lastActiveObject = currentlyActiveObject;
        EventSystem.current.SetSelectedGameObject(null);
    }

    string GetCompleteRichTextTag(ref int _index)
    {
        string completeTag = string.Empty;

        while(_index < myDialogue.dialogue[index].text.Length)
        {
            completeTag += myDialogue.dialogue[index].text[_index];
            if (myDialogue.dialogue[index].text[_index] == '>')
                return completeTag;

            _index++;
        }
        return string.Empty;
    }

    public void OpenLog()
    {
        log.SetActive(true);
        pauseDialogue = true;
        MenuEventManager.Instance.OpenLog();
        DialogueInputHandler.Instance.enabled = false;
    }

    public void CloseLog()
    {
        log.SetActive(false);
        pauseDialogue = false;
        DialogueInputHandler.Instance.enabled = true;
    }

}