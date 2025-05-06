using UnityEngine;

public class LoadingSceneRoot : MonoBehaviour
{
    private void Awake()
    {
        // ���̃I�u�W�F�N�g�S�̂��V�[���Ԃŕێ�
        DontDestroyOnLoad(gameObject);

        // �d���C���X�^���X�̍폜
        if (FindObjectsOfType<LoadingSceneRoot>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
