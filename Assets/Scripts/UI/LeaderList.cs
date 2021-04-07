using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderList : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameTex = null;

    [SerializeField]
    private TextMeshProUGUI scoreTex = null;

    [SerializeField]
    private TextMeshProUGUI rankTex = null;

    public void SetScore(int rank, string name, float score)
    {
        nameTex.text = name;
        rankTex.text = $"{rank}";
        scoreTex.text = $"{score}";
    }
}