using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum InstrumentType
{
    Drums, Guitar, Bass, Piano
}

public enum BuffType
{
    Fayruz, Sonu, Niimi
}

public enum TowerState
{
    Default, Recording, Repeating
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

    [Header("Record Buff Input")]
    public TowerState currentState = TowerState.Default;
    public List<BuffType> recordedBuffs = new List<BuffType>();
    public bool isInputtingBuffs = false;
    public int beatRecordingStarted = 1;
    public int buffTimer = 0;
    int buffTimerMax = 2;
    public int buffIndex = 0;
    public int buffCountMeasure = 0;
    public int buffBeatCount = 1;


    private void Start()
    {
        currentHealth = towerInfo.towerHealth;
        beat = 1;
        currentState = TowerState.Default;
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
            //beatIndicator.transform.localScale = Vector3.Lerp(Vector3.one * 1.5f, Vector3.one * 0.75f, beatDuration);

            //if (ConductorV2.instance.beatDuration < 0.2)
            //{
            //    beatIndicator.transform.localScale = Vector3.one * 1.5f;
            //}
        }
        else
        {
            beatIndicator.SetActive(false);
            beatCircle.SetActive(false);
        }
    }

    public void OnBeat()
    {
        
    }


    public void Fire() //default fire
    {
        if (!rotateStarted) return;

        //Audio SFX
        towerAttackSFX.Play();
        
        
        currentDamage = tempDamageHolder;

        if (towerInfo.isAOETower)
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
        bullet.GetComponent<Projectile>().InitializeProjectile(towerInfo.range, gameObject, currentDamage, towerInfo.projectilePiercesEnemies, attackBuffed);
        

        if(attackBuffed || FeverSystem.Instance.feverModeActive)
            bullet.GetComponent<SpriteRenderer>().color = Color.red;

        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);
        attackBuffed = false;
        
    }

    public void Fire(float yPos) //Fire on specific ypos
    {
        if (!rotateStarted) return;

        //Audio SFX
        towerAttackSFX.Play();

        
        currentDamage = tempDamageHolder;

        if (towerInfo.isAOETower)
        {
            colliders = Physics2D.BoxCastAll(transform.position, Vector2.one * towerInfo.range, 0, transform.forward);

            foreach (var item in colliders)
            {
                if (item.transform.CompareTag("StageTile"))
                {
                    item.transform.GetComponent<Tile>().Pulse(Color.red);
                }
                else if (item.transform.CompareTag("Enemy"))
                {
                    item.transform.GetComponent<Enemy>().Damage(currentDamage);
                }
            }
            colliders = null;
            return;
        }

        GameObject bullet = Instantiate(projectile, new Vector3(gameObject.transform.position.x + 1.2f, gameObject.transform.position.y + yPos), gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet.GetComponent<Projectile>().InitializeProjectile(towerInfo.range, gameObject, currentDamage, towerInfo.projectilePiercesEnemies, attackBuffed);


        if (attackBuffed || FeverSystem.Instance.feverModeActive)
            bullet.GetComponent<SpriteRenderer>().color = Color.red;

        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);
        attackBuffed = false;

    } 

    public void ExtraFire() //buff fire
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
        bullet.GetComponent<Projectile>().InitializeProjectile(towerInfo.range, gameObject, currentDamage, towerInfo.projectilePiercesEnemies, false);

        bullet.GetComponent<SpriteRenderer>().color = Color.blue;

        ConductorV2.instance.triggerEvent.Add(bullet.GetComponent<Projectile>().trigger);

        GameObject bullet2 = Instantiate(projectile, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1.2f), gameObject.transform.rotation, GameManager.Instance.projectileParent);
        bullet2.GetComponent<Projectile>().InitializeProjectile(towerInfo.range, gameObject, currentDamage, towerInfo.projectilePiercesEnemies, false);

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
        TowerManager.Instance.towers.Remove(gameObject);
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

    public void ActivateBuff(BuffType buffType)
    {
        if (GameManager.Instance.tutorialRunning && CursorTD.Instance.towerBuffSequence) //post buff sequence in tutorial
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

        RecordBuff(buffType);

        PlayBuffs(buffType);
    }

    public void PlayBuffs(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.Sonu://Sonu's Buff
                ExtraFire();
                break;
            case BuffType.Fayruz://Fayruz's Buff

                attackBuffed = true;
                break;
            case BuffType.Niimi: //Niimi's Buff
                isShielded = true;

                break;
            default:
                break;
        }
    }

    public void RecordBuff(BuffType buff) //records the buff but if a 5th buff is pressed it will remove the first buff on the list
    {
        currentState = TowerState.Recording;

        isInputtingBuffs = true;

        recordedBuffs.Add(buff);

        beatRecordingStarted = ConductorV2.instance.beatTrack;
        buffTimer = 0;

        buffIndex = 0;

        if (recordedBuffs.Count > 4)
        {
            recordedBuffs.RemoveAt(0);
        }
        
    }

    public void BuffPlayback(int _beat)
    {
        if (currentState == TowerState.Default)
        {
            return;
        }
        else if (currentState == TowerState.Recording)
        {
            buffTimer += 1;
            if(buffTimer == buffTimerMax)
            {
                currentState = TowerState.Repeating;
                buffTimer = 0;
                isInputtingBuffs = false;
            }
            return;
        }
        else if(currentState == TowerState.Repeating)
        {
            if(buffIndex > recordedBuffs.Count - 1)
            {

            }
            else
            {
                PlayBuffs(recordedBuffs[buffIndex]);

            }


            buffIndex += 1;

            buffBeatCount += 1;

            if(buffBeatCount == 4)
            {
                buffBeatCount = 0;
                buffCountMeasure += 1;
                if(buffCountMeasure == 4)
                {
                    buffCountMeasure = 0;
                    recordedBuffs.Clear();
                    currentState = TowerState.Default;
                }
            }

            if (buffIndex >= 4)
            {
                buffIndex = 0;
            }
        }

    }

}
