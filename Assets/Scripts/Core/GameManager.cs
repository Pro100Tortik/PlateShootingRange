using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStart;
    public static event Action OnGameStop;

    [SerializeField] private CountDown countDown;
    [SerializeField] private Timer timer;
    [SerializeField] private AudioClip gameEndSound;
    [SerializeField] private GameResult gameResult;

    private void Awake()
    {
        countDown.OnCountDownEnd += StartGame;

        timer.OnThreeSecondsLeft += StartCountdown;
        timer.OnTimerEnd += StopGame;
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
