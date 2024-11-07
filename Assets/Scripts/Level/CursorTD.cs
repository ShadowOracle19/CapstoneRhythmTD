using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CursorTD : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        if(CombatManager.Instance.resourceNum >= tower.GetComponent<Tower>().resourceCost)
        {
            if (ConductorV2.instance.beatDuration >= ConductorV2.instance.perfectBeatThreshold)//perfect beat hit 
            {
                Debug.Log("perfect Beat Hit");
                tower.GetComponent<Tower>().damage += 2;
            }
            else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.earlyBeatThreshold)//early beat hit
            {
                Debug.Log("early Beat Hit");
                tower.GetComponent<Tower>().damage += 1;
            }
            else
            {
                Debug.Log("miss Beat Hit");

            }
            TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().instrumentType);
            CombatManager.Instance.resourceNum -= tower.GetComponent<Tower>().resourceCost;
            SpawnBeatHitResult();
            TogglePlacementMenu();
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
        if((targetPos.x <= -3 || targetPos.x >= 9) || (targetPos.y <= -2 || targetPos.y >= 2))
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
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.earlyBeatThreshold)//early beat hit
        {
            beatResult.GetComponent<TMP_Text>().text = "early";
            Debug.Log("early Beat Hit");
        }
        else
        {
            beatResult.GetComponent<TMP_Text>().text = "miss";//miss beat
            Debug.Log("miss Beat Hit");

        }
    }
    
}
