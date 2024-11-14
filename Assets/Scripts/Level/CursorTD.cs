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
        if(Input.GetKeyUp(KeyCode.Space))
        {
            TogglePlacementMenu();
        }

        HighlightPlacementSlot();
        MoveCursor();
        Move();
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
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            //Debug.Log("Try move");
            desiredMovement = Vector3.up;
            if (ConductorV2.instance.InThreshHold())//beat hit or early
            {
            }
            SpawnBeatHitResult();
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            desiredMovement = Vector3.left;
            if (ConductorV2.instance.InThreshHold())//beat hit or early
            {
            }
            SpawnBeatHitResult();

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            desiredMovement = Vector3.down;
            if (ConductorV2.instance.InThreshHold())//beat hit or early
            {
            }
            SpawnBeatHitResult();

        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            desiredMovement = Vector3.right;
            if (ConductorV2.instance.InThreshHold())//beat hit or early
            {
            }

            SpawnBeatHitResult();
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
                Debug.Log("perfect Beat Hit");
                tower.GetComponent<Tower>().towerInfo.damage += 2;
            }
            else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.earlyBeatThreshold)//early beat hit
            {
                Debug.Log("early Beat Hit");
                tower.GetComponent<Tower>().towerInfo.damage += 1;
            }
            else
            {
                Debug.Log("miss Beat Hit");

            }
            TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type);
            CombatManager.Instance.resourceNum -= tower.GetComponent<Tower>().towerInfo.resourceCost;
            SpawnBeatHitResult();
            TogglePlacementMenu();
            return;
        }
        else
        {
            unhighlightPlacementSlot();
            return;
        }    
    }

    public void TogglePlacementMenu()
    {
        towerSelectMenuOpened = !towerSelectMenuOpened;
        placementMenu.SetActive(towerSelectMenuOpened);
        unhighlightPlacementSlot();
    }

    public void HighlightPlacementSlot()
    {
        if (!towerSelectMenuOpened) return;

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            SlotW.GetComponent<SpriteRenderer>().color = Color.yellow;
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotW.GetComponent<TowerButton>().tower);
                return;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            SlotA.GetComponent<SpriteRenderer>().color = Color.yellow;
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotA.GetComponent<TowerButton>().tower);
                return;
            }

        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            SlotS.GetComponent<SpriteRenderer>().color = Color.yellow;
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotS.GetComponent<TowerButton>().tower);
                return;
            }

        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {

            SlotD.GetComponent<SpriteRenderer>().color = Color.yellow;
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotD.GetComponent<TowerButton>().tower);

                return;
            }

        }
    }

    public void unhighlightPlacementSlot()
    {
        SlotW.GetComponent<SpriteRenderer>().color = Color.white;
        SlotA.GetComponent<SpriteRenderer>().color = Color.white;
        SlotS.GetComponent<SpriteRenderer>().color = Color.white;
        SlotD.GetComponent<SpriteRenderer>().color = Color.white;
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

        targetPos = originPos + direction;

        //bounding box function
        if((targetPos.x <= -3 || targetPos.x >= 9) || (targetPos.y <= -3 || targetPos.y >= 2))
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
            Debug.Log("early Beat Hit");
        }
        else
        {
            beatResult.GetComponent<TMP_Text>().text = "miss";//miss beat
            beatResult.GetComponent<TMP_Text>().color = Color.red;
            Debug.Log("miss Beat Hit");

        }
    }
    
}
