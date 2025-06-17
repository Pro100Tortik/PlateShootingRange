using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStart;
    public static event Action OnGameStop;

    [SerializeField] private CountDown countDown;
    [SerializeField] private ScoreManagerSO scoreManager;
    [SerializeField] private Timer timer;
    [SerializeField] private AudioClip gameEndSound;
    [SerializeField] private GameResult gameResult;
    [SerializeField] private float gametime = 60f;

    private void Awake()
    {
        scoreManager.ResetScore();

        countDown.OnCountDownEnd += StartGame;

        timer.OnThreeSecondsLeft += StartCountdown;
        timer.OnTimerEnd += StopGame;

        timer.SetTime(gametime);
    }

    private void OnDestroy()
    {
        countDown.OnCountDownEnd -= StartGame;

        timer.OnThreeSecondsLeft -= StartCountdown;
        timer.OnTimerEnd -= StopGame;
    }

    private void Start()
    {
        StartCoroutine(countDown.GameStartCountDown());
    }

    private void StartCountdown()
    {
        StartCoroutine(countDown.GameEndCountdown());
    }

    private void StartGame()
    {
        timer.StartTimer();

        OnGameStart?.Invoke();
    }

    private void StopGame()
    {
        OnGameStop?.Invoke();

        AudioManagerSO.PlaySound(gameEndSound, transform.position, 0.3f);

        StartCoroutine(gameResult.ShowResults());
    }
}
