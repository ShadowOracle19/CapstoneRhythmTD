using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public List<Vector3> path;
    public float speed = 1;
    float timer;
    public Vector3 currentPositionHolder;
    int currentNode;

    public bool move;

    public int health = 5;

    public UnityEvent trigger;

    [SerializeField] private Vector3 nextPosition;

    float time = 1;
    [SerializeField] private SpriteRenderer _renderer;

    float moveCounter = 0;
    public float moveOnBeat = 4;

    public bool firstSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<AIPath>().path.vectorPath
        //if (path.Count == 0) gameObject.SetActive(false);
        //currentPositionHolder = path[currentNode += 1];
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
        moveCounter += 1;
        if(moveCounter == 4)
        {
            move = false;
            moveCounter = 0;
        }
        //Debug.Log("Enemy Movement");
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
            nextPosition = new Vector3(transform.position.x - 1, transform.position.y);
        }
    }

    public void Damage(int damage)
    {
        _renderer.color = Color.red;
        time = 1;
        Debug.Log(gameObject.name + " damaged");
        health -= damage;
        if(health <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        CombatManager.Instance.enemyTotal -= 1;
        RemoveEnemy();
    }


    public void RemoveEnemy()
    {
        ConductorV2.instance.triggerEvent.Remove(trigger);
        Destroy(gameObject);
    }
}
