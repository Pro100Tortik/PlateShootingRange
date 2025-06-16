using System.Collections;
using UnityEngine;
using System;

public class PoolableAudioSource : MonoBehaviour
{
    public event Action OnClipEnd;

    public AudioSource AudioSource { get; private set; }
    private float _clipLength;

    private void Awake() => AudioSource = GetComponent<AudioSource>();

    private IEnumerator Start()
    {
        _clipLength = AudioSource.clip.length;
        yield return new WaitForSeconds(_clipLength);
        OnClipEnd?.Invoke();
    }
}
