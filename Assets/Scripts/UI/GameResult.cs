using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Localization;

public class GameResult : MonoBehaviour
{
    [SerializeField] private ScoreManagerSO scoreManager;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private LocalizedString resultString;
    [SerializeField] private CanvasGroup windowGroup;
    [SerializeField] private CanvasGroup newRecordGroup;
    [SerializeField] private AnimationCurve windowCurve;
    [SerializeField] private RectTransform windowRect;
    [SerializeField] private RectTransform newRecordRect;

    [Header("Window Animation Settings")]
    [SerializeField] private float windowAnimationDuration = 0.5f;
    [SerializeField] private float windowStartScale = 0.5f;

    [Header("New Record Animation Settings")]
    [SerializeField] private float recordRotationAmount = 5f;
    [SerializeField] private float recordScaleVariation = 0.1f;
    [SerializeField] private float recordAnimationSpeed = 2f;

    private Coroutine newRecordAnimation;

    private void Awake()
    {
        windowGroup.alpha = 0f;
        windowGroup.interactable = false;
    }

    public IEnumerator ShowResults()
    {
        resultString.Arguments = new object[] { scoreManager.CurrentScore };
        resultString.RefreshString();
        resultText.text = resultString.GetLocalizedString();

        windowGroup.alpha = 0f;
        windowGroup.interactable = true;

        windowRect.localScale = Vector3.one * windowStartScale;
        newRecordGroup.alpha = 0f;

        yield return StartCoroutine(AnimateWindow());

        if (true)
        {
            newRecordGroup.alpha = 1f;
            newRecordAnimation = StartCoroutine(AnimateNewRecord());
        }
    }

    private IEnumerator AnimateWindow()
    {
        windowGroup.alpha = 1f;
        float time = 0f;
        Vector3 startScale = Vector3.one * windowStartScale;
        Vector3 targetScale = Vector3.one;

        while (time < windowAnimationDuration)
        {
            time += Time.deltaTime;
            float progress = time / windowAnimationDuration;
            float curveValue = windowCurve.Evaluate(progress);

            windowRect.localScale = Vector3.LerpUnclamped(startScale, targetScale, curveValue);
            yield return null;
        }

        windowRect.localScale = targetScale;
    }

    private IEnumerator AnimateNewRecord()
    {
        Vector3 originalScale = newRecordRect.localScale;

        while (true)
        {
            float time = Time.time * recordAnimationSpeed;
            float rotationZ = Mathf.Sin(time) * recordRotationAmount;
            float scaleVariation = Mathf.Sin(time * 1.3f) * recordScaleVariation;

            newRecordRect.localRotation = Quaternion.Euler(0f, 0f, rotationZ);
            newRecordRect.localScale = originalScale + Vector3.one * scaleVariation;

            yield return null;
        }
    }

    private void OnDisable()
    {
        if (newRecordAnimation != null)
        {
            StopCoroutine(newRecordAnimation);
            newRecordAnimation = null;
        }
    }
}