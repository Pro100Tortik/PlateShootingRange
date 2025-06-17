using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private LeaderBoardManagerSO leaderBoardManager;
    [SerializeField] private ScoreManagerSO scoreManager;
    [SerializeField] private LeaderBoardItem itemPrefab;
    [SerializeField] private Transform itemsParent;
    [SerializeField] private CanvasGroup saveGroup;
    private List<LeaderBoardItem> _spawnedItems = new();

    public void LoadRecords()
    {
        foreach (var item in _spawnedItems)
        {
            Destroy(item.gameObject);
        }
        _spawnedItems.Clear();

        var scores = leaderBoardManager.LoadLeaderboard();

        for (int i = 0; i < 7; i++)
        {
            var item = Instantiate(itemPrefab, itemsParent);
            item.SetPosition(i + 1);
            _spawnedItems.Add(item);

            if (i < scores.entries.Count)
            {
                var score = scores.entries[i];
                item.SetItemInfo(score.PlayerName, score.Score);
            }
            else
            {
                item.SetItemInfo("-", 0);
            }
        }
    }

    public void AddScoreToBoard()
    {
        bool enable = IsRecordHighEnough() == true;

        saveGroup.alpha = enable ? 1f : 0f;
        saveGroup.interactable = enable;
    }

    public bool IsRecordHighEnough()
    {
        var list = leaderBoardManager.LoadLeaderboard();

        if (list.entries.Count < 7) 
            return true;

        return scoreManager.CurrentScore > list.entries.Last().Score;
    }

    public void AddRecordAndRefresh(string playerName)
    {
        if (string.IsNullOrEmpty(playerName))
            return;

        leaderBoardManager.AddScore(playerName, scoreManager.CurrentScore);
        LoadRecords();

        saveGroup.alpha = 0f;
        saveGroup.interactable = false;
    }
}
