using UnityEngine;

public class EggManager : MonoBehaviour
{
    private Transform player; 
    public float attractRadius = 3f; 
    public float initialMoveSpeed = 2f; 
    public float acceleration = 2f; 
    public Vector2 offset = new Vector2(0, 0.5f);

    public bool isAttracting = false;
    private float currentMoveSpeed;
    private PlayerController playerController; 

    private SpriteRenderer spriteRenderer;
    public Sprite defaultSprite; 
    public Sprite attractSprite;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    void Update()
    {
        if (player == null || !playerController.isHavingBasket)
        {
            StopAttraction();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attractRadius)
        {
            if (!isAttracting)
            {
                isAttracting = true;
                currentMoveSpeed = initialMoveSpeed; 

                if (spriteRenderer != null && attractSprite != null)
                {
                    spriteRenderer.sprite = attractSprite;
                }
            }
        }


        if (isAttracting)
        {

            Vector2 targetPosition = (Vector2)player.position + offset;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentMoveSpeed * Time.deltaTime);
            currentMoveSpeed += acceleration * Time.deltaTime;

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                playerController.AddEgg();
                Destroy(gameObject);
            }
        }
    }

    private void StopAttraction()
    {
        isAttracting = false;
        currentMoveSpeed = 0; 

        if (spriteRenderer != null && defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerController.isHavingBasket)
            {
                playerController.AddEgg(); 
                Destroy(gameObject); 
            }
        }
    }

   /* private void OnCollected()
    {
        playerController.AddEgg();
        Destroy(gameObject);
    }*/


    //デバッグ用
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractRadius);
    }
}
