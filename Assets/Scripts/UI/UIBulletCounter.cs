using UnityEngine;

public class UIBulletCounter : MonoBehaviour
{
    [SerializeField] private Camera vfxCamera;
    [SerializeField] private Gun gun;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletsParent;
    [SerializeField] private CanvasGroup reloadingTextGroup;
    [SerializeField] private ParticleSystem shellParticle;
    private GameObject[] _bulletsArray;
    private int _capacity;

    private void Awake()
    {
        reloadingTextGroup.alpha = 0f;

        gun.OnAmmoAmountChanged += UpdateAmmoCounter;
        gun.OnReload += UpdateReloadingText;

        _capacity = gun.GunData.MaxAmmoCapacity;

        _bulletsArray = new GameObject[_capacity];

        for (int i = 0; i < _capacity; i++)
        {
            var bullet = Instantiate(bulletPrefab, bulletsParent);
            _bulletsArray[_capacity - 1 - i] = bullet;
        }

        UpdateAmmoCounter();
    }

    private void OnDestroy()
    {
        gun.OnAmmoAmountChanged -= UpdateAmmoCounter;
        gun.OnReload -= UpdateReloadingText;
    }

    private void UpdateAmmoCounter()
    {
        for (int i = 0; i < _capacity; i++)
        {
            if (_bulletsArray[i].activeSelf == true && (i + 1 > gun.CurrentAmmo))
            { 
                Vector3 worldPos = vfxCamera.ScreenToWorldPoint(_bulletsArray[i].transform.position);
                worldPos.z = -1f;
                shellParticle.transform.position = worldPos;
                shellParticle.Play();
            }

            _bulletsArray[i].SetActive(i < gun.CurrentAmmo);
        }
    }

    private void UpdateReloadingText(bool isReloading)
    {
        reloadingTextGroup.alpha = isReloading == true ? 1f : 0f;
    }
}
