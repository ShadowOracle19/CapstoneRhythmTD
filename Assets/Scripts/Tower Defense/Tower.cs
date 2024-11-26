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

    public int currentHealth = 0;

    public Tile connectedTile;
    public int currentDamage;
    public int tempDamageHolder;

    public bool attackBuffed = false;

    [Header("Tower Empower Indicator")]
    public bool towerHover = false;
    public GameObject beatIndicator;
    public GameObject beatCircle;

    [Header("Shield")]
    public GameObject shieldEffect;
    public bool isShielded = false;

    private void Start()
    {
        currentHealth = towerInfo.towerHealth;
    }

    private void Update()
    {
        shieldEffect.SetActive(isShielded);

        if(towerHover)
        {
            if(ConductorV2.instance.beatDuration < 0.2)
            {
                beatIndicator.transform.localScale = Vector3.one * 1.5f;
            }
            if(ConductorV2.instance.beatDuration >= 0.2)
            {
                
            }
            beatIndicator.SetActive(true);
            beatCircle.SetActive(true);
            beatIndicator.transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one * 0.75f, ConductorV2.instance.beatDuration);
        }
        else
        {
            beatIndicator.SetActive(false);
            beatCircle.SetActive(false);
        }
    }

    public void Fire()
    {
        if (!rotateStarted) return;
        
        if(attackBuffed)
        {
            currentDamage = currentDamage * 2;
        }
        else
        {
            currentDamage = tempDamageHolder;
        }


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
                    item.transform.GetComponent<Enemy>().Damage(currentDamage);
                }
            }
            colliders = null;
            return;
        }

        GameObject bullet = Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet.GetComponent<Projectile>().bulletRange = towerInfo.range;
        bullet.GetComponent<Projectile>().towerFiredFrom = gameObject;
        bullet.GetComponent<Projectile>().damage = currentDamage;

        if(attackBuffed)
            bullet.GetComponent<SpriteRenderer>().color = Color.red;

        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);
        attackBuffed = false;
        
    }
    
    public void ExtraFire()
    {
        if (!rotateStarted) return;
        
        if(towerInfo.isAOETower)
        {
            colliders = Physics2D.BoxCastAll(transform.position, Vector2.one * towerInfo.range, 0, transform.forward);

            foreach (var item in colliders)
            {
                if(item.transform.CompareTag("StageTile"))
                {
                    item.transform.GetComponent<Tile>().Pulse(Color.blue);
                }
                else if(item.transform.CompareTag("Enemy"))
                {
                    item.transform.GetComponent<Enemy>().Damage(currentDamage);
                }
            }
            colliders = null;
            return;
        }

        GameObject bullet = Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet.GetComponent<Projectile>().bulletRange = towerInfo.range;
        bullet.GetComponent<Projectile>().towerFiredFrom = gameObject;
        bullet.GetComponent<Projectile>().damage = currentDamage;
        bullet.GetComponent<SpriteRenderer>().color = Color.blue;

        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);
        attackBuffed = false;
        
    }

    public void RemoveTower()
    {
        ConductorV2.instance.triggerEvent.Remove(trigger);
        connectedTile.placedTower = null;
        Destroy(gameObject);
    }

    public void Damage(int damage)
    {
        if(isShielded)
        {
            isShielded = false;
            return;
        }
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            RemoveTower();
        }
    }

    public void ActivateBuff(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.UpArrow://Sonu's Buff
                Debug.Log("Extra Attack");
                ExtraFire();
                break;
            case KeyCode.RightArrow://Fayruz's Buff

                Debug.Log("Buff Attack");
                attackBuffed = true;
                break;
            case KeyCode.LeftArrow: //Niimi's Buff
                isShielded = true;

                break;
            default:
                break;
        }
    }

}
