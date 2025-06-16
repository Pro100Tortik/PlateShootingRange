using System.Collections;
using UnityEngine;
using System;

public class AutoReturnParticleSystem : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Action _onComplete;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void Set(ParticleSystem ps, Action onComplete)
    {
        _particleSystem = ps;
        _onComplete = onComplete;
    }

    void OnEnable()
    {
        StartCoroutine(WaitForEnd());
        _particleSystem.Play();
    }

    IEnumerator WaitForEnd()
    {
        yield return new WaitUntil(() => !_particleSystem.IsAlive(true));
        _onComplete?.Invoke();
    }
}
