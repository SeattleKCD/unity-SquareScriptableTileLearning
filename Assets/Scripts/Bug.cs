using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    private MapManager mapManager;
    private BugSpawner bugSpawner;

    [SerializeField]
    private float moveTime;

    [SerializeField]
    private float lifeTime;

    [SerializeField]
    private float spawnTime;

    [SerializeField]
    private float eatTime;

    [SerializeField]
    private float eatValue;

    [SerializeField]
    private float baseSpeed;

    private float moveCounter;
    private float eatCounter;
    private float lifeCounter;
    private float spawnCounter;

    void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();
        bugSpawner = FindObjectOfType<BugSpawner>();

        moveCounter = Random.Range(0f, moveTime);
        eatCounter = Random.Range(0f, eatTime);
        lifeCounter = lifeTime;
        spawnCounter = spawnTime;
    }

    void Update()
    {
        if (0 >= lifeCounter)
        {
            Destroy(gameObject);
            return;
        }

        TileData curTileData = mapManager.GetTileData(transform.position);

        moveCounter -= Time.deltaTime;
        eatCounter -= Time.deltaTime;
        spawnCounter -= Time.deltaTime * (curTileData.foodValue / 100f);
        lifeCounter -= Time.deltaTime;

        if (0 >= spawnCounter)
        {
            spawnCounter = spawnTime;
            bugSpawner.SpawnBug(transform.position);
        }

        if (0 >= moveCounter)
        {
            moveCounter = moveTime;
            ChangeDirection();
        }

        if (0 >= eatCounter)
        {
            eatCounter = eatTime;
            mapManager.ChangeFoodValue(transform.position, -eatValue);
        }

        Vector3 newPosition;

        newPosition = transform.position + curTileData.walkingSpeed * baseSpeed * Time.deltaTime * transform.up;

        if (mapManager.AmOffMap(newPosition))
        {
            transform.Rotate(0f, 0f, 180f);
        }
        else
            transform.position = newPosition;
    }

    public void ChangeDirection()
    {
        float newRotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, newRotation);
    }
}
