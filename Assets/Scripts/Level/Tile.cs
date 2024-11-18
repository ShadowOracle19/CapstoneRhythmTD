using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    [SerializeField] public GameObject placedTower;

    public Color currentColor;
    
    float time = 1;
    //when init is called offset color based off true or false
    //todo: look into if levels arent retangular and are at different shapes
    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        currentColor = _renderer.color;
        currentColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0);
    }

    private void Update()
    {
        
        time -= Time.deltaTime;
        _renderer.color = Color.Lerp(_renderer.color, currentColor, Time.deltaTime / time);
        
    }

    public void Pulse(Color color)
    {
        _renderer.color = color;
        time = 1;
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
        //TowerManager.Instance.PlaceTower(transform.position, this);

    }
}
