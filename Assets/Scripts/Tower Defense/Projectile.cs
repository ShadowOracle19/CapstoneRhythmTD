using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public float speed = 1;
    float timer;
    public bool canMove = false;
    Vector3 nextPosition;
    public int bulletRange = 0;
    int activeTime;
    public bool piercing;

    public int damage = 1;

    public GameObject towerFiredFrom;

    public UnityEvent trigger;

    public bool burningBullet = false;

    public void InitializeProjectile(int range, GameObject firedFrom, int _damage, bool isPiercing, bool isBurning)
    {
        bulletRange = range;
        towerFiredFrom = firedFrom;
        damage = _damage;
        piercing = isPiercing;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = new Vector3(transform.position.x + 1.2f, transform.position.y);
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
            nextPosition = new Vector3(transform.position.x + 1.2f, transform.position.y);
            canMove = false;
        }
        
    }

    public void OnTick()
    {
        canMove = true;
        activeTime += 1;
        if (activeTime == bulletRange)
        {
            RemoveProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().Damage(damage);
            collision.GetComponent<Enemy>().burnDamage += 4;
            collision.GetComponent<Enemy>().burnt = true;

            if(!piercing) RemoveProjectile();

        }
    }
    public void RemoveProjectile()
    {
        ConductorV2.instance.triggerEvent.Remove(trigger);
        Destroy(gameObject);
    }
}
