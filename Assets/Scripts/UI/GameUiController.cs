using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameUiController : MonoBehaviour
{
    public LeaderboardUi leaderBoard;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI playerNameText;

    [SerializeField]
    private Button restartButton;
    public Button RestartButton => restartButton;

    [SerializeField]
    private GameObject winPanel;

    [SerializeField]
    private GameObject losePanel;

    private void Start()
    {
        GameManager.instance.OnRestartGame += RestartUi;
    }

    public void UpdateLeaderboard()
    {
        leaderBoard.UpdateScore(GameManager.instance.TankList);
    }

    public void UpdateScore(float _amount)
    {
        scoreText.text = _amount.ToString();
    }

    public void SetName(string _name)
    {
        playerNameText.text = _name;
    }

    public void GameOver(bool isWin)
    {
        if (isWin)
        {
            winPanel.SetActive(true);
        }
        else
        {
            losePanel.SetActive(true);
        }
    }

    public void RestartUi()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        RestartButton.gameObject.SetActive(false);
    }
}