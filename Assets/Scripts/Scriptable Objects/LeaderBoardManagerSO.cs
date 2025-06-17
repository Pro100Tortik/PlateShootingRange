using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Leader Board Manager", menuName = "Managers/Leader Board Manager", order = 2)]
public class LeaderBoardManagerSO : ScriptableObject
{
    private const string SaveKey = "Leaderboard";
    public int maxEntries = 7;

    public ScoreList LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(SaveKey) == false)
            return new ScoreList();

        string json = PlayerPrefs.GetString(SaveKey);
        return JsonUtility.FromJson<ScoreList>(json);
    }

    public void SaveLeaderboard(ScoreList scoreList)
    {
        string json = JsonUtility.ToJson(scoreList);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    [ContextMenu("Reset Scores")]
    public void ResetScores()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.DeleteKey(ScoreManagerSO.LocalBestKey);
    }

    public void AddScore(string playerName, int score)
    {
        var list = LoadLeaderboard();

        list.entries.Add(new ScoreEntry() { PlayerName = playerName, Score = score });

        list.entries = list.entries.OrderByDescending(e => e.Score).Take(maxEntries).ToList();

        SaveLeaderboard(list);
    }
}
