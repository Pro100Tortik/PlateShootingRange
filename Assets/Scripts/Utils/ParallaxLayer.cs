using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField, Range(0f, 1f)] private float parallaxFactor = 0.5f;
    [SerializeField] private Vector2 horizontalMinMax;
    private Vector3 _previousCameraPosition;

    private void Start()
    {
        _previousCameraPosition = cam.transform.position;
    }

    private void LateUpdate()
    {
        if (parallaxFactor < 0.1f)
            return;

        Vector3 delta = cam.transform.position - _previousCameraPosition;
        transform.position += new Vector3(delta.x * parallaxFactor, delta.y, 0f);
        _previousCameraPosition = cam.transform.position;
    }

    private void OnDrawGizmos()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (!sr || !cam) return;

        Vector2 spriteHalfSize = sr.bounds.extents;

        // Размер камеры
        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        // Позиции камеры
        float camMinX = horizontalMinMax.x;
        float camMaxX = horizontalMinMax.y;

        // Максимальное смещение камеры
        float cameraMoveRange = camMaxX - camMinX;

        // Позиции, которых может достичь parallax-объект
        float parallaxMoveRange = cameraMoveRange * parallaxFactor;

        // Центр parallaxLayer ограничивается так, чтобы его края оставались видимыми
        float minX = camMinX + camHalfWidth - spriteHalfSize.x + parallaxMoveRange;
        float maxX = camMaxX - camHalfWidth + spriteHalfSize.x - parallaxMoveRange;

        // Нарисуем вертикальные линии границ
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(minX, transform.position.y - 10f, 0f), new Vector3(minX, transform.position.y + 10f, 0f));
        Gizmos.DrawLine(new Vector3(maxX, transform.position.y - 10f, 0f), new Vector3(maxX, transform.position.y + 10f, 0f));
    }
}
