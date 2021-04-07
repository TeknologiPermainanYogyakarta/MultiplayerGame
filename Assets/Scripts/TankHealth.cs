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

    private float currentHealth = 1000f;

    public float maxHealth = 1000f;

    private bool isDie;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //pv.RPC(nameof(RpcSetHealth), RpcTarget.AllBuffered, currentHealth);

        updateHealth();
    }

    public void TakeDamage(float _amount)
    {
        if (isDie)
            return;
        currentHealth -= _amount;

        pv.RPC(nameof(RpcSetHealth), RpcTarget.AllBuffered, currentHealth);
    }

    [PunRPC]
    public void RpcSetHealth(float _healthSync)
    {
        currentHealth = _healthSync;

        if (currentHealth <= 0)
        {
            die();
        }

        updateHealth();
    }

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