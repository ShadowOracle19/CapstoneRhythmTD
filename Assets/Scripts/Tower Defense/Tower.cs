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
    public AudioSource towerAttackSFX;
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
    public bool towerAboutToFire = false;

    [Header("Shield")]
    public GameObject shieldEffect;
    public bool isShielded = false;

    public int beat;
    public bool everyOtherBeat = false;

    private void Start()
    {
        currentHealth = towerInfo.towerHealth;
        beat = 1;
    }

    private void Update()
    {
        if(FeverSystem.Instance.feverModeActive)
            isShielded = true;

        shieldEffect.SetActive(isShielded);

        towerEffectVisual();
    }

    public void towerEffectVisual()
    {
        float beatDuration = ConductorV2.instance.beatDuration;
        if (towerHover && towerAboutToFire)
        {
            beatIndicator.SetActive(true);
            beatCircle.SetActive(true);
            beatIndicator.transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one * 0.75f, beatDuration);

            if (ConductorV2.instance.beatDuration < 0.2)
            {
                beatIndicator.transform.localScale = Vector3.one * 1.5f;
            }
        }
        else
        {
            beatIndicator.SetActive(false);
            beatCircle.SetActive(false);
        }
    }

    public void OnBeat()
    {
        //switch (towerInfo.attackPattern)
        //{
        //    case TowerAttackPattern.everyBeat:
        //        towerAboutToFire = true;
        //        Fire();
        //        break;
        //    case TowerAttackPattern.everyMeasure:
        //        beat += 1;
        //        if(beat == 4)
        //        {
        //            Fire();
        //            beat = 1;
        //            towerAboutToFire = false;
        //        }
        //        else if(beat == 3)
        //        {
        //            towerAboutToFire = true;
        //        }
        //        break;
        //    case TowerAttackPattern.everyOtherBeat:
        //        everyOtherBeat = !everyOtherBeat;
        //        towerAboutToFire = !everyOtherBeat; 
        //        if(everyOtherBeat)
        //        {
        //            Fire();
        //        }
        //        break;
        //    case TowerAttackPattern.everyBeatButOne:
        //        beat += 1;
        //        if (beat < 4)
        //        {
        //            towerAboutToFire = true;
        //            Fire();
                    
        //        }
        //        else if(beat == 4)
        //        {
        //            towerAboutToFire = false;
        //            beat = 1;
        //        }
        //        break;
        //    default:
        //        break;
        //}
    }


    public void Fire()
    {
        if (!rotateStarted) return;

        //Audio SFX
        towerAttackSFX.Play();
        
        if(attackBuffed || FeverSystem.Instance.feverModeActive)
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

        if(attackBuffed || FeverSystem.Instance.feverModeActive)
            bullet.GetComponent<SpriteRenderer>().color = Color.red;

        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);
        attackBuffed = false;
        
    }
    
    public void ExtraFire()
    {
        if (!rotateStarted) return;
        
        if(towerInfo.isAOETower)
        {
            colliders = Physics2D.BoxCastAll(transform.position, Vector2.one * towerInfo.range * 2, 0, transform.forward);

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

        GameObject bullet = Instantiate(projectile, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.2f), gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet.GetComponent<Projectile>().bulletRange = towerInfo.range;
        bullet.GetComponent<Projectile>().towerFiredFrom = gameObject;
        bullet.GetComponent<Projectile>().damage = currentDamage;
        bullet.GetComponent<SpriteRenderer>().color = Color.blue;

        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);

        GameObject bullet2 = Instantiate(projectile, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1.2f), gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet2.GetComponent<Projectile>().bulletRange = towerInfo.range;
        bullet2.GetComponent<Projectile>().towerFiredFrom = gameObject;
        bullet2.GetComponent<Projectile>().damage = currentDamage;
        bullet2.GetComponent<SpriteRenderer>().color = Color.blue;

        ConductorV2.instance.triggerEvent.Add(bullet2.GetComponent<Projectile>().trigger);
        attackBuffed = false;
        
    }

    public void RemoveTower()
    {
        switch (towerInfo.type)
        {
            case InstrumentType.Drums:
                ConductorV2.instance.drums.volume -= 0.05f;
                ConductorV2.instance.drums.volume = Mathf.Clamp(ConductorV2.instance.drums.volume, 0, 0.5f);
                break;

            case InstrumentType.Guitar:
                ConductorV2.instance.guitarH.volume -= 0.05f;
                ConductorV2.instance.guitarM.volume -= 0.05f;
                ConductorV2.instance.guitarH.volume = Mathf.Clamp(ConductorV2.instance.guitarH.volume, 0, 0.5f);
                ConductorV2.instance.guitarM.volume = Mathf.Clamp(ConductorV2.instance.guitarM.volume, 0, 0.5f);
                break;

            case InstrumentType.Bass:
                ConductorV2.instance.bass.volume -= 0.05f;
                ConductorV2.instance.bass.volume = Mathf.Clamp(ConductorV2.instance.bass.volume, 0, 0.5f);
                break;

            case InstrumentType.Piano:
                ConductorV2.instance.piano.volume -= 0.05f;
                ConductorV2.instance.piano.volume = Mathf.Clamp(ConductorV2.instance.piano.volume, 0, 0.5f);
                break;

            default:
                break;
        }
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
        if (GameManager.Instance.tutorialRunning && CursorTD.Instance.towerBuffSequence)
        {
            CursorTD.Instance.buffCounter += 1;
            if(CursorTD.Instance.buffCounter == 4)
            {
                CursorTD.Instance.tutorialText.text = "Now place more towers and buff them to the beat and grow that fever bar!";

                CursorTD.Instance.towerBuffSequence = false;
                CombatManager.Instance.healthBar.SetActive(true);
                CombatManager.Instance.feverBar.SetActive(true);
                CombatManager.Instance.combo.SetActive(true);
                CombatManager.Instance.controls.SetActive(true);

                CursorTD.Instance.arrowKeyParent.SetActive(false);
                CursorTD.Instance.feverModeSequence = true;
                FeverSystem.Instance.feverBarNum = 50;

                EnemySpawner.Instance.ForceEnemySpawn(CursorTD.Instance.gameObject.transform.position.y, EnemyType.Walker);
                CursorTD.Instance.buffCounter = 0;
            }
        }
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
