using UnityEngine;
using UnityEngine.UI;

public class RiceCooker : MonoBehaviour
{
    private enum State
    {
        Empty,
        Cooking,
        Ready,
        HalfRemaining
    }

    [Header("Settings")]
    public int baseRiceAmount = 10; 
    public float baseCookTime = 20f;
    public Sprite[] stateSprites; 
    public ParticleSystem cookingParticles;

    [Header("UI Elements")]
    public Image timer;

    [Header("Audio Settings")]
    public AudioClip cookingSound;
    public AudioClip riceSound;
    private AudioSource audioSource;

    private State currentState = State.Empty; 
    private int riceAmount;
    private int maxRiceAmount;
    private float cookTime;
    private float cookingTimer = 0f; 
    private SpriteRenderer spriteRenderer;
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateState(State.Empty);
        audioSource = GetComponent<AudioSource>();

        UpdateRiceCookerStats();
        timer.fillAmount = 0f;
    }

    void Update()
    {
        if (currentState == State.Cooking)
        {
            cookingTimer -= Time.deltaTime;

            UpdateTimerUI();

            if (cookingTimer <= 0)
            {
                FinishCooking(); 
            }
        }
    }

    public bool IsEmpty()
    {
        return currentState == State.Empty;
    }
    public bool IsCooking()
    {
        return currentState == State.Cooking;
    }

    public void StartCookingRice()
    {
        if (currentState == State.Empty)
        {
            PlayAudio(cookingSound);
            currentState = State.Cooking; 
            cookingTimer = cookTime;
            riceAmount = 0;
            UpdateState(State.Cooking);

            timer.fillAmount = 1f;
        }

    }


    public void TakeRice()
    {
        if (currentState != State.Ready && currentState != State.HalfRemaining)
        {
            Debug.LogWarning("ご飯が取り出せる状態ではありません！");
            return;
        }

        // ご飯の量を確認して減少
        if (riceAmount > 0)
        {
            riceAmount--;
            PlayAudio(riceSound);
            Debug.Log($"残りのご飯: {riceAmount}");

            // ご飯が0以下になった場合、状態を更新
            if (riceAmount == 0)
            {
                UpdateState(State.Empty);
            }
            else if (riceAmount <= maxRiceAmount / 2)
            {
                UpdateState(State.HalfRemaining);
            }
        }
        else
        {
            Debug.LogWarning("ご飯がもうありません！");
        }
    }




    private void FinishCooking()
    {
        riceAmount = maxRiceAmount; 
        UpdateState(State.Ready);
        Debug.Log("ご飯の量: " + riceAmount);

        timer.fillAmount = 0f;
    }

    private void UpdateRiceCookerStats()
    {
        maxRiceAmount = baseRiceAmount ; 
        cookTime = baseCookTime; 
        cookTime = Mathf.Max(cookTime, 5f);
    }

    private void UpdateState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Empty:
                spriteRenderer.sprite = stateSprites[0];
                if (cookingParticles.isPlaying) cookingParticles.Stop(); 
                break;
            case State.Cooking:
                spriteRenderer.sprite = stateSprites[1];
                if (!cookingParticles.isPlaying) cookingParticles.Play(); 
                break;
            case State.Ready:
                spriteRenderer.sprite = stateSprites[2];
                if (!cookingParticles.isPlaying) cookingParticles.Play();
                break;
            case State.HalfRemaining:
                spriteRenderer.sprite = stateSprites[3];
                if (!cookingParticles.isPlaying) cookingParticles.Play();
                break;
        }
    }

    private void UpdateTimerUI()
    {
        if (cookingTimer > 0)
        {
            timer.fillAmount = cookingTimer / cookTime; 
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
