using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public int coinReward = 100;
    public int experienceReward = 10;

    private LevelManager levelManager;
    private CoinManager coinManager;
    private Animator animator;

    private Vector2 spawnPosition = new Vector2(-3, 18);
    private Vector2 exitPosition1 = new Vector2(-4, 11.625f);
    private Vector2 exitPosition2 = new Vector2(-4, 18);

    private bool isWaiting = false;
    public bool isExiting = false;
    public float moveSpeed = 2.0f;
    private Vector3 targetPosition;
    private NPCManager npcManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
        coinManager = FindObjectOfType<CoinManager>();
        animator = GetComponent<Animator>();
        Debug.Log($"[NPC Start] exitPosition1: {exitPosition1}, exitPosition2: {exitPosition2}");
        transform.position = spawnPosition;
    }

    public void SetNPCManager(NPCManager manager)
    {
        npcManager = manager;
    }

    public void SetTargetPosition(Vector3 position, bool isFirstInQueue)
    {
        if (isExiting) return;
        targetPosition = position;
        isWaiting = isFirstInQueue;
        Debug.Log($"[SetTargetPosition] TargetPosition: {targetPosition}, IsFirstInQueue: {isFirstInQueue}");

        // 移動を強制開始（待機状態でない場合のみ）
        if (!isWaiting)
        {
            StartMovingImmediately();
        }
    }

    private void StartMovingImmediately()
    {
        // Animator の遅延初期化
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("[NPC] Animator コンポーネントが見つかりません。");
                return;
            }
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        animator.SetFloat("MoveX", Mathf.Round(direction.x));
        animator.SetFloat("MoveY", Mathf.Round(direction.y));
        animator.SetBool("isMoving", true);
    }

    private void Update()
    {
        if (isExiting) return;

        if (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            // 丸め処理を追加して方向を設定
            float moveX = Mathf.Round(direction.x);
            float moveY = Mathf.Round(direction.y);

            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
            animator.SetBool("isMoving", true);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            //Debug.Log($"[Update] Moving to Target: {targetPosition}, Current Position: {transform.position}, MoveX: {moveX}, MoveY: {moveY}");
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    public void OnPurchaseComplete()
    {
        if (isWaiting && !isExiting)
        {
            isExiting = true;     // ★ ここでフラグ立て
            isWaiting = false;
            coinManager.AddCoins(coinReward);
            levelManager.AddExperience(experienceReward);

            StartCoroutine(StartExitSequence()); // ★ UpdateQueueはここで呼ばない
        }
    }



    private IEnumerator StartExitSequence()
    {
        Debug.Log("NPCの退場シーケンスを開始します。");
        float originalSpeed = moveSpeed; // 現在の速度を保存
        moveSpeed *= 3;

        // 退場位置1（左方向）への移動
        yield return MoveToPosition(exitPosition1, () =>
        {
            Debug.Log("NPCが退場位置1に到達しました。次に進みます。");
        });

        // 退場位置2（上方向）への移動
        yield return MoveToPosition(exitPosition2, () =>
        {
            npcManager.OnNPCFinished(this); 
            Destroy(gameObject);
        });

        moveSpeed = originalSpeed;
    }

    private IEnumerator MoveToPosition(Vector3 position, System.Action onComplete)
    {
        Debug.Log($"NPCが移動を開始: 現在位置: {transform.position}, ターゲット位置: {position}, moveSpeed: {moveSpeed}");
        while ((transform.position - position).sqrMagnitude > 0.0001f)
        {
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
            Vector3 direction = (position - transform.position).normalized;

            // 丸め処理を追加して方向を設定
            float moveX = Mathf.Round(direction.x);
            float moveY = Mathf.Round(direction.y);

            animator.SetFloat("MoveX", moveX);
            animator.SetFloat("MoveY", moveY);
            animator.SetBool("isMoving", true);

            //Debug.Log($"[MoveToPosition] Current: {transform.position}, Next: {nextPosition}, Target: {position}, MoveDelta: {Vector3.Distance(transform.position, nextPosition)}, Direction: {direction}");
            transform.position = nextPosition;
            yield return null;
        }
        transform.position = position;
        Debug.Log($"[MoveToPosition] Reached Target: {position}, Final Position: {transform.position}");
        animator.SetBool("isMoving", false);
        onComplete?.Invoke();
    }
}
