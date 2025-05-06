using UnityEngine;
using UnityEngine.UI;

public class FillColorChange : MonoBehaviour
{
    [Header("Color Settings")]
    [SerializeField] private Color colorA = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color colorB = new Color(0.514f, 0.6f, 0.651f, 1f);

    [Header("Progress Settings")]
    [SerializeField] private Image image; // �i����\������Image
    [SerializeField] private bool useFillAmount = true; // FillAmount���g�����ǂ���
    [SerializeField] private float fillAmount = 0f; // ������Ԃ�FillAmount

    private void Start()
    {
        // ������ԂŐF��ݒ�
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
            // FillAmount�Ɋ�Â��ĐF��ω�������
            image.color = Color.Lerp(colorA, colorB, image.fillAmount);
        }
    }

    // �O������i����ݒ肷�邽�߂̃��\�b�h
    public void SetProgress(float progress)
    {
        if (image != null)
        {
            image.fillAmount = Mathf.Clamp01(progress);
        }
    }
}
