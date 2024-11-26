using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerManager : MonoBehaviour
{
    #region dont touch this
    private static TowerManager _instance;
    public static TowerManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("tower Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public GameObject towerToPlace;
    public bool isTowerHovering = false;

    //list of current towers the player has
    public List<GameObject> towers = new List<GameObject>();

    //Note: After this sprint replace to be more dynamic
    public GameObject drumCooldownSlot;
    public bool drumCooldown;
    public float drumCooldownTimeRemaining = 0;
    public float drumCooldownTime = 0;

    public GameObject bassCooldownSlot;
    public bool bassCooldown;
    public float bassCooldownTimeRemaining = 0;
    public float bassCooldownTime = 0;

    public GameObject guitarCooldownSlot;
    public bool guitarCooldown;
    public float guitarCooldownTimeRemaining = 0;
    public float guitarCooldownTime = 0;

    public GameObject pianoCooldownSlot;
    public bool pianoCooldown;
    public float pianoCooldownTimeRemaining = 0;
    public float pianoCooldownTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cooldown();
    }

    public void Cooldown()
    {
        drumCooldownSlot.SetActive(drumCooldown);
        bassCooldownSlot.SetActive(bassCooldown);
        pianoCooldownSlot.SetActive(pianoCooldown);
        guitarCooldownSlot.SetActive(guitarCooldown);

        if(drumCooldown)
        {
            drumCooldownTime += Time.deltaTime;

            //cooldown effect
            drumCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(drumCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((drumCooldownTime / drumCooldownTimeRemaining) * 100));

            if(drumCooldownTime >= drumCooldownTimeRemaining)
            {
                drumCooldown = false;
                drumCooldownTime = 0;
            }
            
        }
        if(bassCooldown)
        {
            bassCooldownTime += Time.deltaTime;

            //cooldown effect
            bassCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(bassCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((bassCooldownTime / bassCooldownTimeRemaining) * 100));

            if (bassCooldownTime >= bassCooldownTimeRemaining)
            {
                bassCooldown = false;
                bassCooldownTime = 0;
            }
        }
        if(guitarCooldown)
        {
            guitarCooldownTime += Time.deltaTime;

            //cooldown effect
            guitarCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(guitarCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((guitarCooldownTime / guitarCooldownTimeRemaining) * 100));

            if (guitarCooldownTime >= guitarCooldownTimeRemaining)
            {
                guitarCooldown = false;
                guitarCooldownTime = 0;
            }
        }
        if(pianoCooldown)
        {
            pianoCooldownTime += Time.deltaTime;

            //cooldown effect
            pianoCooldownSlot.GetComponent<RectTransform>().offsetMax = new Vector2(pianoCooldownSlot.GetComponent<RectTransform>().offsetMax.x, -((pianoCooldownTime / pianoCooldownTimeRemaining) * 100));

            if (pianoCooldownTime >= pianoCooldownTimeRemaining)
            { 
                pianoCooldown = false;
                pianoCooldownTime = 0;
            }
        }
    }

    public bool CheckIfOnCoolDown(InstrumentType type)
    {
        switch (type)
        {
            case InstrumentType.Drums:
                return drumCooldown;

            case InstrumentType.Guitar:
                return guitarCooldown;

            case InstrumentType.Bass:
                return bassCooldown;

            case InstrumentType.Piano:
                return pianoCooldown;

            default:
                return true;
        }
    }

    public void SetTower(GameObject tower, Vector3 tilePosition, Tile tile, InstrumentType type, _BeatResult result)
    {
        GameObject _tower = Instantiate(tower, tilePosition, Quaternion.identity, CombatManager.Instance.towersParent);
        _tower.GetComponent<SpriteFollowMouse>().enabled = false;
        _tower.GetComponent<BoxCollider2D>().enabled = true;
        _tower.transform.position = tilePosition;
        tile.placedTower = _tower;
        _tower.GetComponent<Tower>().rotateStarted = true;
        _tower.GetComponent<Tower>().connectedTile = tile;
        ConductorV2.instance.triggerEvent.Add(_tower.GetComponent<Tower>().trigger);
        //towerToPlace.GetComponent<Tower>().rotationSelect.SetActive(true);

        switch (result)
        {
            case _BeatResult.miss:
                _tower.GetComponent<Tower>().currentDamage = _tower.GetComponent<Tower>().towerInfo.damage;


                break;
            case _BeatResult.early:
                _tower.GetComponent<Tower>().currentDamage = _tower.GetComponent<Tower>().towerInfo.damage + 1;
                break;
            case _BeatResult.perfect:
                _tower.GetComponent<Tower>().currentDamage = _tower.GetComponent<Tower>().towerInfo.damage + 2;
                break;
            default:
                break;
        }

        switch (type)
        {
            case InstrumentType.Drums:
                ConductorV2.instance.drums.volume = 0.5f;
                drumCooldown = true;
                drumCooldownTimeRemaining = tower.GetComponent<Tower>().towerInfo.cooldownTime;
                drumCooldownTime = 0;
                break;

            case InstrumentType.Guitar:
                ConductorV2.instance.guitarH.volume = 0.5f;
                ConductorV2.instance.guitarM.volume = 0.5f;
                guitarCooldown = true;
                guitarCooldownTimeRemaining = tower.GetComponent<Tower>().towerInfo.cooldownTime;
                guitarCooldownTime = 0;
                break;

            case InstrumentType.Bass:
                ConductorV2.instance.bass.volume = 0.5f;
                bassCooldown = true;
                bassCooldownTimeRemaining = tower.GetComponent<Tower>().towerInfo.cooldownTime;
                bassCooldownTime = 0;
                break;

            case InstrumentType.Piano:
                ConductorV2.instance.piano.volume = 0.5f;
                pianoCooldown = true;
                pianoCooldownTimeRemaining = tower.GetComponent<Tower>().towerInfo.cooldownTime;
                pianoCooldownTime = 0;
                break;

            default:
                break;
        }
    }

}
