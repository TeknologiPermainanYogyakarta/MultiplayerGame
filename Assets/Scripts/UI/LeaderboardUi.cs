using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUi : MonoBehaviour
{
    [SerializeField]
    private GameObject leaderPrefab = null;

    private List<LeaderList> leaderList = new List<LeaderList>();

    [SerializeField]
    private Transform parent = null;

    public void UpdatePlayerList()
    {
        deleteList();

        int amount = GameManager.instance.TankList.Count;

        for (int i = 0; i < amount; i++)
        {
            leaderList.Add(Instantiate(leaderPrefab, parent).GetComponent<LeaderList>());
        }

        UpdateScore(GameManager.instance.TankList);
    }

    private void deleteList()
    {
        leaderList.Clear();
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateScore(List<TankStats> tanks)
    {
        List<TankStats> sorted = new List<TankStats>();

        sorted.AddRange(tanks);
        sorted.Sort((a, b) => b.Score.CompareTo(a.Score));

        for (int i = 0; i < sorted.Count; i++)
        {
            leaderList[i].SetScore(i + 1, sorted[i].NickName, sorted[i].Score);
        }
    }
}