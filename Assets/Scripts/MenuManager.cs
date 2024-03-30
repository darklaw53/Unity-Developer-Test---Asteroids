using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    public GameObject pauseMenu, gameOverMenu;
    public TMP_InputField highScoreNameInput;
    public AudioSource pauseSound, unpauseSound;

    string higScoreName;
    bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0;
            pauseSound.Play();
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
            unpauseSound.Play();
        }
    }

    public void UpdateRegisteredString()
    {
        higScoreName = highScoreNameInput.text;
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void EnterHighScore()
    {
        if (higScoreName.Length > 0 || GameManager.Instance.score <= 0)
        {
            if (GameManager.Instance.score > 0)
            {
                GameManager.Instance.scoreBoardSO.scores.Add
                    (higScoreName + " - " + GameManager.Instance.score);
            }

            GameManager.Instance.scoreBoardSO.scores = Utilities.Instance.SortStringList(GameManager.Instance.scoreBoardSO.scores);
            GameManager.Instance.scoreBoardSO.highScore = Utilities.Instance.GetNumberFromString(GameManager.Instance.scoreBoardSO.scores[0]);

            if (GameManager.Instance.scoreBoardSO.scores.Count > 10) GameManager.Instance.scoreBoardSO.scores.RemoveAt (GameManager.Instance.scoreBoardSO.scores.Count -1);

            ReturnToMainMenu();
        }
    }
}
