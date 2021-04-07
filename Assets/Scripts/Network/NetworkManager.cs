using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    private void Awake()
    {
        instance = this;
    }

    #region join create room

    private string[] names = new string[10]
    {
        "happy",
        "kura",
        "kupu",
        "lebah",
        "tidak",
        "makan",
        "naruto",
        "sasuke",
        "sakura",
        "ramen"
    };

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
        GameManager.instance.mainMenuUi.SetStatusText("Connected to master");
        PhotonNetwork.NickName = $"{names[Random.Range(0, names.Length)]} {Random.Range(0, 1000)}";

        GameManager.instance.mainMenuUi.CreateButton.gameObject.SetActive(true);
        GameManager.instance.mainMenuUi.JoinButton.gameObject.SetActive(true);
    }

    public override void OnCreatedRoom()
    {
        GameManager.instance.mainMenuUi.SetStatusText($"Created room: {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom()
    {
        GameManager.instance.mainMenuUi.SetStatusText($"Joined room: {PhotonNetwork.CurrentRoom.Name}");

        GameManager.instance.onGameStart();
        GameManager.instance.SpawnPlayer();

        checkTanks();
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void JoinRoom(string roomName)
    {
        bool available = PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        checkTanks();
        GameManager.instance.OnPlayerEnterRoom();
    }

    #endregion join create room

    private void checkTanks()
    {
    }
}