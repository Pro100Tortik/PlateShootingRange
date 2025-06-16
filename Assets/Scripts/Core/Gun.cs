using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public event Action OnAmmoAmountChanged;
    public event Action<bool> OnReload;

    [field: SerializeField] public GunDataSO GunData { get; private set; }
    [field: SerializeField] public int CurrentAmmo { get; private set; }
    public float ReloadingTimer { get; private set; }

    [SerializeField] private GameObject bulletHole;
    [SerializeField] private ScoreManagerSO scoreManager;
    [SerializeField] private Camera shootingCamera;
    [SerializeField] private LayerMask targetMask;
    private bool _isReloading = false;
    private bool _canShoot = false;

    private RaycastHit2D[] _result = new RaycastHit2D[4];

    private void Awake()
    {
        CurrentAmmo = GunData.MaxAmmoCapacity;

        GameManager.OnGameStart += UnblockGun;
        GameManager.OnGameStop += BlockGun;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= UnblockGun;
        GameManager.OnGameStop -= BlockGun;
    }

    private void UnblockGun()
    {
        _canShoot = true;
    }

    private void BlockGun()
    {
        _canShoot = false;
    }

    private void Update()
    {
        if (_canShoot == false)
            return;

        if (ReloadingTimer > 0)
        {
            ReloadingTimer -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CurrentAmmo <= 0)
            {
                AudioManagerSO.PlaySound(GunData.NoAmmoSound, transform.position, 0.5f);
                return;
            }

            AudioManagerSO.PlaySound(GunData.FireSound, transform.position, 0.3f);

            CurrentAmmo -= 1;
            OnAmmoAmountChanged?.Invoke();

            var point = shootingCamera.ScreenToWorldPoint(Input.mousePosition);

            IsIndirectHit(point);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && _isReloading == false && CurrentAmmo < GunData.MaxAmmoCapacity)
        {
            _isReloading = true;

            ReloadingTimer = GunData.ReloadTime;
            
            OnReload?.Invoke(true);

            AudioManagerSO.PlaySound(GunData.ReloadingSound, transform.position, 0.5f);

            Invoke(nameof(ReloadGun), GunData.ReloadTime);
        }
    }

    //private bool IsDirectHit(Vector3 point)
    //{
    //    var hit2D = Physics2D.Raycast(point, Vector2.zero, 100, targetMask);
    //    if (hit2D == true && hit2D.collider.TryGetComponent(out ITarget target) == true)
    //    {
    //        target.Hit(out var points);

    //        scoreManager.AddScore(points);
    //        return true;
    //    }

    //    return false;
    //}

    private void IsIndirectHit(Vector3 point)
    {
        _result = Physics2D.CircleCastAll(point, GunData.HitRadius, Vector2.zero, 100, targetMask);

        RaycastHit2D? closestHit = null;
        int closestLayer = -1000;

        foreach (var hit in _result)
        {
            var spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
            if (closestHit == null || (spriteRenderer != null && (spriteRenderer.sortingOrder > closestLayer)))
            {
                closestHit = hit;
            }
        }

        if (closestHit.HasValue == false || closestHit.Value.collider.TryGetComponent(out ITarget target) == false)
        {
            point.z = -1;
            EffectsManager.Instance.Spawn(bulletHole, point, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360f)));
            return;
        }

        target.Hit(out var points);

        scoreManager.AddScore(points);
    }

    private void ReloadGun()
    {
        CurrentAmmo = GunData.MaxAmmoCapacity;
        OnAmmoAmountChanged?.Invoke();
        OnReload?.Invoke(false);

        _isReloading = false;
    }
}
