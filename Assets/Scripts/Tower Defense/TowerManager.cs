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

    public List<GameObject> towers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetTower(GameObject tower, Vector3 tilePosition, Tile tile, InstrumentType type)
    {
        GameObject _tower = Instantiate(tower, tilePosition, Quaternion.identity, CombatManager.Instance.towersParent);
        _tower.GetComponent<SpriteFollowMouse>().enabled = false;
        _tower.GetComponent<BoxCollider2D>().enabled = true;
        _tower.transform.position = tilePosition;
        tile.placedTower = _tower;
        _tower.GetComponent<SpriteRenderer>().sortingOrder = 2;
        _tower.GetComponent<Tower>().rotateStarted = true;
        ConductorV2.instance.triggerEvent.Add(_tower.GetComponent<Tower>().trigger);
        //towerToPlace.GetComponent<Tower>().rotationSelect.SetActive(true);


        switch (type)
        {
            case InstrumentType.Drums:
                ConductorV2.instance.drums.volume = 0.5f;
                break;

            case InstrumentType.Guitar:
                ConductorV2.instance.guitarH.volume = 0.5f;
                ConductorV2.instance.guitarM.volume = 0.5f;
                break;

            case InstrumentType.Bass:
                ConductorV2.instance.bass.volume = 0.5f;
                break;

            case InstrumentType.Piano:
                ConductorV2.instance.piano.volume = 0.5f;
                break;

            default:
                break;
        }
    }

}
