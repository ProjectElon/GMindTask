using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameRunning => !_isPaused;

    [SerializeField] private GameObject _items;
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private TextMeshProUGUI _gameStatusText;
    [SerializeField] private GameObject _checkPoint;

    private int _remainingItems;

    private bool _isPaused;
    
    private void Awake()
    {
        Instance = this;
        _remainingItems = _items.transform.childCount;
        _gameOverUI.SetActive(false);
        _checkPoint.SetActive(false);
        _pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void GameOver(bool won)
    {
        if (won)
        {
            _gameStatusText.text = "YOU WON";
        }
        else
        {
            _gameStatusText.text = "YOU LOST";
        }

        Time.timeScale = 0.0f;
        _gameOverUI.SetActive(true);
    }

    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0.0f;
        _pauseMenuUI.SetActive(true);
    }
    
    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1.0f;
        _pauseMenuUI.SetActive(false);
    }

    public void OnPlayerCollectedAnItem()
    {
        _remainingItems--;
        if (_remainingItems == 0)
        {
            _checkPoint.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoBackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
