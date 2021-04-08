using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("GameStats")]
    public GameUiController gameUiController;
    public MainMenuUi mainMenuUi;

    public CameraFollow cam;

    [SerializeField]
    private TankStats localTank;

    private List<TankStats> tankList = new List<TankStats>();
    public List<TankStats> TankList => tankList;

    public TankStats FindPlayer(TankStats _this)
    {
        return TankList.Find(x => x == _this);
    }

    public void AddTank(TankStats _tank)
    {
        if (tankList.Contains(_tank)) { return; }

        tankList.Add(_tank);
        tankList.Sort((a, b) => a.PlayerNum.CompareTo(b.PlayerNum));

        gameUiController.leaderBoard.UpdatePlayerList();
    }

    public void onGameStart()
    {
        mainMenuUi.gameObject.SetActive(false);
        gameUiController.gameObject.SetActive(true);
    }

    public void OnPlayerEnterRoom()
    {
    }

    public TankStats SpawnPlayer()
    {
        localTank = PhotonNetwork.Instantiate(
            Path.Combine("NetPrefabs", $"{playerPrefab.name}"),
            new Vector3(15f, 0f, 0f),
            Quaternion.identity, 0).GetComponent<TankStats>();

        return localTank;
    }
}