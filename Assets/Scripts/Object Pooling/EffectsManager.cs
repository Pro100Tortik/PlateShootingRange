using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : Singleton<EffectsManager>
{
    private Dictionary<GameObject, PoolBase<GameObject>> _pools = new();

    [SerializeField] private int defaultPreload = 5;
    [SerializeField] private bool defaultExpandable = true;

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation = default)
    {
        if (!_pools.TryGetValue(prefab, out var pool))
        {
            Register(prefab, defaultPreload, defaultExpandable);
            pool = _pools[prefab];
        }

        var obj = pool.Get();
        obj.transform.SetPositionAndRotation(position, rotation);

        StartCoroutine(ReturnGameObject(obj));

        return obj;
    }

    public void Despawn(GameObject obj)
    {
        foreach (var pool in _pools.Values)
        {
            if (pool == null) continue;
            pool.Return(obj);
            return;
        }

        Debug.LogWarning("Trying to return object that doesn't belong to any pool.");
    }

    public void Register(GameObject prefab, int preloadCount, bool expandable = true)
    {
        if (_pools.ContainsKey(prefab))
            return;

        // Create Tab
        var tab = new GameObject(prefab.name);
        tab.transform.parent = transform;

        var pool = new PoolBase<GameObject>(
            preloadFunc: () =>
            {
                var go = Instantiate(prefab, tab.transform);
                go.SetActive(false);

                var particleSystem = go.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    var autoReturn = go.AddComponent<AutoReturnParticleSystem>();
                    autoReturn.Set(particleSystem, () => Despawn(go));
                }

                return go;
            },
            getAction: go => go.SetActive(true),
            returnAction: go => go.SetActive(false),
            preloadCount: preloadCount,
            canExpand: expandable
        );

        _pools[prefab] = pool;
    }

    private IEnumerator ReturnGameObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(5f);
        Despawn(gameObject);
        yield return null;
    }
}
