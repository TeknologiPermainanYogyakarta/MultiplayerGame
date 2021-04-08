using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public float destroyAfter = 5;
    public Rigidbody rigidBody;
    public float force = 1000;

    private void Start()
    {
        rigidBody.AddForce(transform.forward * force);
    }

    private void DestroySelf()
    {
        if (GetComponent<PhotonView>().IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider co)
    {
        if (co.GetComponent<TankHealth>())
        {
            co.GetComponent<TankHealth>().TakeDamage(-17.5f);
        }
        DestroySelf();
    }
}