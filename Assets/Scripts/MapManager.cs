using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private List<TileData> tileDatas;

    [SerializeField]
    private TileBase stoneTile;

    [SerializeField]
    private TileBase sproutTile;

    [SerializeField]
    private TileBase decayTile;

    [SerializeField]
    private TileBase grassTile;

    [SerializeField]
    private float growTime;

    [SerializeField]
    private float growAmount;

    private Dictionary<TileBase, TileData> dataFromTiles;

    private Dictionary<Vector3Int, float> tileFoodData;

    private int minX, maxX, minY, maxY = 0;

    private float growCounter;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();
        tileFoodData = new Dictionary<Vector3Int, float>();

        growCounter = growTime;

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }

        Vector3Int gridPosition = new Vector3Int(0,0,0);
        TileBase curTile;

        gridPosition.z = 0;

        for (int scanX = -100; scanX < 101; scanX++)
        {
            for (int scanY = -100; scanY < 101; scanY++)
            {
                gridPosition.x = scanX;
                gridPosition.y = scanY;
                curTile = map.GetTile(gridPosition);
                if (null != curTile)
                    addTileFoodValue(gridPosition, dataFromTiles[curTile].foodValue);
            }
        }

        //print("minX = " + minX + ", maxX = " + maxX + ", minY = " + minY + ", maxY = " + maxY);

    }

    public void addTileFoodValue(Vector3Int gridPosition, float foodValue)
    {
        if (gridPosition.x < minX)
            minX = gridPosition.x;

        if (gridPosition.x > maxX)
            maxX = gridPosition.x;
 
        if (gridPosition.y < minY)
            minY = gridPosition.y;

        if (gridPosition.y > maxY)
            maxY = gridPosition.y;

        tileFoodData.Add(gridPosition, foodValue);
    }

    private void Update()
    {
        Vector3Int gridPosition = new Vector3Int(0, 0, 0);
        TileBase curTile;
        float newFoodValue;

        gridPosition.z = 0;

        growCounter -= Time.deltaTime;

        if (0 >= growCounter)
        {
            growCounter = growTime;

            for (int scanX = minX; scanX <= maxX; scanX++)
            {
                for (int scanY = minY; scanY <= maxY; scanY++)
                {
                    gridPosition.x = scanX;
                    gridPosition.y = scanY;
                    curTile = map.GetTile(gridPosition);

                    if (null != curTile)
                    {
                        newFoodValue = tileFoodData[gridPosition];

                        if (newFoodValue > 25f && newFoodValue < 100f)
                        {
                            newFoodValue += growAmount;
                            tileFoodData[gridPosition] = newFoodValue;
                            //print("Tile at " + gridPosition + " grew to foodValue " + newFoodValue);

                            updateTile(gridPosition, newFoodValue);
                        }
                    }
                }
            }
        }
    
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gridPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gridPosition);

            //float walkingSpeed = dataFromTiles[clickedTile].walkingSpeed;
            //float foodValue = tileFoodData[gridPosition];

            //print("World position " + mousePosition + ", Cell position " + gridPosition);
            //print("Walking speed on " + clickedTile + " is " + walkingSpeed + ", Food Value is " + foodValue);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public TileData GetTileData(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);

        TileBase tile = map.GetTile(gridPosition);

        if (null == tile)
        {
            return null;
        }
        else
        {
            return dataFromTiles[tile];
        }
    }

    public void ChangeFoodValue(Vector2 worldPosition, float changeBy)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);

        float newFoodValue = tileFoodData[gridPosition] + changeBy;

        //print(" tileFoodValue = " + tileFoodData[gridPosition] + ", changeBy = " + changeBy + ", newFoodValue = " + newFoodValue);

        updateTile(gridPosition, newFoodValue);

        tileFoodData[gridPosition] = Mathf.Clamp(newFoodValue, 0f, 100f);
    }

    private void updateTile(Vector3Int gridPosition, float foodValue)
    {
        if (foodValue < 25f)
            map.SetTile(gridPosition, stoneTile);
        else if (foodValue >= 25f && foodValue < 50f)
            map.SetTile(gridPosition, sproutTile);
        else if (foodValue >= 50f && foodValue < 75f)
            map.SetTile(gridPosition, decayTile);
        else
            map.SetTile(gridPosition, grassTile);
    }

    public bool AmOffMap(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);

        TileBase tile = map.GetTile(gridPosition);

        bool offMap = false;

        if (null == tile)
            offMap = true;

        return offMap;
    }
}
