using UnityEngine;

public class LoadingSceneRoot : MonoBehaviour
{
    private void Awake()
    {
        // このオブジェクト全体をシーン間で保持
        DontDestroyOnLoad(gameObject);

        // 重複インスタンスの削除
        if (FindObjectsOfType<LoadingSceneRoot>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
