using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowCloser : MonoBehaviour
{
    public GameObject windowPanel; // �E�B���h�E�̃p�l��

    void Update()
    {
        // ���N���b�N�܂���Submit�{�^���������ꂽ�ꍇ
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit"))
        {
            CloseWindow(); // �E�B���h�E�����
        }
    }

    // �E�B���h�E����鏈��
    private void CloseWindow()
    {
        if (windowPanel != null)
        {
            windowPanel.SetActive(false); // �E�B���h�E���\��
        }
    }
}
