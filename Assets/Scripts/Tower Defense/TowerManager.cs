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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //delete held tower when its not hovering over square
        if(Input.anyKeyDown && !isTowerHovering)
        {
            DeleteHeldTower();
        }
        //delete held tower while its hovering over a square
        if(Input.GetMouseButtonDown(1))
        {
            DeleteHeldTower();
        }
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
        //towerToPlace.GetComponent<Tower>().rotationSelect.SetActive(true);


        //switch (type)
        //{
        //    case InstrumentType.Drums:
        //        Conductor.Instance.drums.volume = 0.5f;
        //        break;

        //    case InstrumentType.Guitar:
        //        Conductor.Instance.guitarH.volume = 0.5f;
        //        Conductor.Instance.guitarM.volume = 0.5f;
        //        break;

        //    case InstrumentType.Bass:
        //        Conductor.Instance.bass.volume = 0.5f;
        //        break;

        //    case InstrumentType.Piano:
        //        Conductor.Instance.piano.volume = 0.5f;
        //        break;

        //    default:
        //        break;
        //}
    }


    public void DeleteHeldTower()
    {
        Destroy(towerToPlace);
        towerToPlace = null;
    }


}
