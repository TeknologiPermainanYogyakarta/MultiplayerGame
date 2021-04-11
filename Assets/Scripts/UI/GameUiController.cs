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
    private TextMeshProUGUI killPopText;

    [SerializeField]
    private GameObject losePanel;

    private void Start()
    {
        GameManager.instance.OnRestartGame += RestartUi;
        GameManager.instance.OnPlayerDie += DiePop;
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
        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            _name += " MASTER ";
        }
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

    private void DiePop(TankStats tankDie, TankStats tankKiller)
    {
        killPopText.text = $"{tankKiller.name} killed {tankDie.name}.";
    }
}