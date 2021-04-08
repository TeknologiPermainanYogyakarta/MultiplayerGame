using Photon.Pun;
using System;
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
    public bool IsDie => isDie;

    [SerializeField]
    private int armor;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        updateHealth();
    }

    public void SetHealth(float _amount)
    {
        if (isDie)
            return;

        pv.RPC(nameof(RpcSetHealth), RpcTarget.AllBuffered, _amount);
    }

    [PunRPC]
    public void RpcSetHealth(float _amount)
    {
        currentHealth += _amount;

        updateHealth();
        if (currentHealth <= 0)
        {
            die();
        }
    }

    public void Firing(TankHealth _target)
    {
        _target.SetHealth(-33f);
    }

    private void die()
    {
        gameObject.SetActive(false);
        isDie = true;

        GameManager.instance.PlayerDie(GetComponent<TankStats>());
        GameManager.instance.gameUi.RestartButton.gameObject.SetActive(true);
    }

    private void updateHealth()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void resetHealth()
    {
        // because resetter is local, another client is ignored

        if (!pv.IsMine) { return; }

        pv.RPC(nameof(RpcResetHealth), RpcTarget.All);
    }

    [PunRPC]
    private void RpcResetHealth()
    {
        currentHealth = maxHealth;
        gameObject.SetActive(true);
        isDie = false;

        updateHealth();
    }
}