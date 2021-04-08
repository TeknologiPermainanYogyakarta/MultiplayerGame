using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStats : MonoBehaviour, IPunInstantiateMagicCallback
{
    private PhotonView pv;

    public TankHealth TankHealth;

    [SerializeField]
    private float currentScore;

    public float Score => currentScore;
    public string NickName => pv.Owner.NickName;
    public int PlayerNum => pv.Owner.ActorNumber;

    private bool isWin;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        tankSetup();
    }

    // executed in all client or host
    private void tankSetup()
    {
        gameObject.name = pv.Owner.NickName;

        GameManager.instance.AddTank(this);

        if (pv.IsMine)
            GameManager.instance.gameUi.SetName(gameObject.name);
    }

    public void GiveScore(float _amount)
    {
        if (pv.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        pv.RPC(nameof(RpcScore), RpcTarget.AllBuffered, _amount);
        GameManager.instance.gameUi.UpdateScore(currentScore);
    }

    [PunRPC]
    private void RpcScore(float _amount)
    {
        currentScore += _amount;
        GameManager.instance.gameUi.UpdateLeaderboard();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this;
        Debug.Log("this object instantiated");
    }

    public void Winning(bool _isWin)
    {
        if (pv.IsMine)
        {
            pv.RPC(nameof(RpcWinning), RpcTarget.AllBuffered, _isWin);
        }
    }

    [PunRPC]
    private void RpcWinning(bool _isWin)
    {
        isWin = _isWin;
    }

    public void GameOver()
    {
        if (pv.IsMine)
        {
            GameManager.instance.gameUi.GameOver(isWin);
        }
    }

    public void ResetTank()
    {
        TankHealth.resetHealth();
        isWin = false;
    }
}