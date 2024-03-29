using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public ShipLayoutSO shipLayoutSO;
    public GameObject playerDeathExplosion;
    public int lives = 3;

    public List<int> asteroidsPerLevel;

    int currentLevel;

    void Start()
    {
        AsteroidManager.Instance.InstantiatePrefabAtRandomEdge(asteroidsPerLevel[currentLevel]);
    }

    public void GameOver()
    {

    }

    //triggered when player looses a life
    public IEnumerator Exploded()
    {
        playerDeathExplosion.SetActive(true);
        playerDeathExplosion.transform.position = CharacterControllerShip.Instance.transform.position;
        
        yield return new WaitForSeconds(2);

        CharacterControllerShip.Instance.transform.position = Vector3.zero;
        CharacterControllerShip.Instance.transform.rotation = transform.rotation;
        CharacterControllerShip.Instance.gameObject.SetActive(true);
        playerDeathExplosion.SetActive(false);
    }

    public void NextLevel()
    {
        currentLevel++;

        AsteroidManager.Instance.InstantiatePrefabAtRandomEdge(asteroidsPerLevel[currentLevel]);
    }
}