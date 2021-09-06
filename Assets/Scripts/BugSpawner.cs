using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSpawner : MonoBehaviour
{
    private UIController uiController;

    [SerializeField]
    private Bug bugPrefab;

    private int bugCount = 0;


    void Start ()
    {
        uiController = FindObjectOfType<UIController>();

        //if (Input.GetKey(KeyCode.Space))
        for (int spawnCnt = 0; spawnCnt < 1; spawnCnt++)
        {
            SpawnBug(new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
        }
    }

    public void SpawnBug (Vector3 atPosition)
    {
        Bug newBug = Instantiate(bugPrefab);

        newBug.transform.position = atPosition;
        newBug.ChangeDirection();

        bugCount++;
        uiController.UpdateBugCount(bugCount);
    }

    public void BugDied ()
    {
        bugCount--;
        uiController.UpdateBugCount(bugCount);
    }
}
