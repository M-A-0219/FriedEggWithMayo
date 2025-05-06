using UnityEngine;

public class Activator : MonoBehaviour
{
    [Header("Target Object")]
    public GameObject targetObject; // アクティブ/非アクティブを切り替える対象

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // プレイヤーが範囲内に入った
        {
            if (targetObject != null)
            {
                targetObject.SetActive(true); // アクティブ化
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // プレイヤーが範囲外に出た
        {
            if (targetObject != null)
            {
                targetObject.SetActive(false); // 非アクティブ化
            }
        }
    }
}
