using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI coinText;

    private int totalCoins = 0;

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        UpdateCoinUI();
        Debug.Log($" {amount} ���ǉ� ���݂̃R�C��: {totalCoins}");
    }

    public void RemoveCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            UpdateCoinUI();
            Debug.Log($" {amount} ������ ���݂̃R�C��: {totalCoins}");
        }
        else
        {
            Debug.Log("�R�C�����s��");
        }
    }

    public int GetCoins()
    {
        return totalCoins;
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {totalCoins}";
        }
    }
}
