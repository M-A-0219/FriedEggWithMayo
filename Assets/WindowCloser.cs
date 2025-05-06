using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowCloser : MonoBehaviour
{
    public GameObject windowPanel; // ウィンドウのパネル

    void Update()
    {
        // 左クリックまたはSubmitボタンが押された場合
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit"))
        {
            CloseWindow(); // ウィンドウを閉じる
        }
    }

    // ウィンドウを閉じる処理
    private void CloseWindow()
    {
        if (windowPanel != null)
        {
            windowPanel.SetActive(false); // ウィンドウを非表示
        }
    }
}
