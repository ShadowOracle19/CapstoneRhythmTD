using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    [SerializeField] public GameObject tower;

    public Transform parent;

    public void PressTowerButton()
    {
        if(TowerManager.Instance.towerToPlace != null)
        {
            Destroy(TowerManager.Instance.towerToPlace);
        }

        GameObject _tower = Instantiate(tower, parent);
        TowerManager.Instance.towerToPlace = _tower;
    }
}
