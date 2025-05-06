using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; 
    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private float inputX, inputY;
    private Vector2 lastDirection = Vector2.down;
    private PlayerController playerController;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(inputX, inputY).normalized;
        playerRigidbody.velocity = input * speed;

        if (input != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
            lastDirection = input;
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        animator.SetFloat("InputX", lastDirection.x);
        animator.SetFloat("InputY", lastDirection.y);

        // �v���C���[�̌�����PlayerController�ɒʒm
        playerController.SetLastDirection(lastDirection);
    }

    /// <summary>
    /// ���x�𑝉������郁�\�b�h
    /// </summary>
    public void IncreaseSpeed(float amount)
    {
        speed += amount;
        Debug.Log($"�v���C���[�̑��x���������܂���: {speed}");
    }
}
