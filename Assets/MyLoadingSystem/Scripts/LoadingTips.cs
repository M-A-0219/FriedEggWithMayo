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
            public string category; // �J�e�S����^�C�g���p
            public string tip; // �q���g
            public Sprite tipImage; // �q���g�Ɋ֘A����摜�i�I�v�V�����j
        }

        [Header("Tips List")]
        [Tooltip("���[�f�B���O�V�[���̖��O (�f�t�H���g: LoadingScene)")]
        [SerializeField] private GameTip[] tips;

        [Header("Settings")]
        [Tooltip("���[�f�B���O�V�[���̖��O (�f�t�H���g: LoadingScene)")]
        [SerializeField] private float timeBetweenSwitch = 5f;
        [Header("Category")]
        [Tooltip("�J�e�S����^�C�g���p")]
        [SerializeField] private TMP_Text categoryText;
        [Header("TipText")]
        [SerializeField] private TMP_Text tipText;
        [Header("Image")]
        [SerializeField] private Image tipImage;

        private int currentTipIndex = -1;
        private int lastTipIndex = -1;

        [Header("Switching Control")]
        [Tooltip("�؂�ւ��̃I��/�I�t���Ǘ�")]
        [SerializeField] private bool isTipSwitchingEnabled = true; 

        void Start()
        {
            // ����\���i�����_����Tip��\���j
            SwitchTip();

            // ���Ԍo�߂ł̐؂�ւ����L���ȏꍇ�A�R���[�`�����J�n
            if (isTipSwitchingEnabled)
            {
                StartCoroutine(SwitchTipCoroutine());
            }
        }

        void Update()
        {
            // �؂�ւ����L���ŁA�N���b�N�ł̐؂�ւ����\�ȏꍇ
            if (isTipSwitchingEnabled && Input.GetMouseButtonDown(0))
            {
                SwitchTip();
            }
        }

        /// <summary>
        /// �q���g��؂�ւ��A���̃q���g��\�����܂��B
        /// </summary>
        private void SwitchTip()
        {
            int newTipIndex = GetRandomTipIndex();

            // �����q���g���A�����Ȃ��悤�ɂ���
            while (newTipIndex == lastTipIndex && tips.Length > 1)
            {
                newTipIndex = GetRandomTipIndex();
            }

            lastTipIndex = newTipIndex;
            currentTipIndex = newTipIndex;

            ShowTip();
        }

        /// <summary>
        /// �����_���Ƀq���g�̃C���f�b�N�X���擾���܂��B
        /// </summary>
        private int GetRandomTipIndex()
        {
            return Random.Range(0, tips.Length);
        }

        /// <summary>
        /// ���݂̃q���g��\�����܂��B
        /// </summary>
        private void ShowTip()
        {
            GameTip currentTip = tips[currentTipIndex];

            // Tip����̏ꍇ�̓G���[���
            tipText.text = !string.IsNullOrEmpty(currentTip.tip) ? currentTip.tip : "Tip Unavailable";

            // Category����̏ꍇ�̓G���[���
            if (categoryText != null)
            {
                categoryText.text = !string.IsNullOrEmpty(currentTip.category) ? currentTip.category : "Category Unavailable";
            }

            // �摜�̗L���ŕ\��/��\����؂�ւ���
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
        /// ��莞�Ԃ��ƂɃq���g��؂�ւ���R���[�`���B
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
        /// �؂�ւ��𖳌�������i����\���̂ݍs���j
        /// </summary>
        public void DisableTipSwitching()
        {
            isTipSwitchingEnabled = false;
            StopAllCoroutines(); // �R���[�`�����~
        }

        /// <summary>
        /// �؂�ւ���L��������i���Ԍo�߂���уN���b�N�Ő؂�ւ��\�ɂ���j
        /// </summary>
        public void EnableTipSwitching()
        {
            isTipSwitchingEnabled = true;
            StartCoroutine(SwitchTipCoroutine()); // �R���[�`�����ĊJ
        }
    }
}
