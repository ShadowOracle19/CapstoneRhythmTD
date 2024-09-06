using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    public GameObject tower;
    
    public void PressTowerButton()
    {
        if(TowerManager.Instance.towerToPlace != null)
        {
            Destroy(TowerManager.Instance.towerToPlace);
        }

        GameObject _tower = Instantiate(tower);
        TowerManager.Instance.towerToPlace = _tower;
    }
}
