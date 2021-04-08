using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private BattleState currentBattle = BattleState.waiting;

    private void Start()
    {
        GameManager.instance.OnPlayerJoined += battleSetup;
    }

    private void battleSetup(TankStats newTank)
    {
        if (currentBattle != BattleState.waiting) { return; }

        if (GameManager.instance.TankList.Count >= 2)
        {
            GameManager.instance.OnPlayerDie += winCheck;
            currentBattle = BattleState.battling;
        }
    }

    public void winCheck(TankStats dieTank)
    {
        List<TankStats> tanks = GameManager.instance.TankList;

        int tankAlive = tanks.Count(t => !t.TankHealth.IsDie);
        //Debug.LogError($"{tankAlive} left.");

        if (tankAlive <= 1)
        {
            win(tanks);
        }
    }

    private void win(List<TankStats> tanks)
    {
        tanks.Find(t => !t.TankHealth.IsDie).Winning(true);

        currentBattle = BattleState.winning;

        foreach (TankStats tank in tanks)
        {
            tank.GameOver();
        }
    }
}

public enum BattleState { waiting, battling, winning }