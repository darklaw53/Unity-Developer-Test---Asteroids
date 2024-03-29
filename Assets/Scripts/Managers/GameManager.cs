using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public ShipLayoutSO shipLayoutSO;
    public int lives = 3;

    public GameObject bigUFO, smallUFO;
    public int score;

    public List<int> asteroidsPerLevel;
    public List<int> UFOsPerLevel;
    
    int currentLevel;
    List<GameObject> UFOList = new List<GameObject>();

    void Start()
    {
        AsteroidManager.Instance.InstantiatePrefabAtRandomEdge(asteroidsPerLevel[currentLevel]);

        int x = GetBigUFOForLevel();
        for (int i = 0; i < UFOsPerLevel[currentLevel]; i++)
        {
            if (i < x) UFOList.Add(bigUFO);
            else UFOList.Add(smallUFO);
        }

        AsteroidManager.Instance.asteroidThreshold = GetAsteroidhreshold();
    }

    public void SpawnUFO()
    {
        int randomIndex = Random.Range(0, UFOList.Count);
        EnemyManager.Instance.InstantiateUFOAtRandomEdge(UFOList[randomIndex]);
        UFOList.RemoveAt(randomIndex);
    }

    private int GetAsteroidhreshold()
    {
        return Mathf.CeilToInt(asteroidsPerLevel[currentLevel] * 7) / (UFOsPerLevel[currentLevel] + 1);
    }

    private int GetBigUFOForLevel()
    {
        float ratio = 0.9f - currentLevel * 0.1f;
        if (ratio < 0) ratio = 0;
        return Mathf.CeilToInt(UFOsPerLevel[currentLevel] * ratio);
    }

    public void GameOver()
    {

    }

    //triggered when player looses a life
    public IEnumerator Exploded()
    {   
        yield return new WaitForSeconds(2);

        CharacterControllerShip.Instance.transform.position = Vector3.zero;
        CharacterControllerShip.Instance.transform.rotation = transform.rotation;
        CharacterControllerShip.Instance.gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        currentLevel++;

        AsteroidManager.Instance.InstantiatePrefabAtRandomEdge(asteroidsPerLevel[currentLevel]);

        UFOList.Clear();
        int x = GetBigUFOForLevel();
        for (int i = 0; i < UFOsPerLevel[currentLevel]; i++)
        {
            if (i < x) UFOList.Add(bigUFO);
            else UFOList.Add(smallUFO);
        }
    }
}