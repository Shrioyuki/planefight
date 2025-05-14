using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Pause,
    GameOver
}
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<GameManager>();
            }
            return _instance;
        }
    }

    private int bombCount = 0;
    private int score = 0;
    private GameState gameState = GameState.Playing;

    private float doubleClickThreshold = 0.2f;
    private float lastClickTime = 0;

    public AudioSource useBombAudio;
    public AudioSource gameOverAudio;
    // Start is called before the first frame update
    void Start()
    {
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        UseBombUpdate();
    }

    public void AddBomb()
    {
        bombCount++;
        UIManager.Instance.UpdateBombCountUI(bombCount);
    }

    public void SubBomb()
    {
        bombCount--;
        UIManager.Instance.UpdateBombCountUI(bombCount);
    }

    public void AddScore(int count)
    {
        this.score += count;
        UIManager.Instance.UpdateScoreUI(score);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameState = GameState.Pause;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameState = GameState.Playing;
    }

    public bool IsPause()
    {
        return gameState == GameState.Pause;
    }

    void UseBombUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < doubleClickThreshold)
            {
                //Debug.Log("DoubleClick");
                UseBomb();
            }
            else
            {
                lastClickTime=Time.time;
            }
        }
    }

    void UseBomb()
    {
        if (this.bombCount <= 0 || this.gameState != GameState.Playing) { return; }

        SubBomb();

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            enemy.TakeDamage(9999);
        }
        useBombAudio.Play();
    }

    public void GameOver()
    {
        if (gameState == GameState.GameOver) return;

        PauseGame();
        gameState=GameState.GameOver;
        gameOverAudio.Play();

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UIManager.Instance.ShowGameOverPanel(bestScore, score);

        if(score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
