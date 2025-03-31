using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor;
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] public GameObject placedTower;

    
    float time = 1;
    //when init is called offset color based off true or false
    //todo: look into if levels arent retangular and are at different shapes

    //to be changed to using beats once it's been proven to work
    public float forecastEndsIn = 4;
    //don't touch this bit below, this is fine
    public bool forecastingActive = false;

    private void Update()
    {
        
        time -= Time.deltaTime;
        _renderer.color = Color.Lerp(_renderer.color, _baseColor, Time.deltaTime / time);
        
    }

    public void Pulse(Color color)
    {
        _renderer.color = color;
        time = 1;
    }
    
}
