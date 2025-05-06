using UnityEngine;

public class DoorA : MonoBehaviour
{

    public Sprite openSprite;
    public Sprite closeSprite;
    private SpriteRenderer spriteRenderer; 

    private void Start()
    {  
        spriteRenderer = GetComponent<SpriteRenderer>();

        // èâä˙èÛë‘
        if (closeSprite != null)
        {
            spriteRenderer.sprite = closeSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            if (openSprite != null)
            {
                spriteRenderer.sprite = openSprite;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (closeSprite != null)
            {
                spriteRenderer.sprite = closeSprite;
            }
        }
    }
}
