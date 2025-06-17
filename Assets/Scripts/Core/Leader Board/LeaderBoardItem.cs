using UnityEngine;
using TMPro;

public class LeaderBoardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text positionText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text scoreText;

    private void Awake()
    {
        positionText.text = string.Empty;
        playerNameText.text = string.Empty;
        scoreText.text = string.Empty;
    }

    public void SetPosition(int position)
    {
        positionText.text = $"{position}";
    }

    public void SetItemInfo(string playerName, int score)
    {
        playerNameText.text = $"{playerName}";
        scoreText.text = $"{score}";
    }
}
