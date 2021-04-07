using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : MonoBehaviour
{
    private PhotonView pv;
    private Rigidbody rb;
    private Animator anim;

    [SerializeField]
    private Transform gunPos = null;

    private float inputX;
    private float inputY;

    [Header("Stats")]
    [SerializeField]
    private float speed = 20f;

    [SerializeField]
    private float rotateSpeed = 100f;

    [SerializeField]
    private GameObject bullet = null;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        setCamera();
    }

    private void setCamera()
    {
        if (pv.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        GameManager.instance.cam.SetTarget(this);
    }

    private void Update()
    {
        if (pv.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        anim.SetBool("Moving", rb.velocity != Vector3.zero);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            fire();
        }
    }

    private void fire()
    {
        GameObject spawned = PhotonNetwork.Instantiate(System.IO.Path.Combine("NetPrefabs", bullet.name), gunPos.position, transform.rotation);
        anim.SetTrigger("Shoot");
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.transform.TransformDirection(Vector3.forward) * inputY * speed;

        transform.Rotate(0, inputX * rotateSpeed * Time.deltaTime, 0);
    }
}