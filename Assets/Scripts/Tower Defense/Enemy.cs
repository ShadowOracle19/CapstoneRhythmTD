using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public EnemyCreator enemy;

    public List<Vector3> path;
    private float speed = 1;
    float timer;
    public Vector3 currentPositionHolder;
    int currentNode;

    public bool move;
    private bool otherBeatMove = false;

    public int currentHealth;

    public UnityEvent trigger;

    [SerializeField] private Vector3 nextPosition;

    float time = 1;
    [SerializeField] private SpriteRenderer _renderer;

    public Tile tileInFront;



    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<AIPath>().path.vectorPath
        //if (path.Count == 0) gameObject.SetActive(false);
        //currentPositionHolder = path[currentNode += 1];

        currentHealth = enemy.maxHealth;

        move = true;

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
                if (tileInFront.placedTower != null)
                {
                    Clash(enemy.clashStrength);
                }
                else
                {
                    move = false;

                }
                break;

            case EnemyMovementPattern.everyOtherBeat:
                if (tileInFront.placedTower != null)
                {
                    Clash(enemy.clashStrength);
                }
                else
                {
                    otherBeatMove = !otherBeatMove;
                    if (otherBeatMove)
                    {
                        move = false;
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
                tileInFront.placedTower.GetComponent<Tower>().RemoveTower();
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
    //void Movement()
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
    //            move = false;
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

    //fixed direction movement
    public void Movement()
    {
        timer += Time.deltaTime * speed;
        if (gameObject.transform.position != nextPosition && !move)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, nextPosition, timer);
        }
        else
        {
            move = true;
            timer = 0;
            nextPosition = new Vector3(transform.position.x - 1.2f, transform.position.y);
        }
    }

    public void Damage(int damage)
    {
        _renderer.color = Color.red;
        time = 1;
        Debug.Log(gameObject.name + " damaged");
        currentHealth -= damage;
        if(currentHealth <= 0)
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
        CombatManager.Instance.enemyTotal -= 1;
        ConductorV2.instance.triggerEvent.Remove(trigger);
        Destroy(gameObject);
    }
}
