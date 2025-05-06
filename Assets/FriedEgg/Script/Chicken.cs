using System.Collections;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float moveSpeed = 2f; // 移動速度を一定に保つ

    public float minStopTime = 1f; // 停止する最小時間
    public float maxStopTime = 3f; // 停止する最大時間
    public float minMoveTime = 2f; // 移動する最小時間
    public float maxMoveTime = 4f; // 移動する最大時間

    public GameObject eggPrefab;
    public float eggDropChance = 0.1f; // 卵を落とす確率

    private PolygonCollider2D boundaryCollider; // 移動範囲を示すコライダー
    private Vector2 moveDirection; // 移動方向
    private float actionTimer; // 状態切り替えのタイマー
    private SpriteRenderer spriteRenderer; // スプライトの向きを制御する
    private Animator animator; // アニメーション制御
    private Rigidbody2D rb; // 物理挙動を制御
    private bool isTouchingWall = false; // 壁に触れているかどうか
    private Vector2 lastBlockedDirection = Vector2.zero;

    private enum ChickenState
    {
        Stopped,
        Moving
    }

    private ChickenState currentState;

    void Start()
    {
        // 必要なコンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Boundary（移動範囲）の取得
        GameObject boundaryObject = GameObject.FindWithTag("Boundary");
        if (boundaryObject != null)
        {
            boundaryCollider = boundaryObject.GetComponent<PolygonCollider2D>();
        }
        else
        {
            Debug.LogError("Boundary Collider が見つかりません！BoundaryオブジェクトにTagを設定してください。");
        }

        // ランダムな初期タイマーを設定
        actionTimer = Random.Range(minStopTime, maxStopTime);

        // 初期状態を停止に設定
        ChangeState(ChickenState.Stopped);
        StartCoroutine(EggRoutine());
    }

    void Update()
    {
        // タイマーを減少
        actionTimer -= Time.deltaTime;

        if (actionTimer <= 0)
        {
            if (currentState == ChickenState.Stopped)
            {
                StartMoving(); // 停止から移動に切り替え
            }
            else if (currentState == ChickenState.Moving)
            {
                StopMoving(); // 移動から停止に切り替え
            }
        }

        // 移動処理
        if (currentState == ChickenState.Moving)
        {
            Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.deltaTime;

            // 壁に触れていない場合のみ移動
            if (!isTouchingWall && IsInsideBoundary(newPosition))
            {
                rb.MovePosition(newPosition);
            }
            else
            {
                // 壁に当たったら停止
                StopMoving();
            }

            // スプライトの向きを設定
            spriteRenderer.flipX = moveDirection.x > 0;
        }
    }

    void StartMoving()
    {
        // 移動方向をランダムに設定
        float randomX, randomY;
        do
        {
            randomX = Random.Range(-1f, 1f);
            randomY = Random.Range(-1f, 1f);
            moveDirection = new Vector2(randomX, randomY).normalized;
        } while (Mathf.Abs(randomX) < 0.1f && Mathf.Abs(randomY) < 0.1f); // 小さすぎる値を防ぐ

        moveDirection = new Vector2(randomX, randomY).normalized; // 正規化して一定の方向ベクトルを作成

        // 壁に向かっていく場合は方向を反転
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.deltaTime;
        if (!IsInsideBoundary(newPosition))
        {
            moveDirection = -moveDirection; // 移動方向を反転
        }

        // 次のアクションまでの時間を設定
        actionTimer = Random.Range(minMoveTime, maxMoveTime);


        // アニメーションを移動状態に設定
        animator.SetBool("Moving", true);

        ChangeState(ChickenState.Moving);
    }

    void StopMoving()
    {
        // 移動方向をリセット
        moveDirection = Vector2.zero;

        // 次のアクションまでの時間を設定
        actionTimer = Random.Range(minStopTime, maxStopTime);

        // アニメーションを停止状態に設定
        animator.SetBool("Moving", false);

        ChangeState(ChickenState.Stopped);
    }

    bool IsInsideBoundary(Vector3 position)
    {
        if (boundaryCollider == null)
        {
            Debug.LogWarning("Boundary Collider が設定されていません！");
            return true; // Boundaryがない場合は範囲内とみなす
        }

        return boundaryCollider.OverlapPoint(position);
    }

    void DropEgg()
    {
        if (eggPrefab != null)
        {
            Instantiate(eggPrefab, transform.position, Quaternion.identity);
        }
    }

    void ChangeState(ChickenState newState)
    {
        currentState = newState;
    }

    bool IsNearBoundary(Vector2 position, float margin = 0.2f)
    {
        if (boundaryCollider == null)
            return false;

        // ちょっと内側にずらした4方向をチェック
        Vector2[] checkOffsets = new Vector2[]
        {
        Vector2.left * margin,
        Vector2.right * margin,
        Vector2.up * margin,
        Vector2.down * margin
        };

        foreach (var offset in checkOffsets)
        {
            if (!boundaryCollider.OverlapPoint(position + offset))
            {
                return true; // どれかが範囲外なら「近い」とみなす
            }
        }

        return false; // 全部範囲内ならOK（つまり端ではない）
    }

    IEnumerator EggRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 8f));

            if (!IsNearBoundary(transform.position) && Random.value < eggDropChance)
            {
                DropEgg();
            }
        }
    }
    // 壁との衝突検出
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true; // 壁に触れているフラグを立てる
            StopMoving(); // 壁に触れたら停止
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false; // 壁から離れたらフラグを下げる
            lastBlockedDirection = moveDirection; // この方向はダメだった
            StopMoving();
        }
    }
}
