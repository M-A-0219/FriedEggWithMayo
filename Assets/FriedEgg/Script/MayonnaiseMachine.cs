using UnityEngine;
using UnityEngine.UI;

public class MayonnaiseMachine : MonoBehaviour
{
    [Header("Settings")]
    public int mayonnaisePerEgg = 8; // 卵1個で製造できるマヨネーズの量
    //public int mayonnaiseNeededForUpgrade = 1; 
    public float inputHoldDelay = 0.2f;

    private int mayonnaiseStock = 0;
    private bool isHoldingInput = false;

    [Header("UI Settings")]
    public Image fillImage; 
    public int maxMayonnaiseStock = 100;


    [Header("Audio Settings")]
    public AudioClip cookingSound;
    private AudioSource audioSource;

    [Header("Particle Settings")]
    public GameObject particlePrefab; 
    public Vector3 particleOffset = new Vector3(0, 1.0f, 0);

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact(PlayerController player, bool isHolding)
    {

        isHoldingInput = isHolding;

        if (player.eggCount > 0)
        {
            AddEgg(player);
        }
        else if (player.isHavingBowl && player.currentBowlState == BowlState.RiceWithEgg) 
        {
            UpgradeBowl(player); 
        }
    }

    public void AddEgg(PlayerController player)
    {
        player.eggCount--;
        mayonnaiseStock += mayonnaisePerEgg;
        Debug.Log($"現在のマヨネーズ: {mayonnaiseStock}");
        UpdateUI();
    }

    private void UpgradeBowl(PlayerController player)
    {
        if (mayonnaiseStock >= 1) 
        {
            mayonnaiseStock -= 1; 
            player.currentBowlState = BowlState.Complete;
            PlayUpgradeParticle(player);
            UpdateUI();
        }
    }

    private void PlayUpgradeParticle(PlayerController player)
    {
        if (particlePrefab == null)
        {
            Debug.LogWarning("Particle prefab is not assigned!");
            return;
        }

        // パーティクル生成
        Vector3 spawnPosition = player.transform.position + particleOffset;
        GameObject particleInstance = Instantiate(particlePrefab, spawnPosition, Quaternion.identity);

        PlayAudio(cookingSound);

        // パーティクル削除
        ParticleSystem particleSystem = particleInstance.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            Destroy(particleInstance, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
        }
        else
        {
            Debug.LogWarning("The particle prefab does not have a ParticleSystem component!");
        }
    }
    private void UpdateUI()
    {
        if (fillImage != null)
        {
            float fillAmount = (float)mayonnaiseStock / maxMayonnaiseStock;
            fillImage.fillAmount = Mathf.Clamp01(fillAmount);
        }
    }

    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
