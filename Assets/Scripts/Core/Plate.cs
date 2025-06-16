using UnityEngine;

public class Plate : MonoBehaviour, ITarget
{
    public event System.Action OnDestroy;

    [SerializeField] private Rigidbody2D rb;

    [Header("Effects")]
    [SerializeField] private ParticleSystem spawnParticles;
    [SerializeField] private ParticleSystem destroyParticles;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] shatterSounds;

    public void Spawn(Vector3 position)
    {
        rb.interpolation = RigidbodyInterpolation2D.None;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.position = position;

        spawnParticles.Play();
    }

    public void Spawn(Vector3 position, Vector3 direction, float force)
    {
        rb.velocity = Vector2.zero;

        rb.bodyType = RigidbodyType2D.Dynamic;

        transform.position = position;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.velocity = Mathf.Sqrt(-2f * Physics2D.gravity.y * force) * direction;

        spawnParticles.Play();
    }

    public void Hit(out int points)
    {
        points = 1;

        EffectsManager.Instance.Spawn(destroyParticles.gameObject, transform.position);
        AudioManagerSO.PlaySound(shatterSounds[Random.Range(0, shatterSounds.Length - 1)], transform.position, 0.8f);

        Destroy();
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -10)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        rb.interpolation = RigidbodyInterpolation2D.None;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        OnDestroy?.Invoke();
    }
}
