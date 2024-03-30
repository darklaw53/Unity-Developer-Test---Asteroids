using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [Header("Scriptable Objects")]
    public ShipLayoutSO shipLayoutSO;
    public ScoreBoardSO scoreBoardSO;

    [Header("Objects")]
    public GameObject defaultShip;
    public GameObject bigUFO;
    public GameObject smallUFO;
    public GameObject extraLivesIcon;
    public Transform extraLivesHolder;
    public TextMeshProUGUI scoreText;

    [Header("Game Parameters")]
    public int lives = 3;
    public int score = 0;
    public int asteroidsPerLevel = 5;
    public int UFOsPerLevel = 2;
    public int objectsInCenter;
    public float maxRespawnTime = 5f;

    [Header("Audio")]
    public AudioSource backgroundMusic;
    public AudioClip gameOverSound;

    // Internal Variables
    int gainedPoints = 0;
    bool waitingToRespawn;
    bool canGainPoints = true;
    [HideInInspector] public int currentLevel;
    [HideInInspector] public List<GameObject> UFOList = new List<GameObject>();
    List<GameObject> extraLivesIcons = new List<GameObject>();

    void Start()
    {
        if (shipLayoutSO.prefab != null) shipLayoutSO.prefab = defaultShip;
        if (SceneManager.GetActiveScene().buildIndex == 1) Instantiate(shipLayoutSO.prefab);

        AsteroidManager.Instance.InstantiatePrefabAtRandomEdge(asteroidsPerLevel);

        for (int i = 0; i < lives; i++)
        {
            GameObject y = Instantiate(extraLivesIcon, extraLivesHolder);
            extraLivesIcons.Add(y);
        }

        int x = EnemyManager.Instance.GetBigUFOForLevel();
        for (int i = 0; i < UFOsPerLevel; i++)
        {
            if (i < x) UFOList.Add(bigUFO);
            else UFOList.Add(smallUFO);
        }

        AsteroidManager.Instance.asteroidThreshold = EnemyManager.Instance.GetAsteroidhreshold();
    }

    public void GameOver()
    {
        canGainPoints = false;
        backgroundMusic.loop = false;
        backgroundMusic.clip = gameOverSound;
        backgroundMusic.Play();
        MenuManager.Instance.gameOverMenu.SetActive(true);
    }

    public void GainPoints(int points)
    {
        if (canGainPoints)
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

        if (objectsInCenter <= 0)
        {
            Respawn();
        }
        else
        {
            waitingToRespawn = true;
            Invoke("RespawnIfWaiting", maxRespawnTime);
        }
    }

    void Respawn()
    {
        CharacterControllerShip.Instance.transform.position = Vector3.zero;
        CharacterControllerShip.Instance.transform.rotation = transform.rotation;
        CharacterControllerShip.Instance.gameObject.SetActive(true);
    }

    void RespawnIfWaiting()
    {
        if (waitingToRespawn)
        {
            waitingToRespawn = false;
            Respawn();
        }
    }

    //triggered when sidewinder ship uses its special ability
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
        asteroidsPerLevel++;
        UFOsPerLevel++;

        AsteroidManager.Instance.InstantiatePrefabAtRandomEdge(asteroidsPerLevel);

        UFOList.Clear();
        int x = EnemyManager.Instance.GetBigUFOForLevel();
        for (int i = 0; i < UFOsPerLevel; i++)
        {
            if (i < x) UFOList.Add(bigUFO);
            else UFOList.Add(smallUFO);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        objectsInCenter++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        objectsInCenter--;

        if (objectsInCenter <= 0 && waitingToRespawn)
        {
            Respawn();
            waitingToRespawn = false;
            CancelInvoke("RespawnIfWaiting");
        }
    }
}