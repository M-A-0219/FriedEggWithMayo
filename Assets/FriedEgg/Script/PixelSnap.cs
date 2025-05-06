using UnityEngine;

public class PixelSnap : MonoBehaviour
{
    public float pixelsPerUnit = 16f; // Pixel Perfect Camera�Ɠ����l��ݒ�

    void LateUpdate()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Round(position.x * pixelsPerUnit) / pixelsPerUnit;
        position.y = Mathf.Round(position.y * pixelsPerUnit) / pixelsPerUnit;
        transform.position = position;
    }
}
