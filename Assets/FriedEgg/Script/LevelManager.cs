using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private int playerLevel = 1;
    private int currentExperience = 0; 
    private int experienceToNextLevel = 5; // 次のレベルに必要な経験値（初期値）

    [Header("Leveling Settings")]
    public int baseExperience = 10; // 次のレベルに必要な基本経験値
    public float levelMultiplier = 1.5f; // 必要な経験値の増加倍率

    [Header("UI Elements")]
    public Slider experienceBar;
    public TMP_Text levelText;

    [Header("Chicken Settings")]
    public ChickenManager chickenManager;

    [Header("Player Settings")]
    public PlayerMovement playerMovement;
    public float speedIncreaseAmount = 0.2f; // 速度増加量

    [Header("Level-Specific Activations")]
    public List<GameObject> objectsToActivate;
    public List<int> activationLevels;

    private void Start()
    {
        UpdateUI();
    }

    public int GetPlayerLevel()
    {
        return playerLevel;
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;

        while (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        playerLevel++;
        currentExperience -= experienceToNextLevel; // 必要経験値を引き、余剰分を持ち越す
        experienceToNextLevel = Mathf.RoundToInt(baseExperience * Mathf.Pow(levelMultiplier, playerLevel - 1)); // 次のレベルに必要な経験値を計算
        Debug.Log($"次のレベルまでの経験値: {experienceToNextLevel}");

        // 偶数レベルのタイミングで鶏召喚
        if (playerLevel % 2 == 0 && chickenManager != null)
        {
            chickenManager.SpawnChicken();
        }

        // レベルが3の倍数のときプレイヤーの速度を増加
        if (playerLevel % 3 == 0 && playerMovement != null)
        {
            playerMovement.IncreaseSpeed(speedIncreaseAmount);
        }

        ActivateObjectsForLevel(playerLevel);

        UpdateUI();
    }

    private void ActivateObjectsForLevel(int level)
    {
        for (int i = 0; i < objectsToActivate.Count; i++)
        {
            if (i < activationLevels.Count && activationLevels[i] == level)
            {
                if (objectsToActivate[i] != null)
                {
                    objectsToActivate[i].SetActive(true); 
                    Debug.Log($"レベル {level} で {objectsToActivate[i].name} をアクティブ");
                }
            }
        }
    }

    private void UpdateUI()
    {
        levelText.text = $"Lv.{playerLevel}";
        experienceBar.value = (float)currentExperience / experienceToNextLevel;
    }
}
