using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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
    public Button CreateButton => createButton;
    public Button JoinButton => joinButton;

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