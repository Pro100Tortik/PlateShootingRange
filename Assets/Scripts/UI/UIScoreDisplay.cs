using UnityEngine;
using TMPro;
using UnityEngine.Localization;

public class UIScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreManagerSO scoreManager;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private LocalizedString scoreString;

    private void Awake()
    {
        scoreManager.OnScoreChanged += UpdateScore;

        scoreString.StringChanged += UpdateText;

        UpdateScore();
    }

    private void OnDestroy()
    {
        scoreManager.OnScoreChanged -= UpdateScore;
    }

    private void UpdateScore()
    {
        scoreString.Arguments = new object[] { scoreManager.CurrentScore };
        scoreString.RefreshString();
    }

    private void UpdateText(string text) => scoreText.text = text;
}
