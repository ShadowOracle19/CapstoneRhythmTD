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

    bool squareInit = false;
    int squareTick = 0;


    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
    }

    //play this at the start
    public void InitGrid()
    {
        //GenerateGrid();
        float y = 0;
        float x = 0;

        //at start iterate through tiles to allow offset color then after set width change the offset so they make checkered pattern
        foreach(Transform child in gridParent)
        {
            if(y != child.position.y)
            {
                squareInit = !squareInit;
                y = child.position.y;
            }
            squareInit = !squareInit;
            child.GetComponent<Tile>().Init(squareInit);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
