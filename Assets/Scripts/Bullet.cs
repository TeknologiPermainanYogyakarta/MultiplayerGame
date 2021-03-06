using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public float destroyAfter = 5;
    public Rigidbody rigidBody;
    public float force = 1000;

    private TankHealth owner;

    private void Start()
    {
        rigidBody.AddForce(transform.forward * force);
    }

    public void BulletSetup(TankHealth _owner)
    {
        owner = _owner;
    }

    private void DestroySelf()
    {
        if (GetComponent<PhotonView>().IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider co)
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;

        if (co.GetComponent<TankStats>())
        {
            owner.Firing(co.GetComponent<TankStats>());
        }
        else if (co.GetComponent<Crates>())
        {
            co.GetComponent<Crates>().DeleteCrates();
        }
        DestroySelf();
    }
}