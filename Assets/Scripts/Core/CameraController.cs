using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float edgeThreshold = 200f;
    [SerializeField] private Vector2 horizontalMinMax;
    [SerializeField] private float scrollSpeed = 5f;
    private Vector3 _position;
    private float _scaledEdgeThreshold;

    private void LateUpdate()
    {
        float halfWidth = cam.orthographicSize * cam.aspect;

        Vector2 mousePos = Input.mousePosition;
        _scaledEdgeThreshold = edgeThreshold * (Screen.width / 1920f);

        if (mousePos.x <= _scaledEdgeThreshold)
        {
            _position.x -= scrollSpeed * Time.deltaTime;
        }

        if (mousePos.x >= Screen.width - _scaledEdgeThreshold)
        {
            _position.x += scrollSpeed * Time.deltaTime;
        }

        _position.x = Mathf.Clamp(_position.x, horizontalMinMax.x + halfWidth, horizontalMinMax.y - halfWidth);
        _position.z = -10;

        transform.position = _position;
    }

    private void OnDrawGizmos()
    {
        Vector3 leftBorder = new(horizontalMinMax.x, 0f, 0f);
        Vector3 rightBorder = new(horizontalMinMax.y, 0f, 0f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(leftBorder, leftBorder + Vector3.up * 5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(rightBorder, rightBorder + Vector3.up * 5f);
    }
}
