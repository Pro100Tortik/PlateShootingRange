using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action OnTimerEnd;
    public event Action OnThreeSecondsLeft;

    public float TimeLeft { get; private set; }
    [SerializeField] private float standartGameTime = 120f;
    [SerializeField] private TMP_Text timerText;
    private bool _triggeredThreeSeconds = false;
    private bool _isRunning = false;

    private void Awake()
    {
        TimeLeft = standartGameTime;

        int minutes = (int)Mathf.Floor(TimeLeft / 60f);
        int seconds = (int)Mathf.Floor(TimeLeft % 60f);
        timerText.text = $"{minutes}:{Mathf.Max(0, seconds): 00}";
    }

    public void SetTime(float time)
    {
        TimeLeft = time;
    }

    public void StartTimer()
    {
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
        OnTimerEnd?.Invoke();
    }

    private void Update()
    {
        // Can't stop at the end
        if (_isRunning == false)
        {
            if (_triggeredThreeSeconds == false && TimeLeft > 0)
                return;
        }

        TimeLeft = TimeLeft > 0 ? TimeLeft - Time.deltaTime : 0;

        int minutes = (int)Mathf.Floor(TimeLeft / 60f);
        int seconds = (int)Mathf.Floor(TimeLeft % 60f);
        timerText.text = $"{minutes}:{Mathf.Max(0, seconds): 00}";

        if (TimeLeft <= 0f && _isRunning == true)
        {
            _isRunning = false;
            StopTimer();
            return;
        }

        if (TimeLeft < 3.2f && _triggeredThreeSeconds == false)
        {
            _triggeredThreeSeconds = true;

            OnThreeSecondsLeft?.Invoke();
        }
    }
}
