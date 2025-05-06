using UnityEngine;

public class FryingPan : MonoBehaviour
{
    private enum State
    {
        Empty,
        RawEgg,
        CookedEgg,
        Burnt
    }

    [Header("Settings")]
    public Sprite[] stateSprites;
    public float cookingTime = 10f;
    public float burningTime = 15f;
    public int nonBurningLevel = 10; // このレベル以上では焦げなくなる

    [Header("References")]
    public LevelManager levelManager;
    public PlayerController playerController;

    [Header("Audio Settings")]
    public AudioClip cookingSound;
    private AudioSource audioSource;

    private State currentState = State.Empty;
    private SpriteRenderer spriteRenderer;
    private float cookingTimer = 0f;
    private bool isCooking = false;

    public ParticleSystem cookingParticles;
    public ParticleSystem smokeParticles;

    private ParticleSystem.MainModule cookingMain;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // パーティクルの設定
        cookingMain = cookingParticles.main;

        UpdateState(State.Empty);
    }

    public bool IsReadyToCook()
    {
        return currentState == State.Empty;
    }

    void Update()
    {
        if (isCooking)
        {
            cookingTimer -= Time.deltaTime;

            if (currentState == State.RawEgg)
            {
                if (cookingTimer <= 0)
                {
                    UpdateState(State.CookedEgg);
                    cookingTimer = burningTime;
                }
            }
            else if (currentState == State.CookedEgg)
            {
                UpdateCookingEffect(); // ここで色の変化のみ行う
                if (cookingTimer <= 0)
                {
                    if (levelManager.GetPlayerLevel() >= nonBurningLevel)
                    {
                        isCooking = false;
                        return;
                    }
                    UpdateState(State.Burnt);
                    isCooking = false;
                }
            }
        }
    }

    public void StartCooking()
    {
        if (currentState == State.Empty) // 空の状態でのみ開始可能
        {
            UpdateState(State.RawEgg);
            cookingTimer = cookingTime;
            isCooking = true;
            Debug.Log("フライパンで料理を開始");
            PlayAudio(cookingSound);
        }
    }

    public void Interact(PlayerController player)
    {
        // 焦げ状態なら空にする
        if (currentState == State.Burnt)
        {
            UpdateState(State.Empty);
            return;
        }

        if (currentState == State.CookedEgg && player.isHavingBowl && player.currentBowlState == BowlState.Rice)
        {
            player.currentBowlState = BowlState.RiceWithEgg;
            player.isHavingBowl = true;
            UpdateState(State.Empty);
            return;
        }
        else if (!player.isHavingBowl && currentState == State.CookedEgg)
        {
            playerController.ActivateArrow1();
        }
    }

    private void UpdateState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Empty:
                spriteRenderer.sprite = stateSprites[0];
                if (cookingParticles.isPlaying) cookingParticles.Stop();
                if (smokeParticles.isPlaying) smokeParticles.Stop();
                ResetCookingEffect();
                break;
            case State.RawEgg:
                spriteRenderer.sprite = stateSprites[1];
                if (!cookingParticles.isPlaying) cookingParticles.Play();
                if (smokeParticles.isPlaying) smokeParticles.Stop();
                ResetCookingEffect(); // 生の状態では変化しないようリセット
                break;
            case State.CookedEgg:
                spriteRenderer.sprite = stateSprites[2];
                if (!cookingParticles.isPlaying) cookingParticles.Play();
                if (smokeParticles.isPlaying) smokeParticles.Stop();
                break;
            case State.Burnt:
                spriteRenderer.sprite = stateSprites[3];
                if (cookingParticles.isPlaying) cookingParticles.Stop();
                if (!smokeParticles.isPlaying) smokeParticles.Play();
                ResetCookingEffect();
                break;
        }
    }

    private void UpdateCookingEffect()
    {
        float progress = 1f - (cookingTimer / burningTime); // 0 → 1 に進行

        if (progress < 0.7f)
        {
            // 70% までは変化しない
            cookingMain.startColor = new Color(1f, 1f, 1f, 0.4f);
        }
        else
        {
            float darkenAmount = (progress - 0.7f) * 3.3f; // 0.7 以降で急速に黒く
            cookingMain.startColor = new Color(1f - darkenAmount, 1f - darkenAmount, 1f - darkenAmount, 0.4f);
        }
    }

    private void ResetCookingEffect()
    {
        cookingMain.startColor = new Color(1f, 1f, 1f, 0.4f);
    }

    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
