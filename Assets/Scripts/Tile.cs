using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public int _x, _z;
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    [SerializeField] public GameObject placedTower;

    public void Init(bool isOffset, int x, int z)
    {
        _x = x;
        _z = z;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    private void Update()
    {
        //if tower is no longer being held hide highlight
        if (TowerManager.Instance.towerToPlace == null)
        {
            _highlight.SetActive(false);
            TowerManager.Instance.isTowerHovering = false;
        }
    }
    
    //mouse hovers over tile
    private void OnMouseEnter()
    {
        //to make sure mouse enter doesnt work unless a tower is being held
        if (TowerManager.Instance.towerToPlace == null) return;

        _highlight.GetComponent<SpriteRenderer>().sprite = TowerManager.Instance.towerToPlace.GetComponent<SpriteRenderer>().sprite;
        TowerManager.Instance.isTowerHovering = true;

        //dont activate highlight if a tower is already on tile
        if (placedTower != null) return;
        _highlight.SetActive(true);
    }

    //mouse stops hovering over tile
    private void OnMouseExit()
    {
        TowerManager.Instance.isTowerHovering = false;
        _highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (TowerManager.Instance.towerToPlace == null || placedTower != null) return;

        _highlight.SetActive(false);
        TowerManager.Instance.PlaceTower(transform.position, this);

    }
}
