using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    [SerializeField] private float smoothing;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform playerTransform;

    private void LateUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothing);
        smoothedPosition.y = 0;
        smoothedPosition.z = -10;

        transform.position = smoothedPosition;
    }
}