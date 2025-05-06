using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class FadingSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Range(0f, 1f)] public float fadeToAlpha = 0.5f;
    public float fadeSpeed = 2f; 

    private float targetAlpha = 1f;
    private float alpha;

    private void Awake()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        alpha = spriteRenderer.color.a;
    }

    private void Update()
    {

        alpha = Mathf.MoveTowards(alpha, targetAlpha, fadeSpeed * Time.deltaTime);

        var color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetAlpha = fadeToAlpha;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetAlpha = 1f;
        }
    }
}
