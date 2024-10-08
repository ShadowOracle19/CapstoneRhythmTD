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

    public void PlaceTower(Vector3 tilePosition, Tile tile)
    {
        towerToPlace.GetComponent<SpriteFollowMouse>().enabled = false;
        towerToPlace.GetComponent<BoxCollider2D>().enabled = true;
        towerToPlace.transform.position = tilePosition;
        tile.placedTower = towerToPlace;
        towerToPlace.GetComponent<SpriteRenderer>().sortingOrder = 2;
        towerToPlace.GetComponent<Tower>().rotateStarted = true;
        //towerToPlace.GetComponent<Tower>().rotationSelect.SetActive(true);

        Conductor.Instance._intervals.Add(towerToPlace.GetComponent<Tower>().interval); 

        towerToPlace = null;
        
    }

    public void DeleteHeldTower()
    {
        Destroy(towerToPlace);
        towerToPlace = null;
    }


}
