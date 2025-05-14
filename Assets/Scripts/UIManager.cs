using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<UIManager>();
            }
            return _instance;
        }
    }

    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI bombCountTMP;

    public Button pauseButton;
    public Button resumeButton;

    public GameObject gameOverPanel;
    public TextMeshProUGUI bestScoreTMP;
    public TextMeshProUGUI currentScoreTMP;

    public Button restartButton;
    public Button quitButton;

    private void Start()
    {
        pauseButton.onClick.RemoveListener(this.OnPauseButtonClick);
        resumeButton.onClick.RemoveListener(this.OnResumeButtonClick);

        restartButton.onClick.RemoveListener(this.OnRestartButtonClick);
        quitButton.onClick.RemoveListener(this.OnQuitButtonClick);

        pauseButton.onClick.AddListener(this.OnPauseButtonClick);
        resumeButton.onClick.AddListener(this.OnResumeButtonClick);

        restartButton.onClick.AddListener(this.OnRestartButtonClick);
        quitButton.onClick.AddListener(this.OnQuitButtonClick);

    }

    public void UpdateScoreUI(int score)
    {
        this.scoreTMP.text = score + "";
    }

    public void UpdateBombCountUI(int count)
    {
        this.bombCountTMP.text = count + "";
    }

    void OnPauseButtonClick()
    {
        pauseButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        GameManager.Instance.PauseGame();
        AudioManager.Instance.PlayButtonClip();
    }

    void OnResumeButtonClick()
    {
        pauseButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
        AudioManager.Instance.PlayButtonClip();
    }

    public void ShowGameOverPanel(int bestScore,int currentScore)
    {
        gameOverPanel.SetActive(true);
        this.bestScoreTMP.text = bestScore + "";
        this.currentScoreTMP.text = currentScore + "";

    }

    void OnRestartButtonClick()
    {
        GameManager.Instance.RestartGame();
        AudioManager.Instance.PlayButtonClip();
    }

    void OnQuitButtonClick()
    {
        GameManager.Instance.QuitGame();
        AudioManager.Instance.PlayButtonClip();
    }
}
