using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using LoadingSystem; // ProgressManager を使用するために必要

public class LoadingSceneManager : MonoBehaviour
{
    public static LoadingSceneManager Instance;

    [Header("General Settings")]
    [Tooltip("ローディングシーンの名前 (デフォルト: LoadingScene)")]
    [SerializeField] private string loadingSceneName = "LoadingScene";

    [Tooltip("ローディングシーンの最低表示時間 (秒)")]
    [SerializeField] private float minDisplayTime = 1.5f;

    [Header("Progress Manager Settings")]
    [SerializeField] private ProgressManager progressManager; // ProgressManagerの参照

    [Header("Fade Settings")]
    [Tooltip("フェード用Animator")]
    [SerializeField] private Animator transitionAnimator;

    [Tooltip("フェードイン用のトリガーパラメータ名")]
    [SerializeField] private string fadeInTrigger = "FadeIn";

    [Tooltip("フェードアウト用のトリガーパラメータ名")]
    [SerializeField] private string fadeOutTrigger = "FadeOut";

    [Tooltip("ローディング中状態を示すトリガーパラメータ名")]
    [SerializeField] private string loadingTrigger = "Loading";

    /// <summary>
    /// シングルトンインスタンスの初期化
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // ProgressManagerが設定されていない場合、シーン内から探して設定
        if (progressManager == null)
        {
            progressManager = FindObjectOfType<ProgressManager>();
        }

        // ProgressManagerが設定されていない場合のエラーメッセージ
        if (progressManager == null)
        {
            Debug.LogError("LoadingSceneManager: ProgressManagerが設定されていません！");
        }
    }

    /// <summary>
    /// 指定されたシーンをロード
    /// </summary>
    /// <param name="targetScene">ロードするシーン名</param>
    public void LoadScene(string targetScene)
    {
        if (string.IsNullOrEmpty(loadingSceneName) || string.IsNullOrEmpty(targetScene))
        {
            Debug.LogError("LoadingSceneManager: シーン名が設定されていません！");
            return;
        }

        StartCoroutine(LoadSceneWithLoading(targetScene));
    }

    /// <summary>
    /// ローディングシーンを挟んでターゲットシーンをロード
    /// </summary>
    /// <param name="targetScene">ターゲットシーン名</param>
    private IEnumerator LoadSceneWithLoading(string targetScene)
    {
        // フェードインアニメーションを再生
        yield return StartCoroutine(PlayAnimation(fadeInTrigger));

        // ローディングシーンを非同期でロード
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync(loadingSceneName);
        yield return new WaitUntil(() => loadingSceneOp.isDone);

        // ローディングアニメーション状態を設定
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger(loadingTrigger);
        }

        // 次のシーンを非同期でロード
        AsyncOperation targetSceneOp = SceneManager.LoadSceneAsync(targetScene);
        targetSceneOp.allowSceneActivation = false;

        // ローディング中の進捗管理
        while (!targetSceneOp.isDone)
        {
            float progress = targetSceneOp.progress;

            // 進捗が0.9未満の時のみ進捗を更新
            if (progress < 0.9f)
            {
                progressManager.UpdateProgress(progress); // ProgressManagerに進捗を渡す
            }
            else
            {
                progressManager.UpdateProgress(1f); // 進捗が90%以上の時は1とみなす
            }

            if (progress >= 0.9f)
            {
                break;
            }

            yield return null;
        }

        // 最低表示時間を待つ
        yield return new WaitForSeconds(minDisplayTime);

        // 次のシーンをアクティブ化
        targetSceneOp.allowSceneActivation = true;

        // 次のフレームまで待機してシーンを完全に切り替える
        yield return null;

        // フェードアウトアニメーションを再生
        yield return StartCoroutine(PlayAnimation(fadeOutTrigger));
    }

    /// <summary>
    /// 指定されたアニメーショントリガーを再生
    /// </summary>
    /// <param name="trigger">Animator トリガー名</param>
    private IEnumerator PlayAnimation(string trigger)
    {
        if (transitionAnimator == null || string.IsNullOrEmpty(trigger)) yield break;

        transitionAnimator.SetTrigger(trigger);
        yield return new WaitForSeconds(1f); // アニメーションの再生時間に合わせて調整
    }
}
