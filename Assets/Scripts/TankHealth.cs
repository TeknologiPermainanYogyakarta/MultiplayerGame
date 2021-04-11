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
    private GameObject shield;

    [SerializeField]
    private Image healthBar = null;

    private float currentHealth = 100f;

    public float maxHealth = 100f;

    private bool isDie;
    public bool IsDie => isDie;

    private bool isShielded;

    [SerializeField]
    private int armor;

    private Coroutine shieldRoutine;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        updateHealth();
    }

    public void TakeDamage(float _amount)
    {
        if (isDie || isShielded)
            return;

        pv.RPC(nameof(RpcTakeDamage), RpcTarget.AllBuffered, _amount);
    }

    [PunRPC]
    public void RpcTakeDamage(float _amount)
    {
        currentHealth += _amount;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        updateHealth();
        if (currentHealth <= 0)
        {
            die();
        }
    }

    public void Firing(TankHealth _target)
    {
        _target.TakeDamage(-7f);
    }

    private void die()
    {
        gameObject.SetActive(false);
        isDie = true;
        isShielded = false;

        GameManager.instance.PlayerDie(GameManager.instance.LocalTank);
    }

    private void updateHealth()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
    }

    public void ResetHealth()
    {
        if (pv.IsMine)
        {
            pv.RPC(nameof(RpcSetActive), RpcTarget.AllBuffered);
        }
        currentHealth = maxHealth;

        isShielded = false;
        shield.gameObject.SetActive(isShielded);
        isDie = false;

        updateHealth();
    }

    [PunRPC]
    private void RpcSetActive()
    {
        gameObject.SetActive(true);
    }

    public void GiveShield()
    {
        if (pv.IsMine)
        {
            pv.RPC(nameof(RpcGiveShield), RpcTarget.AllBuffered, true);

            if (shieldRoutine != null)
            {
                StopCoroutine(shieldProgress());
            }
            shieldRoutine = StartCoroutine(shieldProgress());
        }
    }

    [PunRPC]
    private void RpcGiveShield(bool _state)
    {
        isShielded = _state;
        shield.gameObject.SetActive(isShielded);
    }

    private IEnumerator shieldProgress()
    {
        yield return new WaitForSeconds(5f);
        pv.RPC(nameof(RpcGiveShield), RpcTarget.AllBuffered, false);
    }
}