using System.Collections.Generic;
using UnityEngine;

public class ChickenManager : MonoBehaviour
{
    public PolygonCollider2D boundaryCollider; // Boundary��PolygonCollider2D
    public GameObject chickenPrefab; // �`�L���̃v���t�@�u
    public int maxChickens = 10; // �����ɑ��݂ł���{�̍ő吔
    public SpriteOrderManager spriteOrderManager;

    private List<GameObject> chickens = new List<GameObject>(); // �������ꂽ�`�L���̃��X�g

    public void SpawnChicken()
    {
        if (chickens.Count >= maxChickens)
        {
            Debug.LogWarning("�`�L���̍ő吔�ɒB���܂����I");
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
        Debug.Log($"�`�L�����������܂����I ���݂̃`�L����: {chickens.Count}");
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
            Debug.LogWarning("Boundary Collider ���ݒ肳��Ă��܂���I");
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
                Debug.LogWarning("�����_���ʒu�����Ɏ��s���܂����i���S�J�E���^���߁j");
                break;
            }
        } while (!boundaryCollider.OverlapPoint(randomPosition));

        return randomPosition;
    }
}
