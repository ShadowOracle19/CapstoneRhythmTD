using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public EnemyCreator enemy;
    public AudioSource enemyDeathSFX;

    public List<Vector3> path;
    private float speed = 1;
    float timer;
    public Vector3 currentPositionHolder;

    public bool dontMove;
    private bool otherBeatMove = false;
    public int barTracker = 0;

    public int currentHealth;

    public UnityEvent trigger;

    [SerializeField] private Vector3 nextPosition;

    float time = 1;
    [SerializeField] private SpriteRenderer _renderer;

    public Tile tileInFront;

    bool playOnce = false;




    // Start is called before the first frame update
    void Start()
    {
        currentHealth = enemy.maxHealth;

        dontMove = true;

        nextPosition = new Vector3(transform.position.x - 1, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime * 5;
        _renderer.color = Color.Lerp(_renderer.color, Color.white, Time.deltaTime / time);

        

        Movement();

    }

    public void OnTick()
    {
        switch (enemy.movementPattern)
        {
            case EnemyMovementPattern.everyBeat:
                dontMove = false;
                break;

            case EnemyMovementPattern.everyOtherBeat:
                otherBeatMove = !otherBeatMove;
                if (otherBeatMove)
                {
                    dontMove = false;
                }
                break;

            case EnemyMovementPattern.twiceEveryBeat:
                nextPosition = new Vector3(transform.position.x - 2.4f, transform.position.y);
                dontMove = false;
                break;

            case EnemyMovementPattern.moveThenCast:
                if(ConductorV2.instance.beatTrack == 2)
                {
                   dontMove = false;
                }
                else if (ConductorV2.instance.beatTrack == 3)
                {
                    enemy.effect.UseEffect();
                }
                break;

            case EnemyMovementPattern.dontMove:
               dontMove = true;
                break;

            case EnemyMovementPattern.everyTwoBeats:
                if (ConductorV2.instance.beatTrack == 2 || ConductorV2.instance.beatTrack == 4)
                {
                    dontMove = false;
                }
                break;

            case EnemyMovementPattern.oncePerBar:
                if (ConductorV2.instance.beatTrack == 4)
                {
                    dontMove = false;
                }
                break;

            case EnemyMovementPattern.onceEveryTwoBars:
                if(ConductorV2.instance.beatTrack == 4)
                {
                    barTracker += 1;
                    if(barTracker == 2)
                    {
                        dontMove = false;
                        barTracker = 0;
                    }
                }
                break;

            default:
                break;
        }

    }


    public void Clash(ClashStrength clashStrength)
    {
        switch (clashStrength)
        {
            case ClashStrength.Weak:
                tileInFront.placedTower.GetComponent<Tower>().Damage(1);
                Kill();
                break;
            case ClashStrength.Medium:
                tileInFront.placedTower.GetComponent<Tower>().Damage(tileInFront.placedTower.GetComponent<Tower>().towerInfo.towerHealth);
                Kill();
                break;
            case ClashStrength.High:
                break;
            case ClashStrength.Immune:
                break;
            default:
                break;
        }
    }

    #region pathing
    //Pathing Function
    //voiddontMovement()
    //{
    //    timer += Time.deltaTime * speed;
    //    if (gameObject.transform.position != currentPositionHolder)
    //    {
    //        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, currentPositionHolder, timer);
    //    }
    //    else
    //    {
    //        if (currentNode < path.Count - 1)
    //        {
    //           dontMove = false;
    //            currentNode++;
    //            CheckNode();
    //        }
    //    }
    //}

    //void CheckNode()
    //{
    //    timer = 0;
    //    currentPositionHolder = path[currentNode];
    //}
    #endregion

    //fixed directiondontMovement
    public void Movement()
    {
        if (tileInFront != null && tileInFront.placedTower != null)
        {
            Clash(enemy.clashStrength);
            dontMove = true;
            return;
        }
        timer += Time.deltaTime * speed;
        if (gameObject.transform.position != nextPosition && !dontMove)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, nextPosition, timer);
        }
        else
        {
           dontMove = true;
            timer = 0;
            nextPosition = new Vector3(transform.position.x - 1.2f, transform.position.y);
        }
    }

    public void Damage(int damage)
    {
        _renderer.color = Color.red;
        time = 1;
        currentHealth -= damage;
        enemyDeathSFX.Play();
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        RemoveEnemy();
    }


    public void RemoveEnemy()
    {
        if (playOnce) return;
        playOnce = true;
        CombatManager.Instance.enemyTotal -= 1;
        ConductorV2.instance.triggerEvent.Remove(trigger);
        Destroy(gameObject);
    }
}
