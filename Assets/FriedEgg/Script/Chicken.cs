using System.Collections;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float moveSpeed = 2f; // �ړ����x�����ɕۂ�

    public float minStopTime = 1f; // ��~����ŏ�����
    public float maxStopTime = 3f; // ��~����ő厞��
    public float minMoveTime = 2f; // �ړ�����ŏ�����
    public float maxMoveTime = 4f; // �ړ�����ő厞��

    public GameObject eggPrefab;
    public float eggDropChance = 0.1f; // ���𗎂Ƃ��m��

    private PolygonCollider2D boundaryCollider; // �ړ��͈͂������R���C�_�[
    private Vector2 moveDirection; // �ړ�����
    private float actionTimer; // ��Ԑ؂�ւ��̃^�C�}�[
    private SpriteRenderer spriteRenderer; // �X�v���C�g�̌����𐧌䂷��
    private Animator animator; // �A�j���[�V��������
    private Rigidbody2D rb; // ���������𐧌�
    private bool isTouchingWall = false; // �ǂɐG��Ă��邩�ǂ���
    private Vector2 lastBlockedDirection = Vector2.zero;

    private enum ChickenState
    {
        Stopped,
        Moving
    }

    private ChickenState currentState;

    void Start()
    {
        // �K�v�ȃR���|�[�l���g���擾
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Boundary�i�ړ��͈́j�̎擾
        GameObject boundaryObject = GameObject.FindWithTag("Boundary");
        if (boundaryObject != null)
        {
            boundaryCollider = boundaryObject.GetComponent<PolygonCollider2D>();
        }
        else
        {
            Debug.LogError("Boundary Collider ��������܂���IBoundary�I�u�W�F�N�g��Tag��ݒ肵�Ă��������B");
        }

        // �����_���ȏ����^�C�}�[��ݒ�
        actionTimer = Random.Range(minStopTime, maxStopTime);

        // ������Ԃ��~�ɐݒ�
        ChangeState(ChickenState.Stopped);
        StartCoroutine(EggRoutine());
    }

    void Update()
    {
        // �^�C�}�[������
        actionTimer -= Time.deltaTime;

        if (actionTimer <= 0)
        {
            if (currentState == ChickenState.Stopped)
            {
                StartMoving(); // ��~����ړ��ɐ؂�ւ�
            }
            else if (currentState == ChickenState.Moving)
            {
                StopMoving(); // �ړ������~�ɐ؂�ւ�
            }
        }

        // �ړ�����
        if (currentState == ChickenState.Moving)
        {
            Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.deltaTime;

            // �ǂɐG��Ă��Ȃ��ꍇ�݈̂ړ�
            if (!isTouchingWall && IsInsideBoundary(newPosition))
            {
                rb.MovePosition(newPosition);
            }
            else
            {
                // �ǂɓ����������~
                StopMoving();
            }

            // �X�v���C�g�̌�����ݒ�
            spriteRenderer.flipX = moveDirection.x > 0;
        }
    }

    void StartMoving()
    {
        // �ړ������������_���ɐݒ�
        float randomX, randomY;
        do
        {
            randomX = Random.Range(-1f, 1f);
            randomY = Random.Range(-1f, 1f);
            moveDirection = new Vector2(randomX, randomY).normalized;
        } while (Mathf.Abs(randomX) < 0.1f && Mathf.Abs(randomY) < 0.1f); // ����������l��h��

        moveDirection = new Vector2(randomX, randomY).normalized; // ���K�����Ĉ��̕����x�N�g�����쐬

        // �ǂɌ������Ă����ꍇ�͕����𔽓]
        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.deltaTime;
        if (!IsInsideBoundary(newPosition))
        {
            moveDirection = -moveDirection; // �ړ������𔽓]
        }

        // ���̃A�N�V�����܂ł̎��Ԃ�ݒ�
        actionTimer = Random.Range(minMoveTime, maxMoveTime);


        // �A�j���[�V�������ړ���Ԃɐݒ�
        animator.SetBool("Moving", true);

        ChangeState(ChickenState.Moving);
    }

    void StopMoving()
    {
        // �ړ����������Z�b�g
        moveDirection = Vector2.zero;

        // ���̃A�N�V�����܂ł̎��Ԃ�ݒ�
        actionTimer = Random.Range(minStopTime, maxStopTime);

        // �A�j���[�V�������~��Ԃɐݒ�
        animator.SetBool("Moving", false);

        ChangeState(ChickenState.Stopped);
    }

    bool IsInsideBoundary(Vector3 position)
    {
        if (boundaryCollider == null)
        {
            Debug.LogWarning("Boundary Collider ���ݒ肳��Ă��܂���I");
            return true; // Boundary���Ȃ��ꍇ�͔͈͓��Ƃ݂Ȃ�
        }

        return boundaryCollider.OverlapPoint(position);
    }

    void DropEgg()
    {
        if (eggPrefab != null)
        {
            Instantiate(eggPrefab, transform.position, Quaternion.identity);
        }
    }

    void ChangeState(ChickenState newState)
    {
        currentState = newState;
    }

    bool IsNearBoundary(Vector2 position, float margin = 0.2f)
    {
        if (boundaryCollider == null)
            return false;

        // ������Ɠ����ɂ��炵��4�������`�F�b�N
        Vector2[] checkOffsets = new Vector2[]
        {
        Vector2.left * margin,
        Vector2.right * margin,
        Vector2.up * margin,
        Vector2.down * margin
        };

        foreach (var offset in checkOffsets)
        {
            if (!boundaryCollider.OverlapPoint(position + offset))
            {
                return true; // �ǂꂩ���͈͊O�Ȃ�u�߂��v�Ƃ݂Ȃ�
            }
        }

        return false; // �S���͈͓��Ȃ�OK�i�܂�[�ł͂Ȃ��j
    }

    IEnumerator EggRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 8f));

            if (!IsNearBoundary(transform.position) && Random.value < eggDropChance)
            {
                DropEgg();
            }
        }
    }
    // �ǂƂ̏Փˌ��o
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true; // �ǂɐG��Ă���t���O�𗧂Ă�
            StopMoving(); // �ǂɐG�ꂽ���~
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false; // �ǂ��痣�ꂽ��t���O��������
            lastBlockedDirection = moveDirection; // ���̕����̓_��������
            StopMoving();
        }
    }
}
