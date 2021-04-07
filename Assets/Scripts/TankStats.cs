using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStats : MonoBehaviour, IPunInstantiateMagicCallback
{
    private PhotonView pv;

    [SerializeField]
    private float currentScore;

    public float Score => currentScore;
    public string NickName => pv.Owner.NickName;

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
    }

    public void GiveScore(float _amount)
    {
        if (pv.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        pv.RPC(nameof(RpcScore), RpcTarget.AllBuffered, _amount);
        GameManager.instance.gameUiController.UpdateScore(currentScore);
    }

    [PunRPC]
    private void RpcScore(float _amount)
    {
        currentScore += _amount;
        GameManager.instance.gameUiController.UpdateLeaderboard();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this;
        Debug.Log("this object instantiated");
    }
}