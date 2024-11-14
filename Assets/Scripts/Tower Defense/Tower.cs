using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InstrumentType
{
    Drums, Guitar, Bass, Piano
}

public class Tower : MonoBehaviour
{
    public TowerTypeCreator towerInfo;

    public UnityEvent trigger;

    public bool rotateStarted = false;

    public Transform firePoint;
    public GameObject projectile;

    private RaycastHit2D[] colliders;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

        
    }


    public void Fire()
    {
        if (!rotateStarted) return;
        
        if(towerInfo.isAOETower)
        {
            colliders = Physics2D.BoxCastAll(transform.position, Vector2.one * towerInfo.range, 0, transform.forward);

            foreach (var item in colliders)
            {
                if(item.transform.CompareTag("StageTile"))
                {
                    item.transform.GetComponent<Tile>().Pulse(Color.red);
                }
                else if(item.transform.CompareTag("Enemy"))
                {
                    item.transform.GetComponent<Enemy>().Damage(towerInfo.damage);
                }
            }
            colliders = null;
            return;
        }

        GameObject bullet = Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet.GetComponent<Projectile>().bulletRange = towerInfo.range;
        bullet.GetComponent<Projectile>().towerFiredFrom = gameObject;
        bullet.GetComponent<Projectile>().damage = towerInfo.damage;
        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);
        
    }

    public void RemoveTower()
    {
        ConductorV2.instance.triggerEvent.Remove(trigger);
        Destroy(gameObject);
    }

    //IEnumerator TestFire()
    //{
    //    while(true)
    //    {
    //        GameObject bullet = Instantiate(projectile, firePoint.position, Quaternion.identity);

    //        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed);
    //        yield return new WaitForSeconds(1);
    //    }
        
    //}

}
