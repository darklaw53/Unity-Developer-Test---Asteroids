using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public ShipLayoutSO shipLayoutSO;
    public int lives = 3;

    public GameObject bigUFO, smallUFO;
    public GameObject extraLivesIcon;
    public Transform extraLivesHolder;
    public TextMesh scoreText;
    public int score = 0;

    public int asteroidsPertLevel = 5;
    public int UFOsPerLevel = 2;
    
    int currentLevel;
    int gainedPoints = 0;
    List<GameObject> UFOList = new List<GameObject>();
    List<GameObject> extraLivesIcons = new List<GameObject>();

    void Start()
    {
        AsteroidManager.Instance.InstantiatePrefabAtRandomEdge(asteroidsPertLevel);
        for (int i = 0; i < lives; i++)
        {
            GameObject y = Instantiate(extraLivesIcon, extraLivesHolder);
            extraLivesIcons.Add(y);
        }

        int x = GetBigUFOForLevel();
        for (int i = 0; i < UFOsPerLevel; i++)
        {
            if (i < x) UFOList.Add(bigUFO);
            else UFOList.Add(smallUFO);
        }

        AsteroidManager.Instance.asteroidThreshold = GetAsteroidhreshold();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (CharacterControllerShip.Instance != null)
        {
            CharacterControllerShip.Instance._rightThrusterB = shipLayoutSO._rightThrusterB;
            CharacterControllerShip.Instance._rightStrafeThrusterB = shipLayoutSO._rightStrafeThrusterB;
            CharacterControllerShip.Instance._retroThrusterB = shipLayoutSO._retroThrusterB;
            CharacterControllerShip.Instance._retroThruster2B = shipLayoutSO._retroThruster2B;
            CharacterControllerShip.Instance._leftStrafeThrusterB = shipLayoutSO._leftStrafeThrusterB;
            CharacterControllerShip.Instance._leftThrusterB = shipLayoutSO._leftThrusterB;
            CharacterControllerShip.Instance._middleThrusterB = shipLayoutSO._middleThrusterB;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            var x = CharacterControllerShip.Instance.GetComponent<BoxCollider2D>();
            x.offset = shipLayoutSO.hitBoxOffset;
            x.size = shipLayoutSO.hitBoxSize;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1) scoreText = 
                GameObject.FindWithTag("scoreText").GetComponent<TextMesh>();
    }

    public void GainPoints(int points)
    {
        score += points;
        if (scoreText != null) scoreText.text = "" + score;

        gainedPoints += points;
        if (gainedPoints >= 10000)
        {
            gainedPoints -= 10000;
            lives++;
            GameObject y = Instantiate(extraLivesIcon, extraLivesHolder);
            extraLivesIcons.Add(y);
        }
    }

    public void SpawnUFO()
    {
        if (UFOList.Count > 0)
        {
            int randomIndex = Random.Range(0, UFOList.Count);
            EnemyManager.Instance.InstantiateUFOAtRandomEdge(UFOList[randomIndex]);
            UFOList.RemoveAt(randomIndex);
        }
    }

    private int GetAsteroidhreshold()
    {
        return Mathf.CeilToInt(asteroidsPertLevel * 7) / (UFOsPerLevel + 1);
    }

    private int GetBigUFOForLevel()
    {
        float ratio = 0.7f - currentLevel * 0.1f;
        if (ratio < 0) ratio = 0;
        return Mathf.CeilToInt(UFOsPerLevel * ratio);
    }

    public void GameOver()
    {

    }

    public void LooseALife()
    {
        if (extraLivesIcons.Count > 0)
        {
            var x = extraLivesIcons[extraLivesIcons.Count - 1];
            extraLivesIcons.Remove(x);
            Destroy(x);
        }
    }

    //triggered when player looses a life
    public IEnumerator Exploded()
    {
        yield return new WaitForSeconds(2);

        CharacterControllerShip.Instance.transform.position = Vector3.zero;
        CharacterControllerShip.Instance.transform.rotation = transform.rotation;
        CharacterControllerShip.Instance.gameObject.SetActive(true);
    }

    public IEnumerator WarpJump()
    {
        yield return new WaitForSeconds(1);
        Vector2 randomPoint = Camera.main.ScreenToWorldPoint
            (new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0));
        CharacterControllerShip.Instance.transform.position = randomPoint;
        CharacterControllerShip.Instance.gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        currentLevel++;
        asteroidsPertLevel++;
        UFOsPerLevel++;

        AsteroidManager.Instance.InstantiatePrefabAtRandomEdge(asteroidsPertLevel);

        UFOList.Clear();
        int x = GetBigUFOForLevel();
        for (int i = 0; i < UFOsPerLevel; i++)
        {
            if (i < x) UFOList.Add(bigUFO);
            else UFOList.Add(smallUFO);
        }
    }
}