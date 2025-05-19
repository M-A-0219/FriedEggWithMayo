using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("NPC Settings")]
    public List<NPC> npcPrefabs; // NPC�̃v���n�u���X�g
    public float npcSpacing = 0.75f; // NPC���m�̊Ԋu
    public float spawnIntervalMin = 3f; // NPC�o���Ԋu�i�ŏ��j
    public float spawnIntervalMax = 8f; // NPC�o���Ԋu�i�ő�j

    private List<NPC> npcQueue = new List<NPC>(); // NPC�̍s��
    private bool spawningNPCs = true; // NPC�𐶐����邩�ǂ���
    private bool isExiting = false; // �ޏꒆ��NPC�����邩�ǂ���

    // ���W�ݒ� (�Œ�l)
    private Vector2 spawnPosition = new Vector2(-3, 18); // �X�|�[���ʒu
    private Vector2 queueStartPosition = new Vector2(-3, 11.625f); // ��̊J�n�ʒu

    void Start()
    {
        // NPC�������[�v���J�n
        StartCoroutine(SpawnNPCs());
    }

    /// <summary>
    /// NPC���X�|�[�������郋�[�`��
    /// </summary>
    private IEnumerator SpawnNPCs()
    {
        while (spawningNPCs)
        {
            Debug.Log($"NPC�X�|�[���J�n�B���݂�NPC��: {npcQueue.Count}");
            float spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(spawnInterval);

            // NPC���X�|�[��
            SpawnNPC();
        }
    }

    /// <summary>
    /// NPC�𐶐����A�s��ɒǉ�
    /// </summary>
    private void SpawnNPC()
    {
        Debug.Log($"���݂�NPC��: {npcQueue.Count}");
        // NPC�̍s��10�l�ȏ�Ȃ琶�����Ȃ�
        if (npcQueue.Count >= 10)
        {
            Debug.Log("�s�񂪍ő�l���ɒB���Ă��邽�߁A�V����NPC�𐶐����܂���B");
            return;
        }
        if (npcPrefabs == null || npcPrefabs.Count == 0)
        {
            Debug.LogError("npcPrefabs ���ݒ肳��Ă��܂���I NPC�𐶐��ł��܂���B");
            return;
        }
        // NPC�̌����ڂ������_���ɐݒ�
        int randomIndex = Random.Range(0, npcPrefabs.Count);
        Debug.Log($"�����_���C���f�b�N�X: {randomIndex}, npcPrefabs.Count: {npcPrefabs.Count}");
        NPC npcInstance = Instantiate(npcPrefabs[randomIndex], spawnPosition, Quaternion.identity);


        // NPC���s��ɒǉ����ĊǗ�
        npcInstance.SetNPCManager(this);
        npcQueue.Add(npcInstance);

        // �s����X�V�i�ޏꒆ�łȂ��ꍇ�̂݁j
        if (!isExiting)
        {
            UpdateNPCQueue();
        }
        Debug.Log($"�V����NPC�𐶐����܂����I ���݂̍s��̐l��: {npcQueue.Count}");
        Debug.Log($"��������NPC�̖��O: {npcInstance.name}");
    }

    /// <summary>
    /// NPC�̍s����X�V���A�eNPC��K�؂Ȉʒu�ɕ��΂���
    /// </summary>
    public void UpdateNPCQueue()
    {
        for (int i = 0; i < npcQueue.Count; i++)
        {
            if (npcQueue[i].isExiting) continue; // �ޏꒆ��NPC�͖���

            // Vector2 �� Vector3 �ɕϊ�
            Vector3 targetPosition = (Vector3)queueStartPosition + new Vector3(0, npcSpacing * i, 0);
            npcQueue[i].SetTargetPosition(targetPosition, i == 0); // �擪��NPC�̂ݑҋ@��Ԃɐݒ�
        }
    }

    /// <summary>
    /// NPC���w�����I�����ۂɍs����X�V
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
