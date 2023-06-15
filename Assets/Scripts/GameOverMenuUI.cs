using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenuUI : MonoBehaviour
{
    [SerializeField] private Button _RestartButton;
    [SerializeField] private Button _MenuButton;

    private void Start()
    {
        _RestartButton.onClick.AddListener(OnRestartButtonClicked);
        _MenuButton.onClick.AddListener(OnMenuButtonClicked);
    }
    
    private void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }

    private void OnMenuButtonClicked()
    {
        GameManager.Instance.GoBackToMenu();
    }
}
