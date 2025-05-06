using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Ysort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Core Settings")]
    [SerializeField] private float offset = 0f; // 描画順序を調整するオフセット値
    [SerializeField] private int scale = -10;  // Y座標に基づいて描画順序を調整するスケール値

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt((transform.position.y + offset) * scale);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), 0.1f);
    }
}

