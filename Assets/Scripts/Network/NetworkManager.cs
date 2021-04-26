using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    private void Awake()
    {
        instance = this;
    }

    #region join create room

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to master");
        GameManager.instance.mainMenuUi.SetStatusText("Connected to master");

        GameManager.instance.mainMenuUi.Connected();
    }

    public override void OnCreatedRoom()
    {
        GameManager.instance.mainMenuUi.SetStatusText($"Created room: {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedRoom()
    {
        GameManager.instance.mainMenuUi.SetStatusText($"Joined room: {PhotonNetwork.CurrentRoom.Name}");

        GameManager.instance.onGameStart();

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
    }

    #endregion join create room

    private void checkTanks()
    {
    }
}