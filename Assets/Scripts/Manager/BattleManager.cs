using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class BattleManager : MonoBehaviour
{
    private const byte WIN_EVENT = 0;

    public CratesManager cratesManager;

    [SerializeField]
    private BattleState currentBattle = BattleState.waiting;

    private void Start()
    {
        GameManager.instance.OnPlayerJoined += battleSetup;
        GameManager.instance.OnRestartGame += restartState;
    }

    #region raise Event

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code != WIN_EVENT)
            return;

        currentBattle = (BattleState)obj.CustomData;
        raiseEventGameOver();
    }

    private void raiseEventGameOver()
    {
        GameManager.instance.gameUi.RestartButton.gameObject.SetActive(true);
        GameManager.instance.LocalTank.GameOver();
    }

    #endregion raise Event

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
            if (PhotonNetwork.IsMasterClient)
            {
                currentBattle = BattleState.winning;

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(WIN_EVENT, currentBattle, raiseEventOptions, SendOptions.SendReliable);
            }
        }
    }

    private void win(List<TankStats> tanks)
    {
        tanks.Find(t => !t.TankHealth.IsDie).Winning(true);
    }

    private void restartState()
    {
        if (GameManager.instance.TankList.Count >= 2)
        {
            currentBattle = BattleState.battling;
        }
        else
        {
            currentBattle = BattleState.waiting;
            GameManager.instance.OnPlayerDie -= winCheck;
        }
    }
}

public enum BattleState { waiting, battling, winning }