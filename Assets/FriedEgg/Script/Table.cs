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
    /// �e�[�u���ɒu����Ă��邩�m�F
    /// </summary>
    /// <returns></returns>
    public bool HasBowl()
    {
        return currentBowlState != BowlState.Empty; 
    }

    /// <summary>
    /// �e�[�u���ɂǂ�Ԃ��u��
    /// </summary>
    /// <param name="bowlState"></param>
    public void PlaceBowl(BowlState bowlState)
    {
        // ���łɒu����Ă���ꍇ
        if (HasBowl())
        {
            Debug.Log("�e�[�u���ɂ͂��łɘ����u����Ă��܂�");
            return;
        }

        currentBowlState = bowlState;
        Debug.Log($"���: {currentBowlState}");
        UpdateTableSprite(); 
    }

    /// <summary>
    /// �e�[�u������ǂ�Ԃ�����o��
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
        //Debug.Log($"�ǂ�Ԃ���: {bowlState}");
        UpdateTableSprite();
        return bowlState; // ���o�����ǂ�Ԃ�̏�Ԃ�Ԃ�
    }

    /// <summary>
    /// �X�v���C�g����Ԃɉ����čX�V
    /// </summary>
    private void UpdateTableSprite()
    {
        if (currentBowlState == BowlState.Empty)
        {
            spriteRenderer.sprite = null; // ��̏ꍇ�X�v���C�g���\��
        }
        else
        {
            spriteRenderer.sprite = stateSprites[(int)currentBowlState];
        }
    }
}
