using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Score Manager", menuName = "Managers/Score Manager", order = 1)]
public class ScoreManagerSO : ScriptableObject
{
    public static string LocalBestKey = "LocalBestScore";

    public event Action OnScoreChanged;

    public int CurrentScore { get; private set; }

    private void OnEnable()
    {
        ResetScore();
    }

    public void AddScore(int score)
    {
        CurrentScore += score;
        OnScoreChanged?.Invoke();
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke();
    }

    public static int GetBest()
    {
        return PlayerPrefs.GetInt(LocalBestKey, 0);
    }

    public static bool TrySetNewBest(int score)
    {
        if (score > GetBest())
        {
            PlayerPrefs.SetInt(LocalBestKey, score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
}
