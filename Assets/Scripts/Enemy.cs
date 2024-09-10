using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public List<Vector3> path;
    public float speed = 1;
    float timer;
    public Vector3 currentPositionHolder;
    int currentNode;

    public bool move;

    public int health = 5;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<AIPath>().path.vectorPath
        currentPositionHolder = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //if (!move) return;

        timer += Time.deltaTime * speed;
        if(gameObject.transform.position != currentPositionHolder)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, currentPositionHolder, timer);
        }
        else
        {
            if(currentNode < path.Count - 1)
            {
                currentNode++;
                CheckNode();
            }
        }
    }

    void CheckNode()
    {
        timer = 0;
        currentPositionHolder = path[currentNode];
    }

    public void Damage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
