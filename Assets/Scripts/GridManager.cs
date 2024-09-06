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
    public int width, height;

    //tile prefab
    public Tile tilePrefab;

    //grid parent where we will spawn tiles 
    public Transform gridParent;


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

    //generate a grid based off width and height
    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y,0), Quaternion.identity, gridParent);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset, x, y);

            }
        }

        //adjust camera to the middle of the grid
        Camera.main.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
