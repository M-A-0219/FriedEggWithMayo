using UnityEngine;
using UnityEngine.UI;

public class FillColorChange : MonoBehaviour
{
    [Header("Color Settings")]
    [SerializeField] private Color colorA = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color colorB = new Color(0.514f, 0.6f, 0.651f, 1f);

    [Header("Progress Settings")]
    [SerializeField] private Image image; // 進捗を表示するImage
    [SerializeField] private bool useFillAmount = true; // FillAmountを使うかどうか
    [SerializeField] private float fillAmount = 0f; // 初期状態のFillAmount

    private void Start()
    {
        // 初期状態で色を設定
        if (image != null)
        {
            image.color = colorA;
            image.fillAmount = fillAmount;
        }
    }

    private void Update()
    {
        if (useFillAmount && image != null)
        {
            // FillAmountに基づいて色を変化させる
            image.color = Color.Lerp(colorA, colorB, image.fillAmount);
        }
    }

    // 外部から進捗を設定するためのメソッド
    public void SetProgress(float progress)
    {
        if (image != null)
        {
            image.fillAmount = Mathf.Clamp01(progress);
        }
    }
}
