using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 1;
    float timer;
    public bool canMove = false;
    Vector3 nextPosition;
    public int bulletRange = 0;
    int activeTime;

    public Intervals interval;
    // Start is called before the first frame update
    void Start()
    {
        nextPosition = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!canMove) return;
        //transform.Translate(transform.right * 20 * Time.deltaTime);
        timer += Time.deltaTime * speed;
        if(gameObject.transform.position != nextPosition)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, nextPosition, timer);
        }
        else
        {
            nextPosition = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            canMove = false;
        }
        
    }

    public void OnTick()
    {
        canMove = true;
        activeTime += 1;
        if (activeTime == bulletRange)
        {
            Conductor.Instance._intervals.Remove(interval);
            Destroy(gameObject);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Damage(1);
        }
    }
}
