using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    private PhotonView pv;

    [SerializeField]
    private Image healthBar = null;

    private float currentHealth = 100f;

    public float maxHealth = 100f;

    private bool isDie;

    [SerializeField]
    private int armor;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //if (pv.IsMine)
        //{
        //    pv.RPC(nameof(RpcArmor), RpcTarget.AllBuffered, Random.Range(-100, 100));
        //}

        updateHealth();
    }

    public void TakeDamage(float _amount)
    {
        if (!pv.IsMine == false) { return; }

        if (isDie)
            return;

        //pv.RPC(nameof(RpcTakeDamageManager), RpcTarget.AllBuffered, _amount, pv.Owner.ActorNumber);
        pv.RPC(nameof(RpcTakeDamage), RpcTarget.AllBuffered, _amount);
    }

    //[PunRPC]
    //public void RpcTakeDamageManager(float _amount, int playerIndex)
    //{
    //    GameManager.instance.TankList[playerIndex - 1].TankHealth.RpcTakeDamage(_amount);
    //}

    [PunRPC]
    public void RpcTakeDamage(float _amount)
    {
        currentHealth += _amount;

        updateHealth();
        if (currentHealth <= 0)
        {
            die();
        }
    }

    //[PunRPC]
    //public void RpcArmor(int _amount)
    //{
    //    armor += _amount;
    //    Debug.LogError($"{armor} is the armor");
    //}

    private void die()
    {
        gameObject.SetActive(false);
        isDie = true;
    }

    private void updateHealth()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}