using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetsManager : MonoBehaviour
{
    [SerializeField] private Plate plate;
    [SerializeField] private int dynamicPlatesPerLaunch = 2;
    [SerializeField] private AudioClip tossSound;

    private List<TargetSpawnpoint> _dynamicSpawnPoints = new();

    private bool _isGameRunning = false;

    #region Game Variables
    private float _currentDynamicRespawnDelay = 10f;
    private int _destroyedPlatesCount = 0;
    private float _timeSinceLastDynamicLaunch = 0f;
    private int _platesToTriggerDynamic = 4;
    #endregion

    private void Awake()
    {
        var spawnPoints = FindObjectsByType<TargetSpawnpoint>(FindObjectsSortMode.None).ToList();

        foreach (var spawnPoint in spawnPoints)
        {
            // Add a target to every spawnpoint
            var target = Instantiate(plate, spawnPoint.transform.position, Quaternion.identity, spawnPoint.transform);
            target.gameObject.SetActive(false);

            target.OnDestroy += () => _destroyedPlatesCount++;

            spawnPoint.SetTarget(target);

            if (spawnPoint.LaunchTarget == false)
            {
                spawnPoint.SpawnTarget();
            }
            else
            {
                _dynamicSpawnPoints.Add(spawnPoint);
            }
        }

        GameManager.OnGameStart += StartSpawning;
        GameManager.OnGameStop += StopSpawning;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= StartSpawning;
        GameManager.OnGameStop -= StopSpawning;
    }

    private void FixedUpdate()
    {
        if (_isGameRunning == false)
            return;

        if (_dynamicSpawnPoints.Count == 0)
            return;

        _timeSinceLastDynamicLaunch += Time.fixedDeltaTime;

        if (_destroyedPlatesCount >= _platesToTriggerDynamic || _timeSinceLastDynamicLaunch >= _currentDynamicRespawnDelay)
        {
            LaunchDynamicPlates();
        }
    }

    private void StartSpawning()
    {
        _isGameRunning = true;
    }

    private void StopSpawning()
    {
        _isGameRunning = false;
    }

    private void LaunchDynamicPlates()
    {
        _platesToTriggerDynamic = Mathf.Min(_platesToTriggerDynamic + 1, 8);

        var shuffledSpawnPoints = _dynamicSpawnPoints.OrderBy(x => Random.value).Take(dynamicPlatesPerLaunch).ToList();

        foreach (var spawnPoint in shuffledSpawnPoints)
        {
            spawnPoint.SpawnTarget();
        }

        AudioManagerSO.PlaySound(tossSound, transform.position, 0.8f);

        _destroyedPlatesCount = 0;
        _timeSinceLastDynamicLaunch = 0f;

        _currentDynamicRespawnDelay = Mathf.Max(5f, _currentDynamicRespawnDelay - 0.5f);
    }
}