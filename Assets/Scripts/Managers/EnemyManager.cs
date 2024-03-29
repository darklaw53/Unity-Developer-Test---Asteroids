using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public Transform enemyParent;

    Camera mainCamera;
    Vector2 screenBounds;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }

    public void InstantiateUFOAtRandomEdge(GameObject UFO)
    {
        Vector3 spawnPosition = Vector3.zero;

        int edgeIndex = Random.Range(0, 2);
        switch (edgeIndex)
        {
            case 0: // Left edge
                spawnPosition = new Vector3(-screenBounds.x - UFO.transform.localScale.y / 1.5f, Random.Range(-screenBounds.y, screenBounds.y), 0f);
                break;
            case 1: // Right edge
                spawnPosition = new Vector3(screenBounds.x + UFO.transform.localScale.y / 1.5f, Random.Range(-screenBounds.y, screenBounds.y), 0f);
                break;
        }

        var x = Instantiate(UFO, spawnPosition, transform.rotation);
        x.transform.parent = enemyParent;
    }
}