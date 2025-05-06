using UnityEngine;

public class Activator : MonoBehaviour
{
    [Header("Target Object")]
    public GameObject targetObject; // �A�N�e�B�u/��A�N�e�B�u��؂�ւ���Ώ�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // �v���C���[���͈͓��ɓ�����
        {
            if (targetObject != null)
            {
                targetObject.SetActive(true); // �A�N�e�B�u��
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // �v���C���[���͈͊O�ɏo��
        {
            if (targetObject != null)
            {
                targetObject.SetActive(false); // ��A�N�e�B�u��
            }
        }
    }
}
