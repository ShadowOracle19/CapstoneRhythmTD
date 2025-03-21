
using Pathfinding.Ionic.Zip;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        inputHandler = PlayerInputHandler.Instance;
    }
    #endregion

    private PlayerInputHandler inputHandler;

    public bool isMoving = false;
    private Vector3 originPos, targetPos;
    public float timeToMove = 1f;

    public Vector3 desiredMovement;

    //placement menu
    private bool towerSelectMenuOpened = false;
    private bool inputOnce = false;
    private bool destructMode = false;

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

    public bool towerSwap;

    [Header("Tutorial Objects")]
    public GameObject tutorialParent;

    public GameObject wasdParent;
    public GameObject wKey;
    public GameObject aKey;
    public GameObject sKey;
    public GameObject dKey;

    public GameObject arrowKeyParent;
    public GameObject upKey;
    public GameObject leftKey;
    public GameObject downKey;
    public GameObject rightKey;

    public GameObject spaceKeyParent;
    public GameObject spaceKey;

    public bool movementSequence = false;
    public bool towerPlacementMenuSequence = false;
    public bool towerPlacementMenuSequencePassed = false;
    public bool towerPlaceSequence = false ;
    public bool towerBuffSequence = false;
    public bool feverModeSequence = false;

    public int moveCounter = 0;
    public int buffCounter = 0;


    public GameObject tutorialPopupParent;
    public TextMeshProUGUI tutorialText;

    public bool beatIsHit = false;

    [Header("Resource Bar")]
    public Slider tower1Slider;
    public Image tower1ResourceSprite;

    public Slider tower2Slider;
    public Image tower2ResourceSprite;

    public Slider tower3Slider;
    public Image tower3ResourceSprite;

    public Slider tower4Slider;
    public Image tower4ResourceSprite;

    [Header("Piano resource gain")]
    public int pianoMod = 0;

    // PFX
    [SerializeField] private ParticleSystem pianoResourceGenParticles;

    private ParticleSystem pianoResourceGenParticlesInstance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseMovement || ConductorV2.instance.countingIn)
            return;

        //return cursor sprite to origin size
        cursorSprite.transform.localScale = Vector3.Lerp(cursorSprite.transform.localScale, Vector3.one, Time.deltaTime * 5);
        

        if (GameManager.Instance.winState) return;

        

        if(tile != null && tile.placedTower != null)
        {
            tile.placedTower.GetComponent<Tower>().towerHover = true;
        }

        PlacementResourceBar();

        #region tutorial
        if (GameManager.Instance.tutorialRunning) //reset color of keys
        {
            wKey.GetComponent<TextMeshPro>().color = Color.Lerp(wKey.GetComponent<TextMeshPro>().color, Color.white, Time.deltaTime);
            aKey.GetComponent<TextMeshPro>().color = Color.Lerp(aKey.GetComponent<TextMeshPro>().color, Color.white, Time.deltaTime);
            sKey.GetComponent<TextMeshPro>().color = Color.Lerp(sKey.GetComponent<TextMeshPro>().color, Color.white, Time.deltaTime);
            dKey.GetComponent<TextMeshPro>().color = Color.Lerp(dKey.GetComponent<TextMeshPro>().color, Color.white, Time.deltaTime);

            upKey.GetComponent<TextMeshPro>().color = Color.Lerp(upKey.GetComponent<TextMeshPro>().color, Color.white, Time.deltaTime);
            leftKey.GetComponent<TextMeshPro>().color = Color.Lerp(leftKey.GetComponent<TextMeshPro>().color, Color.white, Time.deltaTime);
            downKey.GetComponent<TextMeshPro>().color = Color.Lerp(downKey.GetComponent<TextMeshPro>().color, Color.white, Time.deltaTime);
            rightKey.GetComponent<TextMeshPro>().color = Color.Lerp(rightKey.GetComponent<TextMeshPro>().color, Color.white, Time.deltaTime);

            spaceKey.GetComponent<TextMeshPro>().color = Color.Lerp(spaceKey.GetComponent<TextMeshPro>().color, Color.white, Time.deltaTime);
        }
        else
        {
            tutorialParent.SetActive(false);
        }
        if(feverModeSequence)
        {
            wasdParent.SetActive(false);
            spaceKeyParent.SetActive(false);
        }
        #endregion
    }

    public void PlacementResourceBar()
    {
        tower1Slider.value = CombatManager.Instance.resourceNum;
        tower2Slider.value = CombatManager.Instance.resourceNum;
        tower3Slider.value = CombatManager.Instance.resourceNum;
        tower4Slider.value = CombatManager.Instance.resourceNum;
    }

    public void HandleFeverModeInput()
    {
        if (GameManager.Instance.tutorialRunning && !feverModeSequence)
            return;
        

        FeverSystem.Instance.ActivateFeverMode();
        
    }

    public void SwapTowers()
    {
        //towerSwap = !towerSwap;
        //if (towerSwap)
        //{
        //    SlotW.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[4];
        //    SlotW.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[4].GetComponent<Tower>().towerInfo.towerImage;

        //    SlotA.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[5];
        //    SlotA.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[5].GetComponent<Tower>().towerInfo.towerImage;


        //    SlotS.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[6];
        //    SlotS.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[6].GetComponent<Tower>().towerInfo.towerImage;


        //    SlotD.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[7];
        //    SlotD.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[7].GetComponent<Tower>().towerInfo.towerImage;
        //}
        //else
        //{
        //    SlotW.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[0];
        //    SlotW.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[0].GetComponent<Tower>().towerInfo.towerImage;

        //    SlotA.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[1];
        //    SlotA.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[1].GetComponent<Tower>().towerInfo.towerImage;


        //    SlotS.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[2];
        //    SlotS.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[2].GetComponent<Tower>().towerInfo.towerImage;


        //    SlotD.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[3];
        //    SlotD.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[3].GetComponent<Tower>().towerInfo.towerImage;
        //}

        //TowerManager.Instance.SwapTowers();
        
        
    }

    public void DestroyMode()
    {
        if(!towerSelectMenuOpened)
        {
            destructMode = !destructMode;
            if (destructMode)
            {
                cursorSprite.GetComponent<SpriteRenderer>().color = Color.red;
                if (tile != null && tile.placedTower != null && inputHandler.DestructTrigger)
                {
                    tile.placedTower.GetComponent<Tower>().RemoveTower();


                }
            }
            else
            {
                cursorSprite.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

    }

    public void DestroyTower()
    {
        if (destructMode)
        {
            if (tile != null && tile.placedTower != null)
            {
                tile.placedTower.GetComponent<Tower>().RemoveTower();


            }
        }
    }

    public void InitializeCursor()
    {
        isMoving = false;
        gameObject.transform.position = new Vector3(-2.5f, -0.54f, 0);

        SlotW.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[0];
        SlotW.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[0].GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(TowerManager.Instance.towers[0].GetComponent<Tower>(), tower1Slider, tower1ResourceSprite);


        SlotA.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[1];
        SlotA.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[1].GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(TowerManager.Instance.towers[1].GetComponent<Tower>(), tower2Slider, tower2ResourceSprite);


        SlotS.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[2];
        SlotS.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[2].GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(TowerManager.Instance.towers[2].GetComponent<Tower>(), tower3Slider, tower3ResourceSprite);


        SlotD.GetComponent<TowerButton>().tower = TowerManager.Instance.towers[3];
        SlotD.GetComponent<TowerButton>().icon.sprite = TowerManager.Instance.towers[3].GetComponent<Tower>().towerInfo.towerImage;
        TowerManager.Instance.SetResourceBarSprite(TowerManager.Instance.towers[3].GetComponent<Tower>(), tower4Slider, tower4ResourceSprite);

        pauseMovement = false;
        towerSwap = false;

        placementMenu.SetActive(false);

        if(!GameManager.Instance.tutorialRunning)
        {
            tutorialParent.SetActive(false);
            tutorialPopupParent.SetActive(false);

        }

    }

    public void MoveCursor(Vector2 direction)
    {
        if (isMoving || GameManager.Instance.winState || GameManager.Instance.loseState) return;
        
        //get the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //round the angle to 90 steps
        angle = Mathf.Round(angle / 90.0f) * 90.0f;

        //cos/sin give us x/y values 
        float horizontalOut = Mathf.Round(Mathf.Cos(angle * Mathf.Deg2Rad));
        float verticalOut = Mathf.Round(Mathf.Sin(angle * Mathf.Deg2Rad));

        direction = new Vector2(horizontalOut, verticalOut);

        direction.Normalize();

        desiredMovement = direction;

        SpawnBeatHitResult();

        if(towerSelectMenuOpened)
        {
            HighlightPlacementSlot(desiredMovement);
        }
        else
        {
            Move(desiredMovement);
        }

        
        
    }


    public void Buff1Trigger()
    {
        if (towerSelectMenuOpened) return;
        TowerEmpowerment(BuffType.Niimi);
        SpawnBeatHitResult();
    }
    public void Buff2Trigger()
    {
        if (towerSelectMenuOpened) return;
        TowerEmpowerment(BuffType.Sonu);
        SpawnBeatHitResult();
    }
    public void Buff3Trigger()
    {
        if (towerSelectMenuOpened) return;
        TowerEmpowerment(BuffType.Fayruz);
        SpawnBeatHitResult();
    }

    public void TowerEmpowerment(BuffType buff)
    {
        if(tile.placedTower != null)
        {

            switch (CheckOnBeat())
            {
                case _BeatResult.miss:
                    
                    ComboManager.Instance.ResetCombo();
                    break;
                case _BeatResult.great:
                    
                    ComboManager.Instance.IncreaseCombo();
                    tile.placedTower.GetComponent<Tower>().ActivateBuff(buff);
                    break;
                case _BeatResult.perfect:
                    
                    ComboManager.Instance.IncreaseCombo();
                    tile.placedTower.GetComponent<Tower>().ActivateBuff(buff);
                    break;
                default:
                    break;
            }
        }
        else
        {
            SpawnBeatHitResult();
            return;
        }
    }

    public void TryToPlaceTower(GameObject tower)
    {
        //checks if resource is available and if the tower is on cooldown
        if(CombatManager.Instance.resourceNum >= tower.GetComponent<Tower>().towerInfo.resourceCost 
            && !TowerManager.Instance.CheckIfOnCoolDown(tower.GetComponent<Tower>().towerInfo.type) &&
            tile != null && tile.placedTower == null) 
        {
            switch (CheckOnBeat())
            {
                case _BeatResult.late:
                    TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type, _BeatResult.miss);
                    break;
                case _BeatResult.miss:

                    ComboManager.Instance.ResetCombo();
                    TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type, _BeatResult.miss);
                    break;
                case _BeatResult.early:
                    TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type, _BeatResult.early);
                    break;
                case _BeatResult.great:

                    ComboManager.Instance.IncreaseCombo();
                    TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type, _BeatResult.perfect);
                    break;
                case _BeatResult.perfect:

                    ComboManager.Instance.IncreaseCombo();
                    TowerManager.Instance.SetTower(tower, transform.position, tile, tower.GetComponent<Tower>().towerInfo.type, _BeatResult.perfect);
                    break;
                default:
                    break;
            }

            CombatManager.Instance.resourceNum -= tower.GetComponent<Tower>().towerInfo.resourceCost;
            SpawnBeatHitResult();
            TogglePlacementMenu();
            return;
        }
        else
        {
            TogglePlacementMenu();
            return;
        }    
    }

    public void TogglePlacementMenu()
    {
        if (destructMode || GameManager.Instance.winState || GameManager.Instance.loseState) return;

        towerSelectMenuOpened = !towerSelectMenuOpened;
        placementMenu.SetActive(towerSelectMenuOpened);

        if (GameManager.Instance.tutorialRunning && towerPlacementMenuSequence && towerSelectMenuOpened)
        {
            towerPlacementMenuSequencePassed = true;
            tutorialText.text = "use Arrow Keys to select and place a tower";
            towerPlacementMenuSequence = false;
            spaceKeyParent.SetActive(false);
            towerPlaceSequence = true;
            arrowKeyParent.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, 0);
            CombatManager.Instance.towerDisplay.SetActive(true);
        }


    }

    public void HighlightPlacementSlot(Vector2 direction)
    {
        if (!towerSelectMenuOpened) return;


        if (direction == Vector2.up)
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotW.GetComponent<TowerButton>().tower);
                return;
            }
        }
        else if (direction == Vector2.left)
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotD.GetComponent<TowerButton>().tower);
                return;
            }

        }
        else if (direction == Vector2.down)
        {
            if (towerSelectMenuOpened && tile.placedTower == null)
            {
                TryToPlaceTower(SlotS.GetComponent<TowerButton>().tower);
                return;
            }

        }
        else if (direction == Vector2.right)
        {

            if (towerSelectMenuOpened && tile.placedTower == null)
            {

                TryToPlaceTower(SlotA.GetComponent<TowerButton>().tower);
                return;
            }

        }

    }


    public void Move(Vector2 direction)
    {
        if (desiredMovement == Vector3.zero || towerSelectMenuOpened || isMoving || GameManager.Instance.winState || GameManager.Instance.loseState) 
            return;
        

        StartCoroutine(MovePlayer(direction));
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

        //move onto piano tile
        if (tile != null && tile.placedTower != null && tile.placedTower.GetComponent<Tower>().towerInfo.isResourceTower)
        {
            CheckPianoResult(tile.placedTower.GetComponent<Tower>());
        }
        else
        {
            pianoMod = 0;
        }

        isMoving = false;
        desiredMovement = Vector3.zero;
        tile = null;

        if(GameManager.Instance.tutorialRunning && movementSequence)
        {
            moveCounter += 1;
            if (moveCounter == 4)
            {
                movementSequence = false;
                arrowKeyParent.SetActive(false);
                moveCounter = 0;

                tutorialPopupParent.SetActive(false);

                CombatManager.Instance.resources.SetActive(true);
                CombatManager.Instance.resourceNum = 0;
            }
        }
        
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

    //I will store all the on beat tutorial stuff here
    public void Pulse()
    {
        //Debug.Log("pulse");
        cursorSprite.transform.localScale = pulseSize;
        beatIsHit = false;

        //tutorial stuff
        //display movement tutorial
        if((movementSequence || towerPlaceSequence) && GameManager.Instance.tutorialRunning)
        {
            arrowKeyParent.SetActive(true);
            upKey.GetComponent<TextMeshPro>().color = Color.red;
            leftKey.GetComponent<TextMeshPro>().color = Color.red;
            rightKey.GetComponent<TextMeshPro>().color = Color.red;

            downKey.GetComponent<TextMeshPro>().color = Color.red;
        }
        if(towerPlacementMenuSequence && GameManager.Instance.tutorialRunning)
        {
            spaceKeyParent.SetActive(true);
            spaceKey.GetComponent<TextMeshPro>().color = Color.red;
        }
        if (towerBuffSequence && GameManager.Instance.tutorialRunning && tile != null && tile.placedTower != null)
        {
            wasdParent.SetActive(true);
            wKey.GetComponent<TextMeshPro>().color = Color.red;
            aKey.GetComponent<TextMeshPro>().color = Color.red;
            dKey.GetComponent<TextMeshPro>().color = Color.red;
        }
        if (feverModeSequence && FeverSystem.Instance.feverBarNum == 100 && GameManager.Instance.tutorialRunning)
        {
            wasdParent.SetActive(true);
            sKey.GetComponent<TextMeshPro>().color = Color.red;
        }
    }

    public void CheckPianoResult(Tower tower)
    {
        switch (CheckOnBeat())
        {
            case _BeatResult.late:
                pianoMod = 0;
                break;
            case _BeatResult.miss:
                pianoMod = 0;
                break;
            case _BeatResult.early:
                pianoMod += 1;
                //SpawnResourceGenParticles();
                break;
            case _BeatResult.great:
                pianoMod += 3;
                //SpawnResourceGenParticles();
                break;
            case _BeatResult.perfect:
                pianoMod += 5;
                //SpawnResourceGenParticles();
                break;
            default:
                break;
        }

        CombatManager.Instance.resourceNum += tower.towerInfo.resourceGain * pianoMod;

        SpawnResourceGenParticles();
    }

    public void SpawnBeatHitResult()
    {
        if (GameManager.Instance.winState || GameManager.Instance.loseState || GameManager.Instance.isGamePaused || beatIsHit) return;
        
        beatIsHit = true;
        GameObject beatResult = Instantiate(beatHitResultPrefab, new Vector3(transform.position.x, transform.position.y + 0.6f, transform.position.z), Quaternion.identity);

        switch (CheckOnBeat())
        {
            case _BeatResult.late:
                beatResult.GetComponent<TMP_Text>().text = "late";//miss beat
                beatResult.GetComponent<TMP_Text>().color = Color.grey;
                break;
            case _BeatResult.miss:
                beatResult.GetComponent<TMP_Text>().text = "miss";//miss beat
                beatResult.GetComponent<TMP_Text>().color = Color.red;
                break;
            case _BeatResult.early:
                beatResult.GetComponent<TMP_Text>().text = "early";
                beatResult.GetComponent<TMP_Text>().fontSize = 40;
                beatResult.GetComponent<TMP_Text>().color = Color.yellow;
                break;
            case _BeatResult.great:
                CombatManager.Instance.resourceNum += 1;
                beatResult.GetComponent<TMP_Text>().text = "great";
                beatResult.GetComponent<TMP_Text>().fontSize = 45;
                beatResult.GetComponent<TMP_Text>().color = Color.blue;
                break;
            case _BeatResult.perfect:
                CombatManager.Instance.resourceNum += 3;
                beatResult.GetComponent<TMP_Text>().text = "perfect";
                beatResult.GetComponent<TMP_Text>().fontSize = 50;
                beatResult.GetComponent<TMP_Text>().color = Color.green;
                break;
            default:
                CombatManager.Instance.resourceNum += 3;
                beatResult.GetComponent<TMP_Text>().text = "perfect";
                beatResult.GetComponent<TMP_Text>().fontSize = 50;
                beatResult.GetComponent<TMP_Text>().color = Color.green;
                break;
        }
    }

    public _BeatResult CheckOnBeat()
    {
        if (ConductorV2.instance.beatDuration >= ConductorV2.instance.perfectBeatThreshold)//perfect beat hit 
        {
            return _BeatResult.perfect;
            
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.greatBeatThreshold)//great beat hit
        {
            return _BeatResult.great;
        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.earlyBeatThreshold)//early beat hit
        {
            return _BeatResult.early;

        }
        else if (ConductorV2.instance.beatDuration >= ConductorV2.instance.missBeatThreshold)//miss beat hit
        {
            return _BeatResult.miss;

        }
        else if (ConductorV2.instance.beatDuration < ConductorV2.instance.missBeatThreshold)//late beat hit
        {
            return _BeatResult.late;
        }
        else
        {
            return _BeatResult.miss;
        }
    }

    private void SpawnResourceGenParticles()
    {
        pianoResourceGenParticlesInstance = Instantiate(pianoResourceGenParticles, transform.position, Quaternion.identity);
    }
    
}
