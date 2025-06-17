using UnityEngine;

[CreateAssetMenu(fileName = "Audio Manager", menuName = "Managers/Audio Manager", order = 0)]
public class AudioManagerSO : ScriptableObject
{
    private static AudioManagerSO _instance;
    public static AudioManagerSO Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<AudioManagerSO>("Managers/Audio Manager");
            }
            return _instance;
        }
    }

    [field: SerializeField] public PoolableAudioSource SoundObject { get; private set; }
    [field: SerializeField, Min(1)] public int PreloadCount { get; private set; } = 15;
    private static float _volumeChangeMultiplier = 0.15f;
    private static float _pitchChangeMultiplier = 0.1f;
    private static PoolBase<PoolableAudioSource> _sourcesPool;

    public static void PlaySound(AudioClip clip, Vector3 position, float volume)
    {
        if (_sourcesPool == null)
        {
            InitializePool();
        }

        float randomVolume = 1f + Random.Range(-_volumeChangeMultiplier, _volumeChangeMultiplier);
        float randomPitch = 1f + Random.Range(-_pitchChangeMultiplier, _pitchChangeMultiplier);

        var source = _sourcesPool.Get();
        var audioSource = source.AudioSource;
        audioSource.transform.position = position;

        audioSource.clip = clip;
        audioSource.volume = randomVolume;
        audioSource.pitch = randomPitch;
        audioSource.Play();

        source.OnClipEnd += () => ReturnSource(source);
    }

    private static void InitializePool()
    {
        var tab = new GameObject($"AudioSources");
        DontDestroyOnLoad(tab);

        _sourcesPool = new PoolBase<PoolableAudioSource>(
            preloadFunc: () =>
            {
                var target = Instantiate(Instance.SoundObject, tab.transform);
                target.gameObject.SetActive(false);
                return target;
            },
            getAction: plate => plate.gameObject.SetActive(true),
            returnAction: plate => plate.gameObject.SetActive(false),
            preloadCount: Instance.PreloadCount,
            canExpand: false
        );
    }

    private static void ReturnSource(PoolableAudioSource source)
    {
        source.OnClipEnd -= () => ReturnSource(source);

        _sourcesPool.Return(source);
    }
}
