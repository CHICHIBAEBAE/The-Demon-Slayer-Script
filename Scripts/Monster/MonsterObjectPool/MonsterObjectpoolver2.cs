using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectPoolver2 : MonoBehaviour
{
    public Transform[] spawnPoints; // ���� ��ġ �迭
    public GameObject[] monsterPrefabs; // ���� ������ �迭

    public float spawnInterval = 3f; // ���� ���� ����
    public float initialSpawnDelay = 2f; // �ʱ� ���� ���� �ð�

    private List<GameObject> monsterPool; // ���� ������Ʈ Ǯ
    private List<int> availableSpawnPoints; // ���� ������ ��ġ �ε��� ����Ʈ

    private Coroutine spawnCoroutine;

    private void OnEnable()
    {
        // ������Ʈ�� Ȱ��ȭ�� �� �ʱ�ȭ �� �ڷ�ƾ ����
        Initialize();
        Invoke("StartSpawning", initialSpawnDelay);
    }

    private void OnDisable()
    {
        // ������Ʈ�� ��Ȱ��ȭ�� �� �ڷ�ƾ ����
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        // ��� Ȱ��ȭ�� ���� ��Ȱ��ȭ
        foreach (GameObject monster in monsterPool)
        {
            if (monster.activeSelf)
            {
                monster.SetActive(false);
            }
        }
    }

    private void Initialize()
    {
        // ���� ������Ʈ Ǯ �ʱ�ȭ
        if (monsterPool == null)
        {
            monsterPool = new List<GameObject>();
        }
        else
        {
            monsterPool.Clear();
        }

        // ���͸� �ʱ⿡ ���� ����
        SpawnAllMonsters();

        // ���� ������ ��ġ �ε��� ����Ʈ �ʱ�ȭ
        if (availableSpawnPoints == null)
        {
            availableSpawnPoints = new List<int>();
        }
        else
        {
            availableSpawnPoints.Clear();
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableSpawnPoints.Add(i);
        }
    }

    private void StartSpawning()
    {
        // �ڷ�ƾ ����
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnMonsters());
        }
    }

    private void SpawnAllMonsters()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // ���͸� ������Ʈ Ǯ���� �������ų� ����
            GameObject monster = GetOrCreateMonsterFromPool();

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
        bool hasMonster = false;

        // �ش� ���� ��ġ�� ���Ͱ� �ִ��� �˻�
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
            foreach (GameObject monsterPrefab in monsterPrefabs)
            {
                GameObject newMonster = Instantiate(monsterPrefab);
                newMonster.SetActive(false);
                monsterPool.Add(newMonster);
            }
            monster = monsterPool.Find(m => !m.activeSelf);
        }
        return monster;
    }
}
