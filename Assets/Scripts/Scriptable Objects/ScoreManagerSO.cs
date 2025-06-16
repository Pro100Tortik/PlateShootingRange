using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Score Manager", menuName = "Managers/Score Manager", order = 1)]
public class ScoreManagerSO : ScriptableObject
{
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
}
