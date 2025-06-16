using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Create New Gun")]
public class GunDataSO : ScriptableObject
{
    [field: SerializeField, Min(1)] public int MaxAmmoCapacity { get; private set; } = 7;
    [field: SerializeField, Min(0.1f)] public float ReloadTime { get; private set; } = 0.4f;
    [field: SerializeField, Min(0.01f)] public float HitRadius { get; private set; } = 0.2f;
    [field: SerializeField] public AudioClip FireSound { get; private set; }
    [field: SerializeField] public AudioClip NoAmmoSound { get; private set; }
    [field: SerializeField] public AudioClip ReloadingSound { get; private set; }
}
