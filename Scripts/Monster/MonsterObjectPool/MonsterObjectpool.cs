using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectPool : MonoBehaviour
{
    public Transform[] spawnPoints; // ���� ��ġ �迭
    public GameObject[] monsterPrefabs; // ���� ������ �迭

    public float spawnInterval = 3f; // ���� ���� ����
    public float initialSpawnDelay = 2f; // �ʱ� ���� ���� �ð�

    private List<GameObject> monsterPool; // ���� ������Ʈ Ǯ
    private List<int> availableSpawnPoints; // ���� ������ ��ġ �ε��� ����Ʈ

    
    private void Start()
    {
        // ���� ������Ʈ Ǯ �ʱ�ȭ
        monsterPool = new List<GameObject>();
        // ���͸� �ʱ⿡ ���� ����
        SpawnAllMonsters();
        // ���� ������ ��ġ �ε��� ����Ʈ �ʱ�ȭ
        availableSpawnPoints = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableSpawnPoints.Add(i);
        }
        Invoke("StartSpawning", initialSpawnDelay);
    }


    private void StartSpawning()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(SpawnMonsters());
        }
    }
    private void SpawnAllMonsters()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // ���͸� ������Ʈ Ǯ���� �������ų� ����
            GameObject monster = GetOrCreateMonsterFromPool();

            MonsterController monsterController = monster.GetComponent<MonsterController>();

            monsterController.healthSystem.ResetHealth();
            // ���͸� ���� ��ġ�� ����
            monster.transform.position = spawnPoint.position;
            monster.SetActive(true);
        }
    }
    private IEnumerator SpawnMonsters()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // ���� ������ ��ġ�� �ִ��� Ȯ��
            if (availableSpawnPoints.Count == 0)
                continue;

            // ���� ��ġ �ε����� �����ϰ� ����
            int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
            int spawnPointIndex = availableSpawnPoints[spawnIndex];

            // �ش� ���� ��ġ�� ���Ͱ� �̹� �����Ǿ� �ִ��� Ȯ��
            if (IsMonsterSpawnedAtPoint(spawnPointIndex))
                continue;

            // ���͸� ������Ʈ Ǯ���� �������ų� ����
            GameObject monster = GetOrCreateMonsterFromPool();

            // ���͸� ���õ� ���� ��ġ�� ����
            monster.transform.position = spawnPoints[spawnPointIndex].position;
            monster.SetActive(true);

            // ������ ��ġ �ε����� ���� ������ ��ġ �ε��� ����Ʈ���� ����
            availableSpawnPoints.RemoveAt(spawnIndex);
        }
    }
    private bool IsMonsterSpawnedAtPoint(int spawnPointIndex)
    {
        // �ش� ���� ��ġ�� ���Ͱ� �ִ��� Ȯ���ϴ� ������ ����
        // �̹� ���Ͱ� �ִٸ� false�� ��ȯ�ϰ�, ���ٸ� true�� ��ȯ�Ͽ� ������ ����

        bool hasMonster = false;

        // �ش� ���� ��ġ�� ���Ͱ� �ִ��� �˻�
        // ���ÿ����� monsterPool ����Ʈ�� ����Ͽ� �ش� ��ġ�� ���� ������ �Ǵ�
        GameObject monster = monsterPool.Find(m => m != null && m.activeSelf && m.transform.position == spawnPoints[spawnPointIndex].position);
        if (monster == null)
        {
            hasMonster = true;
        }

        return hasMonster;
    }


    // ���͸� ������Ʈ Ǯ���� �������ų� ����
    private GameObject GetOrCreateMonsterFromPool()
    {
        GameObject monster = monsterPool.Find(m => !m.activeSelf);
        if (monster == null)
        {
            // ��� ���� �������� ��ȸ�ϸ� ������Ʈ Ǯ�� �߰�
            foreach (GameObject monsterPrefab in monsterPrefabs)
            {
                GameObject newMonster = Instantiate(monsterPrefab, transform);
                newMonster.SetActive(false);
                monsterPool.Add(newMonster);
            }
            // �ٽ� ���͸� ������
            monster = monsterPool.Find(m => !m.activeSelf);
        }
        return monster;
    }

    public void SaveCheckpoint()
    {
        DeactivateAllMonsters();
        SpawnAllMonsters();
    }

    //private void ClearObjectPool()
    //{
    //    foreach (GameObject monster in monsterPool)
    //    {
    //        Destroy(monster); 
    //    }
    //    monsterPool.Clear(); 
    //    availableSpawnPoints.Clear(); 
    //}

    private void DeactivateAllMonsters()
    {
        foreach (GameObject monster in monsterPool)
        {
            if (monster.activeSelf)
            {
                monster.SetActive(false);
            }
        }
    }
}

