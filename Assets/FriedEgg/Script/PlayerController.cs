using System.Collections;
using TMPro;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public bool isHavingBasket = false;
    public bool isHavingBowl = false; // どんぶりを持っているか
    public BowlState currentBowlState = BowlState.Empty; // 現在のどんぶりの状態
    public int eggCount = 0;

    public LayerMask interactableLayer; // 対象物レイヤー
    [Header("Input Settings")]
    public float inputHoldDelay = 0.2f; // 長押し時の処理間隔

    private float holdTimer = 0f; // 長押し処理用のタイマー
    private bool isHoldingLeftClick = false; // 右クリックを長押し中かどうか
    [Header("Interaction Settings")]
    public float interactionDistance = 2.0f; // インタラクト可能距離
    public float interactionAngle = 45f; // 視界の角度（半分の角度）

    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;

    [Header("Audio Settings")]
    public AudioClip saleSound;
    private AudioSource audioSource;

    [Header("UI Elements")]
    public TMP_Text eggCountText;

    private Vector2 lastDirection = Vector2.down; // プレイヤーが最後に向いた方向（PlayerMovementから取得）
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        UpdateEggCountUI();
    }

    void Update()
    {
        HandleInput();
        UpdateAnimatorParameters();

        if (Input.GetKey(KeyCode.Space)&&(!isHavingBowl))
        {
            isHavingBasket = true;
        }
        else if (isHavingBasket && eggCount == 0) // 卵がない場合
        {
            isHavingBasket = false;
        }

        animator.SetBool("isHavingBasket", isHavingBasket);

        // 卵の数に応じてアニメーションを切り替え
        if (eggCount < 3)
        {
            animator.SetInteger("Egg", eggCount);
        }
        else
        {
            animator.SetInteger("Egg", 3); // 3以上は最大値
        }
        // 長押し処理（右クリック）
        if (isHoldingLeftClick)
        {
            holdTimer -= Time.deltaTime;
            if (holdTimer <= 0f)
            {
                holdTimer = inputHoldDelay; // タイマーをリセット

                // 長押し中にマヨネーズマシンに卵を投入
                Collider2D target = FindClosestInteractable();
                if (target != null && target.CompareTag("MayonnaiseMachine"))
                {
                    MayonnaiseMachine machine = target.GetComponent<MayonnaiseMachine>();
                    if (eggCount > 0) // 卵がある場合のみ投入
                    {
                        machine.AddEgg(this); // マシンに卵を追加
                        Debug.Log($"卵を長押しで追加しました！ 現在の卵数: {eggCount}");
                        UpdateEggCountUI();
                    }
                    else
                    {
                        Debug.Log("卵がありません！");
                        isHoldingLeftClick = false; // 卵がなくなったら長押しを解除
                    }
                }
            }
        }
    }
    public void SetLastDirection(Vector2 direction)
    {
        lastDirection = direction; // PlayerMovementから方向をセットする
    }
    public void AddEgg()
    {
        eggCount++;
        Debug.Log("卵を拾いました。現在の卵数: " + eggCount);

        UpdateEggCountUI();
    }
    private void UpdateEggCountUI()
    {
        if (eggCountText != null)
        {
            eggCountText.text = $": {eggCount}"; // テキストを更新
        }
    }
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")) // 左クリック
        {
            if (!isHoldingLeftClick) // 長押し中でない場合のみ単押しを処理
            {
                Debug.Log("左クリックが押されました");
                InteractWithLeftClick();
            }
        }
        else if (Input.GetMouseButton(0) || Input.GetButton("Submit")) // 左クリック長押し
        {
            if (!isHoldingLeftClick) // 長押し開始時
            {
                isHoldingLeftClick = true;
                holdTimer = 0f; // タイマーをリセット
            }
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Submit")) // 左クリックを離したとき
        {
            if (isHoldingLeftClick)
            {
                isHoldingLeftClick = false; // 長押しを解除
            }
        }
    }




   /* private void InteractWithRightClick()
    {
        Debug.Log("右クリックが押されました");

        Collider2D target = FindClosestInteractable();
        if (target == null)
        {
            Debug.Log("右クリック処理: 対象物が見つかりませんでした");
            return;
        }

        Debug.Log("右クリック処理: 対象物 " + target.name + " を検出しました");

        if (target.CompareTag("FryingPan") && eggCount > 0) // フライパンに卵を使用
        {
            FryingPan fryingPan = target.GetComponent<FryingPan>();

            // フライパンが使用可能（空の状態）か確認
            if (fryingPan != null && fryingPan.IsReadyToCook())
            {
                Debug.Log("フライパンを発見し、卵を使用します");
                eggCount--; // 卵を1つ減らします
                fryingPan.StartCooking(); // フライパンの調理を開始

                Debug.Log("卵を使用しました。現在の卵数: " + eggCount);
                UpdateEggCountUI();
            }
        }
        else if (target.CompareTag("FryingPan"))
        {
            Debug.Log("フライパンは検出されたが、卵がありません");
        }
        else
        {
            Debug.Log("右クリック処理: フライパンまたはマヨネーズマシン以外の対象物が検出されました");
        }
    }*/



    private void InteractWithLeftClick()
    {
        Collider2D target = FindClosestInteractable();
        if (target == null) return;


        if (target.CompareTag("FryingPan") && eggCount > 0) // フライパンに卵を使用
        {
            FryingPan fryingPan = target.GetComponent<FryingPan>();

            // フライパンが使用可能（空の状態）か確認
            if (fryingPan != null && fryingPan.IsReadyToCook())
            {
                Debug.Log("フライパンを発見し、卵を使用します");
                eggCount--; // 卵を1つ減らします
                fryingPan.StartCooking(); // フライパンの調理を開始

                Debug.Log("卵を使用しました。現在の卵数: " + eggCount);
                UpdateEggCountUI();
            }
        }
        else if (target.CompareTag("FryingPan"))
        {
            Debug.Log("フライパンは検出されたが、卵がありません");
        }

        // マヨネーズマシンとの相互作用
        if (target.CompareTag("MayonnaiseMachine"))
        {
            MayonnaiseMachine machine = target.GetComponent<MayonnaiseMachine>();

            // プレイヤーが2段階目のどんぶりを持っている場合はアップグレードを試みる
            if (isHavingBowl && currentBowlState == BowlState.RiceWithEgg)
            {
                machine.Interact(this, false); // false: 単押し
            }
            if(isHavingBowl && currentBowlState == BowlState.Rice)
            {
                ActivateArrow2();
            }
            if (!isHavingBowl)
            {
                ActivateArrow1();
            }
            return;
        }

        // フライパンとの相互作用
        if (target.CompareTag("FryingPan"))
        {
            FryingPan fryingPan = target.GetComponent<FryingPan>();
            fryingPan.Interact(this);
            return; // 処理終了
        }

        // 炊飯器との相互作用
        if (target.CompareTag("RiceCooker"))
        {
            RiceCooker riceCooker = target.GetComponent<RiceCooker>();
            // 炊飯中または空でない場合の操作制限
            if (!riceCooker.IsEmpty() && riceCooker.IsCooking())
            {
                Debug.Log("炊飯中のため操作できません！");
                return;
            }
            // ご飯を炊き始める場合
            if (riceCooker.IsEmpty())
            {
                riceCooker.StartCookingRice(); // 炊飯器で米を炊き始める
                Debug.Log("炊飯器で米を炊き始めました！");
                return;
            }

            // ご飯を取り出す場合
            if (!isHavingBasket && !isHavingBowl) // 条件を明確化
            {
                riceCooker.TakeRice(); // 炊飯器からご飯を取り出す
                isHavingBowl = true;
                currentBowlState = BowlState.Rice; // 1段階目を取得
                Debug.Log("炊飯器からご飯を取りました！ どんぶり状態: " + currentBowlState);
                return;
            }

            // どんぶりやかごを持っている場合
            Debug.Log("すでにどんぶりやかごを持っているため、ご飯を取り出せません！");
        }

        // テーブルとの相互作用
        if (target.CompareTag("Table"))
        {
            Table table = target.GetComponent<Table>();

            if (isHavingBowl) // プレイヤーがどんぶりを持っている場合
            {
                if (table.HasBowl()) // テーブルにどんぶりがすでにある場合
                {
                    Debug.Log("テーブルにはすでにどんぶりが置かれています！ どんぶりを置けません！");
                    return;
                }

                // テーブルにどんぶりを置く
                table.PlaceBowl(currentBowlState);
                isHavingBowl = false;
                currentBowlState = BowlState.Empty; // プレイヤーのどんぶりを空に
                Debug.Log("テーブルにどんぶりを置きました！");
            }
            else if (table.HasBowl()&&!isHavingBasket) // プレイヤーがどんぶりを持っていない場合、テーブルからどんぶりを取る
            {
                isHavingBowl = true;
                currentBowlState = table.TakeBowl(); // テーブルからどんぶりを取得
                Debug.Log("テーブルからどんぶりを取りました！ どんぶり状態: " + currentBowlState);
            }
        }

        // NPCとの相互作用
        if (target.CompareTag("NPC"))
        {
            NPC npc = target.GetComponent<NPC>();

            // NPCにどんぶりを販売
            if (isHavingBowl && currentBowlState == BowlState.Complete)
            {
                // プレイヤーのどんぶりをリセット
                isHavingBowl = false;
                currentBowlState = BowlState.Empty;

                // NPCの購入処理を呼び出し
                npc.OnPurchaseComplete();

                Debug.Log("どんぶりを販売しました！");
                PlayAudio(saleSound);
            }
            if (isHavingBowl && currentBowlState == BowlState.RiceWithEgg)
            {
                ActivateArrow3();
            }
            if (isHavingBowl && currentBowlState == BowlState.Rice)
            {
                ActivateArrow2();
            }
            /*if (!isHavingBowl)
            {
                ActivateArrow1();
            }*/
            return; // 処理終了
        }

    }



    private Collider2D FindClosestInteractable()
    {
        // プレイヤーの基準点を一時的に上に変更
        Vector3 pivotOffset = new Vector3(0, 1.0f, 0); // プレイヤーの高さに応じて調整
        Vector3 pivotPosition = transform.position + pivotOffset;

        // オーバーラップサークルでインタラクション可能なオブジェクトを取得
        Collider2D[] hits = Physics2D.OverlapCircleAll(pivotPosition, interactionDistance, interactableLayer);

        Collider2D closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            // 自分自身のコライダーを無視（重なりを回避）
            if (hit.gameObject == gameObject)
            {
                continue;
            }

            // プレイヤーからターゲットへの方向を計算
            Vector2 directionToTarget = (hit.transform.position - pivotPosition).normalized;

            // プレイヤーの視界角度内にあるかを判定
            float angle = Vector2.Angle(lastDirection, directionToTarget);
            if (angle <= interactionAngle)
            {
                // ターゲットまでの距離を計算
                float distance = (hit.transform.position - pivotPosition).sqrMagnitude;

                // 最も近いオブジェクトを更新
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = hit;
                }
            }
        }

        return closest;
    }




    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }


    private void UpdateAnimatorParameters()
    {
        animator.SetBool("isHavingBasket", isHavingBasket);
        animator.SetBool("isHavingBowl", isHavingBowl);

        // 卵の数に応じてアニメーションを切り替え
        if (eggCount < 3)
        {
            animator.SetInteger("Egg", eggCount);
        }
        else
        {
            animator.SetInteger("Egg", 3); // 3以上は最大値
        }
        // 丼の状態をアニメーションに反映
        animator.SetInteger("Bowl", (int)currentBowlState);
    }

    public void ActivateArrow1()
    {
        StartCoroutine(ActivateAndDeactivateArrow(arrow1));
    }

    public void ActivateArrow2()
    {
        StartCoroutine(ActivateAndDeactivateArrow(arrow2));
    }

    public void ActivateArrow3()
    {
        StartCoroutine(ActivateAndDeactivateArrow(arrow3));
    }

    private IEnumerator ActivateAndDeactivateArrow(GameObject arrow)
    {
        if (arrow != null)
        {
            arrow.SetActive(true); // アクティブ化
            yield return new WaitForSeconds(3f); // 3秒待機
            arrow.SetActive(false); // 非アクティブ化
        }
    }

}
