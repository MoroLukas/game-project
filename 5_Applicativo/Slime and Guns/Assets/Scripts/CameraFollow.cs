using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform target;         // Il personaggio da seguire
    public Vector3 offset;           // Spostamento rispetto al personaggio
    public float smoothSpeed = 0.125f; 

    void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z 
        );

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}