using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crates : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TankStats>())
        {
            other.GetComponent<TankStats>().GiveScore(2f);
            if (PhotonNetwork.IsMasterClient)
            {
                GetComponent<PhotonView>().RPC(nameof(RpcSetActive), RpcTarget.All, false);
                Invoke(nameof(showAgain), 5f);
            }
        }
    }

    private void showAgain()
    {
        GetComponent<PhotonView>().RPC(nameof(RpcSetActive), RpcTarget.All, true);
    }

    [PunRPC]
    private void RpcSetActive(bool _state)
    {
        gameObject.SetActive(_state);
    }
}