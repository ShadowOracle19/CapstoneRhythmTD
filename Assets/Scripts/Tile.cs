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

    public void Init(bool isOffset, int x, int z)
    {
        _x = x;
        _z = z;
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    private void Update()
    {

    }

    
    //mouse hovers over tile
    private void OnMouseEnter()
    {
        if (TowerManager.Instance.towerToPlace == null) return;
        _highlight.GetComponent<SpriteRenderer>().sprite = TowerManager.Instance.towerToPlace.GetComponent<SpriteRenderer>().sprite;
        _highlight.SetActive(true);
    }

    //mouse stops hovering over tile
    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
}
