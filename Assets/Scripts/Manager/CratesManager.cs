using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CratesManager : MonoBehaviour
{
    [SerializeField]
    private List<Crates> crates = new List<Crates>();

    [SerializeField]
    private Vector3 center;

    [SerializeField]
    private Vector3 spawnArea;

    private PhotonView pv;

    [SerializeField]
    private GameObject vfxExplosion;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = new Color(1, 0, 0, 0.3f);
        //Gizmos.DrawCube(center, spawnArea);
    }

    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }
        foreach (Crates item in crates)
        {
            SetupCrates(item);
            item.showAgain();
        }
    }

    public void SetupCrates(Crates _crate)
    {
        CratesType randomType = (CratesType)Random.Range(0, Enum.GetNames(typeof(CratesType)).Length);
        _crate.SetType(randomType);
    }

    public void spawnExplosion(Vector3 pos)
    {
        GameObject explosion = Instantiate(vfxExplosion, pos, Quaternion.identity);
        Destroy(explosion, 3f);
    }
}

public enum CratesType { bomb, heal, score }