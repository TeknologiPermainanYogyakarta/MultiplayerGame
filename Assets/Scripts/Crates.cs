using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crates : MonoBehaviour
{
    private CratesType currentType = CratesType.score;

    public void SetType(CratesType _type)
    {
        currentType = _type;
        setColor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>())
        {
        }
        else if (other.GetComponent<TankStats>())
        {
            applyEffect(other);
        }
    }

    public void DeleteCrates()
    {
        GameManager.instance.battle.cratesManager.SetupCrates(this);
        setColor();
        GetComponent<PhotonView>().RPC(nameof(RpcSetActive), RpcTarget.AllBuffered, false, (int)currentType);
        Invoke(nameof(showAgain), 5f);
    }

    private void setColor()
    {
        MeshRenderer _mesh = GetComponent<MeshRenderer>();
        switch (currentType)
        {
            case CratesType.bomb:
                _mesh.material.color = new Color(1, 0, 0, 1);
                break;

            case CratesType.heal:
                _mesh.material.color = new Color(0, 1, 0, 1);
                break;

            case CratesType.score:
                _mesh.material.color = new Color(0, 0, 1, 1);
                break;

            default:
                break;
        }
    }

    private void applyEffect(Collider other)
    {
        TankStats tank = other.GetComponent<TankStats>();
        if (!tank.GetComponent<PhotonView>().IsMine)
        {
            return;
        }
        switch (currentType)
        {
            case CratesType.bomb:
                tank.TankHealth.TakeDamage(-60);
                break;

            case CratesType.heal:
                tank.TankHealth.TakeDamage(100);
                break;

            case CratesType.score:
                tank.GiveScore(2);
                tank.TankHealth.GiveShield();
                break;

            default:
                break;
        }

        DeleteCrates();
    }

    public void showAgain()
    {
        GetComponent<PhotonView>().RPC(nameof(RpcSetActive), RpcTarget.AllBuffered, true, (int)currentType);
    }

    [PunRPC]
    private void RpcSetActive(bool _state, int _type)
    {
        gameObject.SetActive(_state);

        SetType((CratesType)_type);
        if (!_state)
        {
            GameManager.instance.battle.cratesManager.spawnExplosion(this.transform.position);
        }
    }
}