using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : Singleton<AsteroidManager>
{
    public List<GameObject> asteroids;
    public Transform asteroidParent;

    int numberOfActiveAsteroids;
    Camera mainCamera;
    Vector2 screenBounds;

    private void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }

    public void InstantiatePrefabAtRandomEdge(int numberOfRepeats)
    {
        for (int i = 0; i < numberOfRepeats; i++)
        {
            Vector3 spawnPosition = Vector3.zero;

            int prefabIndex = Random.Range(0, asteroids.Count);
            GameObject prefabToInstantiate = asteroids[prefabIndex];

            int edgeIndex = Random.Range(0, 4);
            switch (edgeIndex)
            {
                case 0: // Top edge
                    spawnPosition = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y + prefabToInstantiate.transform.localScale.y / 2, 0f);
                    break;
                case 1: // Bottom edge
                    spawnPosition = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), -screenBounds.y - prefabToInstantiate.transform.localScale.y / 2, 0f);
                    break;
                case 2: // Left edge
                    spawnPosition = new Vector3(-screenBounds.x - prefabToInstantiate.transform.localScale.y / 2, Random.Range(-screenBounds.y, screenBounds.y), 0f);
                    break;
                case 3: // Right edge
                    spawnPosition = new Vector3(screenBounds.x + prefabToInstantiate.transform.localScale.y / 2, Random.Range(-screenBounds.y, screenBounds.y), 0f);
                    break;
            }

            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            Instantiate(prefabToInstantiate, spawnPosition, randomRotation, asteroidParent);
            numberOfActiveAsteroids += 7;
        }
    }

    public void AsteroidDestroyed()
    {
        numberOfActiveAsteroids--;

        if (numberOfActiveAsteroids <= 0) GameManager.Instance.NextLevel();
    }
}