using UnityEngine;

public class Table : MonoBehaviour
{
    [Header("Settings")]
    public Sprite[] stateSprites; 

    [Header("References")]
    private SpriteRenderer spriteRenderer;

    private BowlState currentBowlState = BowlState.Empty;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateTableSprite();
    }

    /// <summary>
    /// テーブルに置かれているか確認
    /// </summary>
    /// <returns></returns>
    public bool HasBowl()
    {
        return currentBowlState != BowlState.Empty; 
    }

    /// <summary>
    /// テーブルにどんぶりを置く
    /// </summary>
    /// <param name="bowlState"></param>
    public void PlaceBowl(BowlState bowlState)
    {
        // すでに置かれている場合
        if (HasBowl())
        {
            Debug.Log("テーブルにはすでに丼が置かれています");
            return;
        }

        currentBowlState = bowlState;
        Debug.Log($"状態: {currentBowlState}");
        UpdateTableSprite(); 
    }

    /// <summary>
    /// テーブルからどんぶりを取り出す
    /// </summary>
    /// <returns></returns>
    public BowlState TakeBowl()
    {
        if (!HasBowl())
        {
            return BowlState.Empty;
        }

        BowlState bowlState = currentBowlState; 
        currentBowlState = BowlState.Empty;
        //Debug.Log($"どんぶり状態: {bowlState}");
        UpdateTableSprite();
        return bowlState; // 取り出したどんぶりの状態を返す
    }

    /// <summary>
    /// スプライトを状態に応じて更新
    /// </summary>
    private void UpdateTableSprite()
    {
        if (currentBowlState == BowlState.Empty)
        {
            spriteRenderer.sprite = null; // 空の場合スプライトを非表示
        }
        else
        {
            spriteRenderer.sprite = stateSprites[(int)currentBowlState];
        }
    }
}
