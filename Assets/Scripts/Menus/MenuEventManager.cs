using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuEventManager : MonoBehaviour
{
    #region dont touch this
    private static MenuEventManager _instance;
    public static MenuEventManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("MenuEventManager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion
    EventSystem eventSystem;

    public GameObject pauseMenuFirstObject;
    public GameObject titleMenuFirstObject;
    public GameObject DialogueMenuFirstObject;
    public GameObject mainMenuFirstObject;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    public void CloseSettings()
    {
        if (GameManager.Instance.pauseMenuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(pauseMenuFirstObject);
        }
        else if(GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleMenuFirstObject);
        }
    }

    public void DialogueOpen()
    {
        eventSystem.SetSelectedGameObject(DialogueMenuFirstObject);
    }

    public void PauseMenuOpen()
    {
        eventSystem.SetSelectedGameObject(pauseMenuFirstObject);
    }

    public void PauseMenuClose()
    {
        if (GameManager.Instance.titleRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(titleMenuFirstObject);
        }
        else if(GameManager.Instance.menuRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(mainMenuFirstObject);
        }
        else if(GameManager.Instance.dialogueRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(DialogueMenuFirstObject);
        }
        else if(GameManager.Instance.combatRoot.activeSelf)
        {
            eventSystem.SetSelectedGameObject(null);
        }
    }
}
