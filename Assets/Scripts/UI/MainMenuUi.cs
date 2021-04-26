using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using Photon.Pun;

public class MainMenuUi : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField createField = null;

    [SerializeField]
    private Button createButton = null;

    [SerializeField]
    private TMP_InputField joinField = null;

    [SerializeField]
    private Button joinButton = null;

    [SerializeField]
    private TextMeshProUGUI statusText = null;

    [SerializeField]
    private Image mainPanel = null;

    [SerializeField]
    private TMP_InputField usernameField = null;

    [SerializeField]
    private Button randomButton = null;

    private string[] names = new string[25]
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
        "ramen",
        "Sosis",
        "Galon",
        "Kucing",
        "Bambang",
        "Rantang",
        "Radit",
        "Kemoceng",
        "Jerapah",
        "Agresi",
        "Cangcorang",
        "Susilo",
        "Berkah",
        "Baso",
        "Menangis",
        "Pilu"
    };

    public void SetStatusText(string _text)
    {
        statusText.text = _text;
    }

    public void SetMainPanel(bool _state)
    {
        mainPanel.gameObject.SetActive(_state);
    }

    private void Start()
    {
        createButton.onClick.AddListener(createButtonAction);
        joinButton.onClick.AddListener(joinButtonAction);

        RandomName();

        randomButton.onClick.AddListener(RandomName);
    }

    private void RandomName()
    {
        string randomName = $"{names[Random.Range(0, names.Length)]} {Random.Range(0, 1000)}";

        PhotonNetwork.NickName = randomName;
        usernameField.text = randomName;
    }

    public void SetName(string _endName)
    {
        PhotonNetwork.NickName = _endName;
    }

    private void createButtonAction()
    {
        if (string.IsNullOrEmpty(createField.text))
        {
            Debug.Log("cannot empty");
            return;
        }
        NetworkManager.instance.CreateRoom(createField.text);
    }

    internal void Connected()
    {
        createButton.interactable = true;
        joinButton.interactable = true;
    }

    private void joinButtonAction()
    {
        if (string.IsNullOrEmpty(joinField.text))
        {
            Debug.Log("cannot empty");
            return;
        }

        NetworkManager.instance.JoinRoom(joinField.text);
    }
}