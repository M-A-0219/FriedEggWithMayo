using System.Collections.Generic;
using UnityEngine;

public class ChickenManager : MonoBehaviour
{
    public PolygonCollider2D boundaryCollider; // BoundaryのPolygonCollider2D
    public GameObject chickenPrefab; // チキンのプレファブ
    public int maxChickens = 10; // 同時に存在できる鶏の最大数
    public SpriteOrderManager spriteOrderManager;

    private List<GameObject> chickens = new List<GameObject>(); // 生成されたチキンのリスト

    public void SpawnChicken()
    {
        if (chickens.Count >= maxChickens)
        {
            Debug.LogWarning("チキンの最大数に達しました！");
            return;
        }

        Vector3 spawnPosition = GetRandomPositionInBoundary();
        GameObject newChicken = Instantiate(chickenPrefab, spawnPosition, Quaternion.identity);
        chickens.Add(newChicken);
        SpriteOrder spriteOrder = newChicken.GetComponent<SpriteOrder>();
        if (spriteOrder != null && spriteOrderManager != null)
        {
            spriteOrderManager.RegisterSprite(spriteOrder);
        }
        Debug.Log($"チキンを召喚しました！ 現在のチキン数: {chickens.Count}");
    }

    public void RemoveChicken(GameObject chicken)
    {
        if (chickens.Contains(chicken))
        {
            SpriteOrder spriteOrder = chicken.GetComponent<SpriteOrder>();
            if (spriteOrder != null && spriteOrderManager != null)
            {
                spriteOrderManager.UnregisterSprite(spriteOrder);
            }

            chickens.Remove(chicken);
        }
    }

    private Vector3 GetRandomPositionInBoundary()
    {
        if (boundaryCollider == null)
        {
            Debug.LogWarning("Boundary Collider が設定されていません！");
            return Vector3.zero;
        }

        Vector3 randomPosition = Vector3.zero;
        int safetyCounter = 0;

        do
        {
            Bounds bounds = boundaryCollider.bounds;
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            randomPosition = new Vector3(randomX, randomY, 0);

            safetyCounter++;
            if (safetyCounter > 100)
            {
                Debug.LogWarning("ランダム位置生成に失敗しました（安全カウンタ超過）");
                break;
            }
        } while (!boundaryCollider.OverlapPoint(randomPosition));

        return randomPosition;
    }
}
