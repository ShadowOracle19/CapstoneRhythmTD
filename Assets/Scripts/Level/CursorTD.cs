using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            TogglePlacementMenu();
        }

        //on press log which direction needs to be moved
        if(Input.GetKeyUp(KeyCode.W) && !isMoving)
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TowerManager.Instance.SetTower(SlotW.GetComponent<TowerButton>().tower, transform.position, tile);
                SlotW.GetComponent<SpriteRenderer>().color = Color.white;
                TogglePlacementMenu();
                return;
            }
            desiredMovement = Vector3.up;
        }
        else if (Input.GetKeyUp(KeyCode.A) && !isMoving)
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TowerManager.Instance.SetTower(SlotA.GetComponent<TowerButton>().tower, transform.position, tile);
                SlotA.GetComponent<SpriteRenderer>().color = Color.white;
                TogglePlacementMenu();
                return;
            }
            desiredMovement = Vector3.left;
        }
        else if (Input.GetKeyUp(KeyCode.S) && !isMoving)
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TowerManager.Instance.SetTower(SlotS.GetComponent<TowerButton>().tower, transform.position, tile);
                SlotS.GetComponent<SpriteRenderer>().color = Color.white;
                TogglePlacementMenu();
                return;
            }
            desiredMovement = Vector3.down;
        }
        else if (Input.GetKeyUp(KeyCode.D) && !isMoving)
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TowerManager.Instance.SetTower(SlotD.GetComponent<TowerButton>().tower, transform.position, tile);
                SlotD.GetComponent<SpriteRenderer>().color = Color.white;
                TogglePlacementMenu();
                return;
            }
            desiredMovement = Vector3.right;
        }

        HighlightPlacementSlot();
        
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            SlotW.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SlotA.GetComponent<SpriteRenderer>().color = Color.yellow;

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SlotS.GetComponent<SpriteRenderer>().color = Color.yellow;

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SlotD.GetComponent<SpriteRenderer>().color = Color.yellow;

        }
    }

    public void unhighlightPlacementSlot()
    {
        SlotW.GetComponent<SpriteRenderer>().color = Color.white;
        SlotA.GetComponent<SpriteRenderer>().color = Color.white;
        SlotS.GetComponent<SpriteRenderer>().color = Color.white;
        SlotD.GetComponent<SpriteRenderer>().color = Color.white;
    }

    //plays every beat
    public void Move()
    {
        if (desiredMovement == Vector3.zero || towerSelectMenuOpened) return;
        StartCoroutine(MovePlayer(desiredMovement));
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        originPos = transform.position;

        targetPos = originPos + direction;

        //bounding box function
        if((targetPos.x <= -6 || targetPos.x >= 8) || (targetPos.y <= -2 || targetPos.y >= 1))
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

        yield return null;
    }

    //check which tile cursor is on
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("StageTile"))
        {
            tile = collision.gameObject.GetComponent<Tile>();
        }
    }
}