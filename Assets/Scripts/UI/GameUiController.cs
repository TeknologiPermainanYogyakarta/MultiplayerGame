using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameUiController : MonoBehaviour
{
    public LeaderboardUi leaderBoard;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    public void UpdateLeaderboard()
    {
        leaderBoard.UpdateScore(GameManager.instance.TankList);
    }

    public void UpdateScore(float _amount)
    {
        scoreText.text = _amount.ToString();
    }
}