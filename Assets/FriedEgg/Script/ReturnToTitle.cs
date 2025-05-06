using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitle : MonoBehaviour
{
    [Header("�ݒ�: �^�C�g���V�[���̖��O")]
    public string titleSceneName = "TitleScene"; // �^�C�g���V�[���̖��O

    void Update()
    {
        // Esc�L�[�������ꂽ�Ƃ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToTitleScene(); // �^�C�g���V�[���ɖ߂�
        }
    }

    /// <summary>
    /// �^�C�g���V�[���ɖ߂鏈��
    /// </summary>
    private void ReturnToTitleScene()
    {
        if (!string.IsNullOrEmpty(titleSceneName))
        {
            SceneManager.LoadScene(titleSceneName); // �w�肳�ꂽ�V�[�������[�h
        }
        else
        {
            Debug.LogError("�^�C�g���V�[�������ݒ肳��Ă��܂���I");
        }
    }
}
