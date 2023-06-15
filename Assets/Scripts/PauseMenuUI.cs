using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _menuButton;

    private void Start()
    {
        _continueButton.onClick.AddListener(OnContinueButtonClicked);
        _menuButton.onClick.AddListener(OnMenuButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
        GameManager.Instance.ResumeGame();
    }

    private void OnMenuButtonClicked()
    {
        GameManager.Instance.GoBackToMenu();
    }
}
