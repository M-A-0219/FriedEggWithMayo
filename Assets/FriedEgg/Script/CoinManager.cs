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
        Debug.Log($" {amount} 枚追加 現在のコイン: {totalCoins}");
    }

    public void RemoveCoins(int amount)
    {
        if (totalCoins >= amount)
        {
            totalCoins -= amount;
            UpdateCoinUI();
            Debug.Log($" {amount} 枚減少 現在のコイン: {totalCoins}");
        }
        else
        {
            Debug.Log("コインが不足");
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
