using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace LoadingSystem
{
    /// <summary>
    /// ローディング画面の進捗表示を管理するクラス。
    /// 進行状況に応じてプログレスバー、サークルゲージ、複数のイメージ、またはパーセンテージを更新します。
    /// </summary>
    public class ProgressManager : MonoBehaviour
    {
        [Header("Progress Bar Settings")]
        [Tooltip("プログレスバーを使用するかどうか")]
        [SerializeField] private bool useProgressBar = false;

        [Tooltip("プログレスバーのImageコンポーネント")]
        [SerializeField] private Image progressBar;

        [Header("Circular Gauge Settings")]
        [Tooltip("円形ゲージを使用するかどうか")]
        [SerializeField] private bool useCircularGauge = false;

        [Tooltip("円形ゲージのImageコンポーネント")]
        [SerializeField] private Image circularGauge;

        [Header("Multiple Image Progress Settings")]
        [Tooltip("複数のイメージを利用して進捗を表示するかどうか")]
        [SerializeField] private bool useMultipleImageProgress = false;

        [Tooltip("複数の進捗表示用Imageコンポーネントのリスト")]
        [SerializeField] private List<Image> progressImages = new List<Image>();

        [Header("Progress Percentage Settings")]
        [Tooltip("進捗をパーセンテージで表示するかどうか")]
        [SerializeField] private bool usePercentage = false;

        [Tooltip("レガシーUIテキスト (Textコンポーネント)")]
        [SerializeField] private Text legacyText;

        [Tooltip("TextMeshProのテキストコンポーネント")]
        [SerializeField] private TMP_Text tmpText;

        /// <summary>
        /// 進捗状況を更新します。
        /// </summary>
        /// <param name="progress">進行状況 (0～1)</param>
        public void UpdateProgress(float progress)
        {
            // プログレスバーを更新
            if (useProgressBar && progressBar != null)
            {
                UpdateProgressBarSmoothly(progressBar, progress, 5f);
            }

            // 円形ゲージを更新
            if (useCircularGauge && circularGauge != null)
            {
                UpdateProgressBarSmoothly(circularGauge, progress, 5f);
            }

            // 複数のイメージを使った進捗表示を更新
            if (useMultipleImageProgress && progressImages.Count > 0)
            {
                UpdateMultipleImageProgress(progress);
            }

            // 進捗をパーセンテージで表示
            if (usePercentage)
            {
                UpdatePercentageText(progress);
            }
        }

        /// <summary>
        /// 複数のイメージを使用した進捗表示を更新します。
        /// </summary>
        /// <param name="progress">進行状況 (0～1)</param>
        private void UpdateMultipleImageProgress(float progress)
        {
            int filledImageCount = Mathf.FloorToInt(progress * progressImages.Count);

            for (int i = 0; i < progressImages.Count; i++)
            {
                if (i < filledImageCount)
                {
                    progressImages[i].fillAmount = 1f;
                }
                else if (i == filledImageCount)
                {
                    float partialFill = (progress * progressImages.Count) - filledImageCount;
                    progressImages[i].fillAmount = partialFill;
                }
                else
                {
                    progressImages[i].fillAmount = 0f;
                }
            }
        }

        /// <summary>
        /// プログレスバーまたは円形ゲージをスムーズに更新します。
        /// </summary>
        /// <param name="progressBarImage">更新対象のImageコンポーネント</param>
        /// <param name="targetValue">目標値 (0～1)</param>
        /// <param name="speed">更新速度</param>
        private void UpdateProgressBarSmoothly(Image progressBarImage, float targetValue, float speed)
        {
            if (progressBarImage == null) return;

            // fillAmountの差が非常に小さい場合は即座に目標値に設定
            if (Mathf.Abs(progressBarImage.fillAmount - targetValue) < 0.01f)
            {
                progressBarImage.fillAmount = targetValue;
            }
            else
            {
                float currentValue = progressBarImage.fillAmount;
                progressBarImage.fillAmount = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * speed);
            }
        }

        /// <summary>
        /// 進捗をパーセンテージ形式で表示します。
        /// </summary>
        /// <param name="progress">進行状況 (0～1)</param>
        private void UpdatePercentageText(float progress)
        {
            int percentage = Mathf.RoundToInt(progress * 100);

            if (legacyText != null)
            {
                legacyText.text = $"{percentage}%";
            }

            if (tmpText != null)
            {
                tmpText.text = $"{percentage}%";
            }
        }
    }
}
