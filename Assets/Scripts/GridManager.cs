using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region dont touch this
    private static GridManager _instance;
    public static GridManager Instance
    {
        get
        {
            if (_instance is null)
            {
                Debug.LogError("grid Manager is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    //sets the width and height of the grid
    [SerializeField] private int _width, _height;

    //tile prefab
    [SerializeField] private Tile _tilePrefab;

    //grid parent where we will spawn tiles 
    [SerializeField] private Transform _gridParent;


    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
    }

    //play this at the start
    public void InitGrid()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y,0), Quaternion.identity, _gridParent);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset, x, y);

            }
        }


        Camera.main.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
