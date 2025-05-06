using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using LoadingSystem; // ProgressManager ���g�p���邽�߂ɕK�v

public class LoadingSceneManager : MonoBehaviour
{
    public static LoadingSceneManager Instance;

    [Header("General Settings")]
    [Tooltip("���[�f�B���O�V�[���̖��O (�f�t�H���g: LoadingScene)")]
    [SerializeField] private string loadingSceneName = "LoadingScene";

    [Tooltip("���[�f�B���O�V�[���̍Œ�\������ (�b)")]
    [SerializeField] private float minDisplayTime = 1.5f;

    [Header("Progress Manager Settings")]
    [SerializeField] private ProgressManager progressManager; // ProgressManager�̎Q��

    [Header("Fade Settings")]
    [Tooltip("�t�F�[�h�pAnimator")]
    [SerializeField] private Animator transitionAnimator;

    [Tooltip("�t�F�[�h�C���p�̃g���K�[�p�����[�^��")]
    [SerializeField] private string fadeInTrigger = "FadeIn";

    [Tooltip("�t�F�[�h�A�E�g�p�̃g���K�[�p�����[�^��")]
    [SerializeField] private string fadeOutTrigger = "FadeOut";

    [Tooltip("���[�f�B���O����Ԃ������g���K�[�p�����[�^��")]
    [SerializeField] private string loadingTrigger = "Loading";

    /// <summary>
    /// �V���O���g���C���X�^���X�̏�����
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

        // ProgressManager���ݒ肳��Ă��Ȃ��ꍇ�A�V�[��������T���Đݒ�
        if (progressManager == null)
        {
            progressManager = FindObjectOfType<ProgressManager>();
        }

        // ProgressManager���ݒ肳��Ă��Ȃ��ꍇ�̃G���[���b�Z�[�W
        if (progressManager == null)
        {
            Debug.LogError("LoadingSceneManager: ProgressManager���ݒ肳��Ă��܂���I");
        }
    }

    /// <summary>
    /// �w�肳�ꂽ�V�[�������[�h
    /// </summary>
    /// <param name="targetScene">���[�h����V�[����</param>
    public void LoadScene(string targetScene)
    {
        if (string.IsNullOrEmpty(loadingSceneName) || string.IsNullOrEmpty(targetScene))
        {
            Debug.LogError("LoadingSceneManager: �V�[�������ݒ肳��Ă��܂���I");
            return;
        }

        StartCoroutine(LoadSceneWithLoading(targetScene));
    }

    /// <summary>
    /// ���[�f�B���O�V�[��������Ń^�[�Q�b�g�V�[�������[�h
    /// </summary>
    /// <param name="targetScene">�^�[�Q�b�g�V�[����</param>
    private IEnumerator LoadSceneWithLoading(string targetScene)
    {
        // �t�F�[�h�C���A�j���[�V�������Đ�
        yield return StartCoroutine(PlayAnimation(fadeInTrigger));

        // ���[�f�B���O�V�[����񓯊��Ń��[�h
        AsyncOperation loadingSceneOp = SceneManager.LoadSceneAsync(loadingSceneName);
        yield return new WaitUntil(() => loadingSceneOp.isDone);

        // ���[�f�B���O�A�j���[�V������Ԃ�ݒ�
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger(loadingTrigger);
        }

        // ���̃V�[����񓯊��Ń��[�h
        AsyncOperation targetSceneOp = SceneManager.LoadSceneAsync(targetScene);
        targetSceneOp.allowSceneActivation = false;

        // ���[�f�B���O���̐i���Ǘ�
        while (!targetSceneOp.isDone)
        {
            float progress = targetSceneOp.progress;

            // �i����0.9�����̎��̂ݐi�����X�V
            if (progress < 0.9f)
            {
                progressManager.UpdateProgress(progress); // ProgressManager�ɐi����n��
            }
            else
            {
                progressManager.UpdateProgress(1f); // �i����90%�ȏ�̎���1�Ƃ݂Ȃ�
            }

            if (progress >= 0.9f)
            {
                break;
            }

            yield return null;
        }

        // �Œ�\�����Ԃ�҂�
        yield return new WaitForSeconds(minDisplayTime);

        // ���̃V�[�����A�N�e�B�u��
        targetSceneOp.allowSceneActivation = true;

        // ���̃t���[���܂őҋ@���ăV�[�������S�ɐ؂�ւ���
        yield return null;

        // �t�F�[�h�A�E�g�A�j���[�V�������Đ�
        yield return StartCoroutine(PlayAnimation(fadeOutTrigger));
    }

    /// <summary>
    /// �w�肳�ꂽ�A�j���[�V�����g���K�[���Đ�
    /// </summary>
    /// <param name="trigger">Animator �g���K�[��</param>
    private IEnumerator PlayAnimation(string trigger)
    {
        if (transitionAnimator == null || string.IsNullOrEmpty(trigger)) yield break;

        transitionAnimator.SetTrigger(trigger);
        yield return new WaitForSeconds(1f); // �A�j���[�V�����̍Đ����Ԃɍ��킹�Ē���
    }
}
