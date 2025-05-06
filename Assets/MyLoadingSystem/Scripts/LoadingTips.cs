using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace LoadingSystem
{
    public class LoadingTips : MonoBehaviour
    {
        [System.Serializable]
        public class GameTip
        {
            public string category; // カテゴリやタイトル用
            public string tip; // ヒント
            public Sprite tipImage; // ヒントに関連する画像（オプション）
        }

        [Header("Tips List")]
        [Tooltip("ローディングシーンの名前 (デフォルト: LoadingScene)")]
        [SerializeField] private GameTip[] tips;

        [Header("Settings")]
        [Tooltip("ローディングシーンの名前 (デフォルト: LoadingScene)")]
        [SerializeField] private float timeBetweenSwitch = 5f;
        [Header("Category")]
        [Tooltip("カテゴリやタイトル用")]
        [SerializeField] private TMP_Text categoryText;
        [Header("TipText")]
        [SerializeField] private TMP_Text tipText;
        [Header("Image")]
        [SerializeField] private Image tipImage;

        private int currentTipIndex = -1;
        private int lastTipIndex = -1;

        [Header("Switching Control")]
        [Tooltip("切り替えのオン/オフを管理")]
        [SerializeField] private bool isTipSwitchingEnabled = true; 

        void Start()
        {
            // 初回表示（ランダムにTipを表示）
            SwitchTip();

            // 時間経過での切り替えが有効な場合、コルーチンを開始
            if (isTipSwitchingEnabled)
            {
                StartCoroutine(SwitchTipCoroutine());
            }
        }

        void Update()
        {
            // 切り替えが有効で、クリックでの切り替えが可能な場合
            if (isTipSwitchingEnabled && Input.GetMouseButtonDown(0))
            {
                SwitchTip();
            }
        }

        /// <summary>
        /// ヒントを切り替え、次のヒントを表示します。
        /// </summary>
        private void SwitchTip()
        {
            int newTipIndex = GetRandomTipIndex();

            // 同じヒントが連続しないようにする
            while (newTipIndex == lastTipIndex && tips.Length > 1)
            {
                newTipIndex = GetRandomTipIndex();
            }

            lastTipIndex = newTipIndex;
            currentTipIndex = newTipIndex;

            ShowTip();
        }

        /// <summary>
        /// ランダムにヒントのインデックスを取得します。
        /// </summary>
        private int GetRandomTipIndex()
        {
            return Random.Range(0, tips.Length);
        }

        /// <summary>
        /// 現在のヒントを表示します。
        /// </summary>
        private void ShowTip()
        {
            GameTip currentTip = tips[currentTipIndex];

            // Tipが空の場合はエラー回避
            tipText.text = !string.IsNullOrEmpty(currentTip.tip) ? currentTip.tip : "Tip Unavailable";

            // Categoryが空の場合はエラー回避
            if (categoryText != null)
            {
                categoryText.text = !string.IsNullOrEmpty(currentTip.category) ? currentTip.category : "Category Unavailable";
            }

            // 画像の有無で表示/非表示を切り替える
            if (tipImage != null)
            {
                if (currentTip.tipImage != null)
                {
                    tipImage.sprite = currentTip.tipImage;
                    tipImage.gameObject.SetActive(true);
                }
                else
                {
                    tipImage.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 一定時間ごとにヒントを切り替えるコルーチン。
        /// </summary>
        private IEnumerator SwitchTipCoroutine()
        {
            while (isTipSwitchingEnabled)
            {
                yield return new WaitForSeconds(timeBetweenSwitch);
                SwitchTip();
            }
        }

        /// <summary>
        /// 切り替えを無効化する（初回表示のみ行う）
        /// </summary>
        public void DisableTipSwitching()
        {
            isTipSwitchingEnabled = false;
            StopAllCoroutines(); // コルーチンを停止
        }

        /// <summary>
        /// 切り替えを有効化する（時間経過およびクリックで切り替え可能にする）
        /// </summary>
        public void EnableTipSwitching()
        {
            isTipSwitchingEnabled = true;
            StartCoroutine(SwitchTipCoroutine()); // コルーチンを再開
        }
    }
}
