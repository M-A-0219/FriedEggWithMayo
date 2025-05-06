using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace LoadingSystem
{
    /// <summary>
    /// ���[�f�B���O��ʂ̐i���\�����Ǘ�����N���X�B
    /// �i�s�󋵂ɉ����ăv���O���X�o�[�A�T�[�N���Q�[�W�A�����̃C���[�W�A�܂��̓p�[�Z���e�[�W���X�V���܂��B
    /// </summary>
    public class ProgressManager : MonoBehaviour
    {
        [Header("Progress Bar Settings")]
        [Tooltip("�v���O���X�o�[���g�p���邩�ǂ���")]
        [SerializeField] private bool useProgressBar = false;

        [Tooltip("�v���O���X�o�[��Image�R���|�[�l���g")]
        [SerializeField] private Image progressBar;

        [Header("Circular Gauge Settings")]
        [Tooltip("�~�`�Q�[�W���g�p���邩�ǂ���")]
        [SerializeField] private bool useCircularGauge = false;

        [Tooltip("�~�`�Q�[�W��Image�R���|�[�l���g")]
        [SerializeField] private Image circularGauge;

        [Header("Multiple Image Progress Settings")]
        [Tooltip("�����̃C���[�W�𗘗p���Đi����\�����邩�ǂ���")]
        [SerializeField] private bool useMultipleImageProgress = false;

        [Tooltip("�����̐i���\���pImage�R���|�[�l���g�̃��X�g")]
        [SerializeField] private List<Image> progressImages = new List<Image>();

        [Header("Progress Percentage Settings")]
        [Tooltip("�i�����p�[�Z���e�[�W�ŕ\�����邩�ǂ���")]
        [SerializeField] private bool usePercentage = false;

        [Tooltip("���K�V�[UI�e�L�X�g (Text�R���|�[�l���g)")]
        [SerializeField] private Text legacyText;

        [Tooltip("TextMeshPro�̃e�L�X�g�R���|�[�l���g")]
        [SerializeField] private TMP_Text tmpText;

        /// <summary>
        /// �i���󋵂��X�V���܂��B
        /// </summary>
        /// <param name="progress">�i�s�� (0�`1)</param>
        public void UpdateProgress(float progress)
        {
            // �v���O���X�o�[���X�V
            if (useProgressBar && progressBar != null)
            {
                UpdateProgressBarSmoothly(progressBar, progress, 5f);
            }

            // �~�`�Q�[�W���X�V
            if (useCircularGauge && circularGauge != null)
            {
                UpdateProgressBarSmoothly(circularGauge, progress, 5f);
            }

            // �����̃C���[�W���g�����i���\�����X�V
            if (useMultipleImageProgress && progressImages.Count > 0)
            {
                UpdateMultipleImageProgress(progress);
            }

            // �i�����p�[�Z���e�[�W�ŕ\��
            if (usePercentage)
            {
                UpdatePercentageText(progress);
            }
        }

        /// <summary>
        /// �����̃C���[�W���g�p�����i���\�����X�V���܂��B
        /// </summary>
        /// <param name="progress">�i�s�� (0�`1)</param>
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
        /// �v���O���X�o�[�܂��͉~�`�Q�[�W���X���[�Y�ɍX�V���܂��B
        /// </summary>
        /// <param name="progressBarImage">�X�V�Ώۂ�Image�R���|�[�l���g</param>
        /// <param name="targetValue">�ڕW�l (0�`1)</param>
        /// <param name="speed">�X�V���x</param>
        private void UpdateProgressBarSmoothly(Image progressBarImage, float targetValue, float speed)
        {
            if (progressBarImage == null) return;

            // fillAmount�̍������ɏ������ꍇ�͑����ɖڕW�l�ɐݒ�
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
        /// �i�����p�[�Z���e�[�W�`���ŕ\�����܂��B
        /// </summary>
        /// <param name="progress">�i�s�� (0�`1)</param>
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
