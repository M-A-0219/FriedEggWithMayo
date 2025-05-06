using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitle : MonoBehaviour
{
    [Header("設定: タイトルシーンの名前")]
    public string titleSceneName = "TitleScene"; // タイトルシーンの名前

    void Update()
    {
        // Escキーが押されたとき
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToTitleScene(); // タイトルシーンに戻る
        }
    }

    /// <summary>
    /// タイトルシーンに戻る処理
    /// </summary>
    private void ReturnToTitleScene()
    {
        if (!string.IsNullOrEmpty(titleSceneName))
        {
            SceneManager.LoadScene(titleSceneName); // 指定されたシーンをロード
        }
        else
        {
            Debug.LogError("タイトルシーン名が設定されていません！");
        }
    }
}
