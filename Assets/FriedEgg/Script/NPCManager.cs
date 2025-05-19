using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("NPC Settings")]
    public List<NPC> npcPrefabs; // NPCのプレハブリスト
    public float npcSpacing = 0.75f; // NPC同士の間隔
    public float spawnIntervalMin = 3f; // NPC出現間隔（最小）
    public float spawnIntervalMax = 8f; // NPC出現間隔（最大）

    private List<NPC> npcQueue = new List<NPC>(); // NPCの行列
    private bool spawningNPCs = true; // NPCを生成するかどうか
    private bool isExiting = false; // 退場中のNPCがいるかどうか

    // 座標設定 (固定値)
    private Vector2 spawnPosition = new Vector2(-3, 18); // スポーン位置
    private Vector2 queueStartPosition = new Vector2(-3, 11.625f); // 列の開始位置

    void Start()
    {
        // NPC生成ループを開始
        StartCoroutine(SpawnNPCs());
    }

    /// <summary>
    /// NPCをスポーンさせるルーチン
    /// </summary>
    private IEnumerator SpawnNPCs()
    {
        while (spawningNPCs)
        {
            Debug.Log($"NPCスポーン開始。現在のNPC数: {npcQueue.Count}");
            float spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(spawnInterval);

            // NPCをスポーン
            SpawnNPC();
        }
    }

    /// <summary>
    /// NPCを生成し、行列に追加
    /// </summary>
    private void SpawnNPC()
    {
        Debug.Log($"現在のNPC数: {npcQueue.Count}");
        // NPCの行列が10人以上なら生成しない
        if (npcQueue.Count >= 10)
        {
            Debug.Log("行列が最大人数に達しているため、新しいNPCを生成しません。");
            return;
        }
        if (npcPrefabs == null || npcPrefabs.Count == 0)
        {
            Debug.LogError("npcPrefabs が設定されていません！ NPCを生成できません。");
            return;
        }
        // NPCの見た目をランダムに設定
        int randomIndex = Random.Range(0, npcPrefabs.Count);
        Debug.Log($"ランダムインデックス: {randomIndex}, npcPrefabs.Count: {npcPrefabs.Count}");
        NPC npcInstance = Instantiate(npcPrefabs[randomIndex], spawnPosition, Quaternion.identity);


        // NPCを行列に追加して管理
        npcInstance.SetNPCManager(this);
        npcQueue.Add(npcInstance);

        // 行列を更新（退場中でない場合のみ）
        if (!isExiting)
        {
            UpdateNPCQueue();
        }
        Debug.Log($"新しいNPCを生成しました！ 現在の行列の人数: {npcQueue.Count}");
        Debug.Log($"生成したNPCの名前: {npcInstance.name}");
    }

    /// <summary>
    /// NPCの行列を更新し、各NPCを適切な位置に並ばせる
    /// </summary>
    public void UpdateNPCQueue()
    {
        for (int i = 0; i < npcQueue.Count; i++)
        {
            if (npcQueue[i].isExiting) continue; // 退場中のNPCは無視

            // Vector2 を Vector3 に変換
            Vector3 targetPosition = (Vector3)queueStartPosition + new Vector3(0, npcSpacing * i, 0);
            npcQueue[i].SetTargetPosition(targetPosition, i == 0); // 先頭のNPCのみ待機状態に設定
        }
    }

    /// <summary>
    /// NPCが購入を終えた際に行列を更新
    /// </summary>
    public void OnNPCFinished(NPC npc)
    {
        if (npcQueue.Contains(npc))
        {
            npcQueue.Remove(npc);
        }

        UpdateNPCQueue();
    }
}
