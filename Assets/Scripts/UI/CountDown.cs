using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Localization;
using System;

public class CountDown : MonoBehaviour
{
    public event Action OnCountDownEnd;

    [SerializeField] private CanvasGroup textGroup;
    [SerializeField] private TMP_Text countDownText;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float maxScale = 1.5f;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private AnimationCurve popCurve;
    [SerializeField] private LocalizedString startString;
    [SerializeField] private LocalizedString stopString;
    [SerializeField] private Color[] numberColors = new Color[]
    {
        Color.red,
        Color.yellow,
        Color.red + Color.yellow,
        Color.green
    };

    public IEnumerator GameStartCountDown()
    {
        textGroup.alpha = 1f;

        yield return AnimateText("3", 0);

        yield return AnimateText("2", 1);

        yield return AnimateText("1", 2);

        yield return AnimateText(startString.GetLocalizedString(), 3);

        OnCountDownEnd?.Invoke();
    }

    public IEnumerator GameEndCountdown()
    {
        textGroup.alpha = 1f;

        yield return AnimateText("3", 3);

        yield return AnimateText("2", 2);

        yield return AnimateText("1", 1);

        yield return AnimateText(stopString.GetLocalizedString(), 0);
    }

    private IEnumerator AnimateText(string text, int colorIndex)
    {
        countDownText.text = text;
        countDownText.color = numberColors[colorIndex];
        countDownText.transform.localScale = Vector3.one;
        textGroup.alpha = 1f;

        float time = 0f;
        while (time < animationDuration)
        {
            time += Time.deltaTime;
            float t = time / animationDuration;

            countDownText.transform.localScale = Vector3.Lerp(
                Vector3.one,
                Vector3.one * maxScale,
                popCurve.Evaluate(t)
            );

            if (t > 0.5f)
            {
                textGroup.alpha = Mathf.Lerp(1f, 0f, (t - 0.5f) * 2f);
            }

            yield return null;
        }

        countDownText.transform.localScale = Vector3.one * maxScale;
        textGroup.alpha = 0f;
    }
}