using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Random = UnityEngine.Random;

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
    public GameUiController gameUi;
    public MainMenuUi mainMenuUi;

    public CameraFollow cam;

    [Header("Battle")]
    public BattleManager battle;

    [SerializeField]
    private TankStats localTank;

    private List<TankStats> tankList = new List<TankStats>();
    public List<TankStats> TankList => tankList;

    public event Action<TankStats> OnPlayerJoined;

    public event Action<TankStats> OnPlayerDie;

    public void AddTank(TankStats _tank)
    {
        if (tankList.Contains(_tank)) { return; }

        tankList.Add(_tank);
        tankList.Sort((a, b) => a.PlayerNum.CompareTo(b.PlayerNum));

        gameUi.leaderBoard.UpdatePlayerList();

        OnPlayerJoined?.Invoke(_tank);
    }

    public void onGameStart()
    {
        mainMenuUi.gameObject.SetActive(false);
        gameUi.gameObject.SetActive(true);

        gameUi.RestartButton.onClick.AddListener(RestartGame);

        SpawnPlayer();
    }

    public TankStats SpawnPlayer()
    {
        localTank = PhotonNetwork.Instantiate(
            Path.Combine("NetPrefabs", $"{playerPrefab.name}"),
            new Vector3(Random.Range(-15f, 15f), 0f, Random.Range(-15f, 15f)),
            Quaternion.identity, 0).GetComponent<TankStats>();

        return localTank;
    }

    public void PlayerDie(TankStats _playerDie)
    {
        OnPlayerDie?.Invoke(_playerDie);
    }

    public void RestartGame()
    {
        localTank.ResetTank();
        gameUi.RestartUi();
    }
}