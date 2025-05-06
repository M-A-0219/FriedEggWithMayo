using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private int playerLevel = 1;
    private int currentExperience = 0; 
    private int experienceToNextLevel = 5; // ���̃��x���ɕK�v�Ȍo���l�i�����l�j

    [Header("Leveling Settings")]
    public int baseExperience = 10; // ���̃��x���ɕK�v�Ȋ�{�o���l
    public float levelMultiplier = 1.5f; // �K�v�Ȍo���l�̑����{��

    [Header("UI Elements")]
    public Slider experienceBar;
    public TMP_Text levelText;

    [Header("Chicken Settings")]
    public ChickenManager chickenManager;

    [Header("Player Settings")]
    public PlayerMovement playerMovement;
    public float speedIncreaseAmount = 0.2f; // ���x������

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
        currentExperience -= experienceToNextLevel; // �K�v�o���l�������A�]�蕪�������z��
        experienceToNextLevel = Mathf.RoundToInt(baseExperience * Mathf.Pow(levelMultiplier, playerLevel - 1)); // ���̃��x���ɕK�v�Ȍo���l���v�Z
        Debug.Log($"���̃��x���܂ł̌o���l: {experienceToNextLevel}");

        // �������x���̃^�C�~���O�Ō{����
        if (playerLevel % 2 == 0 && chickenManager != null)
        {
            chickenManager.SpawnChicken();
        }

        // ���x����3�̔{���̂Ƃ��v���C���[�̑��x�𑝉�
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
                    Debug.Log($"���x�� {level} �� {objectsToActivate[i].name} ���A�N�e�B�u");
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
