using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CursorTD : MonoBehaviour
{
    #region dont touch this
    private static CursorTD _instance;
    public static CursorTD Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("CursorTD Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    public bool isMoving = false;
    private Vector3 originPos, targetPos;
    public float timeToMove = 1f;

    public Vector3 desiredMovement;

    //placement menu
    private bool towerSelectMenuOpened = false;
    public Tile tile;
    public GameObject placementMenu;
    public GameObject SlotW;
    public GameObject SlotA;
    public GameObject SlotS;
    public GameObject SlotD;

    public GameObject cursorSprite;
    public Vector3 pulseSize;

    public GameObject beatHitResultPrefab;

    public bool pauseMovement = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMovement)
            return;

        //return cursor sprite to origin size
        cursorSprite.transform.localScale = Vector3.Lerp(cursorSprite.transform.localScale, Vector3.one, Time.deltaTime * 5);
        

        if (GameManager.Instance.winState) return;
        if(Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftControl) && (tile != null && tile.placedTower == null))
        {
            TogglePlacementMenu();
        }

        if(tile != null && tile.placedTower != null)
        {
            tile.placedTower.GetComponent<Tower>().towerHover = true;
        }

        DestroyMode();
        HighlightPlacementSlot();
        MoveCursor();
        TowerEmpowermentInput();
        Move();
    }

    public void DestroyMode()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            cursorSprite.GetComponent<SpriteRenderer>().color = Color.red;
            if (tile != null && tile.placedTower != null && Input.GetKeyDown(KeyCode.Space))
            {
                tile.placedTower.GetComponent<Tower>().RemoveTower();


            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            cursorSprite.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void InitializePlacementMenu()
    {
        SlotW.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[0];
        SlotW.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[0].GetComponent<Tower>().towerInfo.towerImage;

        SlotA.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[1];
        SlotA.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[1].GetComponent<Tower>().towerInfo.towerImage;


        SlotS.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[2];
        SlotS.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[2].GetComponent<Tower>().towerInfo.towerImage;


        SlotD.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[3];
        SlotD.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[3].GetComponent<Tower>().towerInfo.towerImage;

    }

    public void MoveCursor()
    {
        if (isMoving) return;
        //on press log which direction needs to be moved
        if (Input.GetKeyUp(KeyCode.W))
        {
            //movement
            desiredMovement = Vector3.up;
            SpawnBeatHitResult();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            //movement
            desiredMovement = Vector3.left;
            SpawnBeatHitResult();

        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            //movement
            desiredMovement = Vector3.down;
            SpawnBeatHitResult();

        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            //movement
            desiredMovement = Vector3.right;

            SpawnBeatHitResult();
        }
        
    }

    public void TowerEmpowermentInput()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            TowerEmpowerment();
            SpawnBeatHitResult();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            TowerEmpowerment();
            SpawnBeatHitResult();

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            TowerEmpowerment();
            SpawnBeatHitResult();

        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            TowerEmpowerment();
            SpawnBeatHitResult();
        }
    }

    public void TowerEmpowerment()
    {
        if(tile.placedTower != null)
        {
            if (ConductorV2.instance.beatDuration >= ConductorV2.instance.perfectBeatThreshold)//perfect beat hit 
            {
                tile.placedTower.GetComponentInChildren<SpriteRenderer>().color = Color.green;
            }
            else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.earlyBeatThreshold)//early beat hit
            {
                tile.placedTower.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
            }
            else if(ConductorV2.instance.beatDuration < ConductorV2.instance.earlyBeatThreshold)
            {
                tile.placedTower.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            }
        }
        else
        {
            return;
        }
    }

    public void TryToPlaceTower(GameObject tower)
    {
        //checks if resource is available and if the tower is on cooldown
        if(CombatManager.Instance.resourceNum >= tower.GetComponent<Tower>().towerInfo.resourceCost 
            && !TowerManager.Instance.CheckIfOnCoolDown(tower.GetComponent<Tower>().towerInfo.type)) 
        {
            if (ConductorV2.instance.beatDuration >= ConductorV2.instance.perfectBeatThreshold)//perfect beat hit 
            {
                TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type, _BeatResult.perfect);
            }
            else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.earlyBeatThreshold)//early beat hit
            {
                TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type, _BeatResult.early);
            }
            else if(ConductorV2.instance.beatDuration < ConductorV2.instance.earlyBeatThreshold)
            {
                TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type, _BeatResult.miss);
                //miss beat

            }
            CombatManager.Instance.resourceNum -= tower.GetComponent<Tower>().towerInfo.resourceCost;
            SpawnBeatHitResult();
            TogglePlacementMenu();
            return;
        }
        else
        {
            return;
        }    
    }

    public void TogglePlacementMenu()
    {
        towerSelectMenuOpened = !towerSelectMenuOpened;
        placementMenu.SetActive(towerSelectMenuOpened);
    }

    public void HighlightPlacementSlot()
    {
        if (!towerSelectMenuOpened) return;

        if (Input.GetKeyUp(KeyCode.W))
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotW.GetComponent<TowerButton>().tower);
                return;
            }
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotA.GetComponent<TowerButton>().tower);
                return;
            }

        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotS.GetComponent<TowerButton>().tower);
                return;
            }

        }
        else if (Input.GetKeyUp(KeyCode.D))
        {

            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotD.GetComponent<TowerButton>().tower);

                return;
            }

        }
    }


    public void Move()
    {
        if (desiredMovement == Vector3.zero || towerSelectMenuOpened || isMoving) return;
        StartCoroutine(MovePlayer(desiredMovement));
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        
        isMoving = true;

        float elapsedTime = 0;

        originPos = transform.position;

        targetPos = originPos + (direction * 1.2f);


        //bounding box function
        if((targetPos.x <= -5 || targetPos.x >= 9) || (targetPos.y <= -3.5 || targetPos.y >= 2))
        {
            isMoving = false;
            desiredMovement = Vector3.zero;
            yield break;
        }

        while(elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(originPos, targetPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
        desiredMovement = Vector3.zero;
        tile = null;
        yield return null;
    }

    //check which tile cursor is on
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("StageTile"))
        {
            if ( tile != null && tile != collision.gameObject.GetComponent<Tile>())
            {
                if(tile.placedTower != null)
                {
                    tile.placedTower.GetComponent<Tower>().towerHover = false;

                }

            }
            tile = collision.gameObject.GetComponent<Tile>();
        }
    }

    public void Pulse()
    {
        //Debug.Log("pulse");
        cursorSprite.transform.localScale = pulseSize;
    }

    public void SpawnBeatHitResult()
    {
        Debug.Log(ConductorV2.instance.beatDuration);
        GameObject beatResult = Instantiate(beatHitResultPrefab, new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), Quaternion.identity);
        if (ConductorV2.instance.beatDuration >= ConductorV2.instance.perfectBeatThreshold)//perfect beat hit 
        {
            beatResult.GetComponent<TMP_Text>().text = "perfect!";
            beatResult.GetComponent<TMP_Text>().fontSize = 72;
            beatResult.GetComponent<TMP_Text>().color = Color.green;
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.earlyBeatThreshold)//early beat hit
        {
            beatResult.GetComponent<TMP_Text>().text = "early";
            beatResult.GetComponent<TMP_Text>().fontSize = 56;
            beatResult.GetComponent<TMP_Text>().color = Color.yellow;
        }
        else if(ConductorV2.instance.beatDuration < ConductorV2.instance.earlyBeatThreshold)
        {
            beatResult.GetComponent<TMP_Text>().text = "miss";//miss beat
            beatResult.GetComponent<TMP_Text>().color = Color.red;

        }
    }
    
}
