using UnityEngine;

public class PixelSnap : MonoBehaviour
{
    public float pixelsPerUnit = 16f; // Pixel Perfect CameraÇ∆ìØÇ∂ílÇê›íË

    void LateUpdate()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x * pixelsPerUnit) / pixelsPerUnit;
        position.y = Mathf.Round(position.y * pixelsPerUnit) / pixelsPerUnit;
        transform.position = position;
    }
}
