using System.Collections;
using TMPro;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public bool isHavingBasket = false;
    public bool isHavingBowl = false; // �ǂ�Ԃ�������Ă��邩
    public BowlState currentBowlState = BowlState.Empty; // ���݂̂ǂ�Ԃ�̏��
    public int eggCount = 0;

    public LayerMask interactableLayer; // �Ώە����C���[
    [Header("Input Settings")]
    public float inputHoldDelay = 0.2f; // ���������̏����Ԋu

    private float holdTimer = 0f; // �����������p�̃^�C�}�[
    private bool isHoldingLeftClick = false; // �E�N���b�N�𒷉��������ǂ���
    [Header("Interaction Settings")]
    public float interactionDistance = 2.0f; // �C���^���N�g�\����
    public float interactionAngle = 45f; // ���E�̊p�x�i�����̊p�x�j

    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;

    [Header("Audio Settings")]
    public AudioClip saleSound;
    private AudioSource audioSource;

    [Header("UI Elements")]
    public TMP_Text eggCountText;

    private Vector2 lastDirection = Vector2.down; // �v���C���[���Ō�Ɍ����������iPlayerMovement����擾�j
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        UpdateEggCountUI();
    }

    void Update()
    {
        HandleInput();
        UpdateAnimatorParameters();

        if (Input.GetKey(KeyCode.Space)&&(!isHavingBowl))
        {
            isHavingBasket = true;
        }
        else if (isHavingBasket && eggCount == 0) // �����Ȃ��ꍇ
        {
            isHavingBasket = false;
        }

        animator.SetBool("isHavingBasket", isHavingBasket);

        // ���̐��ɉ����ăA�j���[�V������؂�ւ�
        if (eggCount < 3)
        {
            animator.SetInteger("Egg", eggCount);
        }
        else
        {
            animator.SetInteger("Egg", 3); // 3�ȏ�͍ő�l
        }
        // �����������i�E�N���b�N�j
        if (isHoldingLeftClick)
        {
            holdTimer -= Time.deltaTime;
            if (holdTimer <= 0f)
            {
                holdTimer = inputHoldDelay; // �^�C�}�[�����Z�b�g

                // ���������Ƀ}���l�[�Y�}�V���ɗ��𓊓�
                Collider2D target = FindClosestInteractable();
                if (target != null && target.CompareTag("MayonnaiseMachine"))
                {
                    MayonnaiseMachine machine = target.GetComponent<MayonnaiseMachine>();
                    if (eggCount > 0) // ��������ꍇ�̂ݓ���
                    {
                        machine.AddEgg(this); // �}�V���ɗ���ǉ�
                        Debug.Log($"���𒷉����Œǉ����܂����I ���݂̗���: {eggCount}");
                        UpdateEggCountUI();
                    }
                    else
                    {
                        Debug.Log("��������܂���I");
                        isHoldingLeftClick = false; // �����Ȃ��Ȃ����璷����������
                    }
                }
            }
        }
    }
    public void SetLastDirection(Vector2 direction)
    {
        lastDirection = direction; // PlayerMovement����������Z�b�g����
    }
    public void AddEgg()
    {
        eggCount++;
        Debug.Log("�����E���܂����B���݂̗���: " + eggCount);

        UpdateEggCountUI();
    }
    private void UpdateEggCountUI()
    {
        if (eggCountText != null)
        {
            eggCountText.text = $": {eggCount}"; // �e�L�X�g���X�V
        }
    }
    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")) // ���N���b�N
        {
            if (!isHoldingLeftClick) // ���������łȂ��ꍇ�̂ݒP����������
            {
                Debug.Log("���N���b�N��������܂���");
                InteractWithLeftClick();
            }
        }
        else if (Input.GetMouseButton(0) || Input.GetButton("Submit")) // ���N���b�N������
        {
            if (!isHoldingLeftClick) // �������J�n��
            {
                isHoldingLeftClick = true;
                holdTimer = 0f; // �^�C�}�[�����Z�b�g
            }
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Submit")) // ���N���b�N�𗣂����Ƃ�
        {
            if (isHoldingLeftClick)
            {
                isHoldingLeftClick = false; // ������������
            }
        }
    }




   /* private void InteractWithRightClick()
    {
        Debug.Log("�E�N���b�N��������܂���");

        Collider2D target = FindClosestInteractable();
        if (target == null)
        {
            Debug.Log("�E�N���b�N����: �Ώە���������܂���ł���");
            return;
        }

        Debug.Log("�E�N���b�N����: �Ώە� " + target.name + " �����o���܂���");

        if (target.CompareTag("FryingPan") && eggCount > 0) // �t���C�p���ɗ����g�p
        {
            FryingPan fryingPan = target.GetComponent<FryingPan>();

            // �t���C�p�����g�p�\�i��̏�ԁj���m�F
            if (fryingPan != null && fryingPan.IsReadyToCook())
            {
                Debug.Log("�t���C�p���𔭌����A�����g�p���܂�");
                eggCount--; // ����1���炵�܂�
                fryingPan.StartCooking(); // �t���C�p���̒������J�n

                Debug.Log("�����g�p���܂����B���݂̗���: " + eggCount);
                UpdateEggCountUI();
            }
        }
        else if (target.CompareTag("FryingPan"))
        {
            Debug.Log("�t���C�p���͌��o���ꂽ���A��������܂���");
        }
        else
        {
            Debug.Log("�E�N���b�N����: �t���C�p���܂��̓}���l�[�Y�}�V���ȊO�̑Ώە������o����܂���");
        }
    }*/



    private void InteractWithLeftClick()
    {
        Collider2D target = FindClosestInteractable();
        if (target == null) return;


        if (target.CompareTag("FryingPan") && eggCount > 0) // �t���C�p���ɗ����g�p
        {
            FryingPan fryingPan = target.GetComponent<FryingPan>();

            // �t���C�p�����g�p�\�i��̏�ԁj���m�F
            if (fryingPan != null && fryingPan.IsReadyToCook())
            {
                Debug.Log("�t���C�p���𔭌����A�����g�p���܂�");
                eggCount--; // ����1���炵�܂�
                fryingPan.StartCooking(); // �t���C�p���̒������J�n

                Debug.Log("�����g�p���܂����B���݂̗���: " + eggCount);
                UpdateEggCountUI();
            }
        }
        else if (target.CompareTag("FryingPan"))
        {
            Debug.Log("�t���C�p���͌��o���ꂽ���A��������܂���");
        }

        // �}���l�[�Y�}�V���Ƃ̑��ݍ�p
        if (target.CompareTag("MayonnaiseMachine"))
        {
            MayonnaiseMachine machine = target.GetComponent<MayonnaiseMachine>();

            // �v���C���[��2�i�K�ڂ̂ǂ�Ԃ�������Ă���ꍇ�̓A�b�v�O���[�h�����݂�
            if (isHavingBowl && currentBowlState == BowlState.RiceWithEgg)
            {
                machine.Interact(this, false); // false: �P����
            }
            if(isHavingBowl && currentBowlState == BowlState.Rice)
            {
                ActivateArrow2();
            }
            if (!isHavingBowl)
            {
                ActivateArrow1();
            }
            return;
        }

        // �t���C�p���Ƃ̑��ݍ�p
        if (target.CompareTag("FryingPan"))
        {
            FryingPan fryingPan = target.GetComponent<FryingPan>();
            fryingPan.Interact(this);
            return; // �����I��
        }

        // ���ъ�Ƃ̑��ݍ�p
        if (target.CompareTag("RiceCooker"))
        {
            RiceCooker riceCooker = target.GetComponent<RiceCooker>();
            // ���ђ��܂��͋�łȂ��ꍇ�̑��쐧��
            if (!riceCooker.IsEmpty() && riceCooker.IsCooking())
            {
                Debug.Log("���ђ��̂��ߑ���ł��܂���I");
                return;
            }
            // ���т𐆂��n�߂�ꍇ
            if (riceCooker.IsEmpty())
            {
                riceCooker.StartCookingRice(); // ���ъ�ŕĂ𐆂��n�߂�
                Debug.Log("���ъ�ŕĂ𐆂��n�߂܂����I");
                return;
            }

            // ���т����o���ꍇ
            if (!isHavingBasket && !isHavingBowl) // �����𖾊m��
            {
                riceCooker.TakeRice(); // ���ъ킩�炲�т����o��
                isHavingBowl = true;
                currentBowlState = BowlState.Rice; // 1�i�K�ڂ��擾
                Debug.Log("���ъ킩�炲�т����܂����I �ǂ�Ԃ���: " + currentBowlState);
                return;
            }

            // �ǂ�Ԃ�₩���������Ă���ꍇ
            Debug.Log("���łɂǂ�Ԃ�₩���������Ă��邽�߁A���т����o���܂���I");
        }

        // �e�[�u���Ƃ̑��ݍ�p
        if (target.CompareTag("Table"))
        {
            Table table = target.GetComponent<Table>();

            if (isHavingBowl) // �v���C���[���ǂ�Ԃ�������Ă���ꍇ
            {
                if (table.HasBowl()) // �e�[�u���ɂǂ�Ԃ肪���łɂ���ꍇ
                {
                    Debug.Log("�e�[�u���ɂ͂��łɂǂ�Ԃ肪�u����Ă��܂��I �ǂ�Ԃ��u���܂���I");
                    return;
                }

                // �e�[�u���ɂǂ�Ԃ��u��
                table.PlaceBowl(currentBowlState);
                isHavingBowl = false;
                currentBowlState = BowlState.Empty; // �v���C���[�̂ǂ�Ԃ�����
                Debug.Log("�e�[�u���ɂǂ�Ԃ��u���܂����I");
            }
            else if (table.HasBowl()&&!isHavingBasket) // �v���C���[���ǂ�Ԃ�������Ă��Ȃ��ꍇ�A�e�[�u������ǂ�Ԃ�����
            {
                isHavingBowl = true;
                currentBowlState = table.TakeBowl(); // �e�[�u������ǂ�Ԃ���擾
                Debug.Log("�e�[�u������ǂ�Ԃ�����܂����I �ǂ�Ԃ���: " + currentBowlState);
            }
        }

        // NPC�Ƃ̑��ݍ�p
        if (target.CompareTag("NPC"))
        {
            NPC npc = target.GetComponent<NPC>();

            // NPC�ɂǂ�Ԃ��̔�
            if (isHavingBowl && currentBowlState == BowlState.Complete)
            {
                // �v���C���[�̂ǂ�Ԃ�����Z�b�g
                isHavingBowl = false;
                currentBowlState = BowlState.Empty;

                // NPC�̍w���������Ăяo��
                npc.OnPurchaseComplete();

                Debug.Log("�ǂ�Ԃ��̔����܂����I");
                PlayAudio(saleSound);
            }
            if (isHavingBowl && currentBowlState == BowlState.RiceWithEgg)
            {
                ActivateArrow3();
            }
            if (isHavingBowl && currentBowlState == BowlState.Rice)
            {
                ActivateArrow2();
            }
            /*if (!isHavingBowl)
            {
                ActivateArrow1();
            }*/
            return; // �����I��
        }

    }



    private Collider2D FindClosestInteractable()
    {
        // �v���C���[�̊�_���ꎞ�I�ɏ�ɕύX
        Vector3 pivotOffset = new Vector3(0, 1.0f, 0); // �v���C���[�̍����ɉ����Ē���
        Vector3 pivotPosition = transform.position + pivotOffset;

        // �I�[�o�[���b�v�T�[�N���ŃC���^���N�V�����\�ȃI�u�W�F�N�g���擾
        Collider2D[] hits = Physics2D.OverlapCircleAll(pivotPosition, interactionDistance, interactableLayer);

        Collider2D closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            // �������g�̃R���C�_�[�𖳎��i�d�Ȃ������j
            if (hit.gameObject == gameObject)
            {
                continue;
            }

            // �v���C���[����^�[�Q�b�g�ւ̕������v�Z
            Vector2 directionToTarget = (hit.transform.position - pivotPosition).normalized;

            // �v���C���[�̎��E�p�x���ɂ��邩�𔻒�
            float angle = Vector2.Angle(lastDirection, directionToTarget);
            if (angle <= interactionAngle)
            {
                // �^�[�Q�b�g�܂ł̋������v�Z
                float distance = (hit.transform.position - pivotPosition).sqrMagnitude;

                // �ł��߂��I�u�W�F�N�g���X�V
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = hit;
                }
            }
        }

        return closest;
    }




    private void PlayAudio(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }


    private void UpdateAnimatorParameters()
    {
        animator.SetBool("isHavingBasket", isHavingBasket);
        animator.SetBool("isHavingBowl", isHavingBowl);

        // ���̐��ɉ����ăA�j���[�V������؂�ւ�
        if (eggCount < 3)
        {
            animator.SetInteger("Egg", eggCount);
        }
        else
        {
            animator.SetInteger("Egg", 3); // 3�ȏ�͍ő�l
        }
        // ���̏�Ԃ��A�j���[�V�����ɔ��f
        animator.SetInteger("Bowl", (int)currentBowlState);
    }

    public void ActivateArrow1()
    {
        StartCoroutine(ActivateAndDeactivateArrow(arrow1));
    }

    public void ActivateArrow2()
    {
        StartCoroutine(ActivateAndDeactivateArrow(arrow2));
    }

    public void ActivateArrow3()
    {
        StartCoroutine(ActivateAndDeactivateArrow(arrow3));
    }

    private IEnumerator ActivateAndDeactivateArrow(GameObject arrow)
    {
        if (arrow != null)
        {
            arrow.SetActive(true); // �A�N�e�B�u��
            yield return new WaitForSeconds(3f); // 3�b�ҋ@
            arrow.SetActive(false); // ��A�N�e�B�u��
        }
    }

}
