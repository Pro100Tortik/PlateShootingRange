using UnityEngine;

public class TargetSpawnpoint : MonoBehaviour
{
    public bool IsOccupied { get; set; } = false;

    [field: SerializeField] public bool LaunchTarget { get; private set; } = false;
    [SerializeField, Range(1f, 20f)] private float launchForce = 7f;
    private Plate _currentTarget;
    private float _respawnTime = 7f;

    public void SetTarget(Plate plate)
    {
        _currentTarget = plate;

        _currentTarget.OnDestroy += HandleTargetDestruction;
    }

    public void SpawnTarget()
    {
        _currentTarget.gameObject.SetActive(true);

        if (LaunchTarget == true)
        {
            _currentTarget.Spawn(transform.position, transform.right, launchForce);
        }
        else
        {
            _currentTarget.Spawn(transform.position);
        }
    }

    private void HandleTargetDestruction()
    {
        _currentTarget.gameObject.SetActive(false);

        // This used by static respawns
        if (LaunchTarget == false)
        {
            Invoke(nameof(SpawnTarget), _respawnTime);

            // Decrease respawn time
            _respawnTime = Mathf.Max(3.5f, _respawnTime - 0.5f);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Draw plate preview
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        if (LaunchTarget == false)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * launchForce);
    }
#endif
}
