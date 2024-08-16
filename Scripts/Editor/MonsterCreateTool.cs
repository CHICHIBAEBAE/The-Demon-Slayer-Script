using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MonsterCreateTool : EditorWindow
{
    private string monsterName;
    public float[] attackDetectRange; //���� ��������
    public float[] attackDistance; //���� ����
    private float detectRange;
    private MonsterClassType monsterClassType;
    private GameObject monsterPrefab;

    private int hp;
    private float speed;
    private float damage;

    [MenuItem("MyTool/Monster Stats Creator")]
    public static void ShowWindow()
    {
        GetWindow<MonsterCreateTool>("Monster Stats Creator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("���� ���� ��", EditorStyles.boldLabel);

        // StatsSO �ʵ��
        hp = EditorGUILayout.IntField("HP", hp);
        speed = EditorGUILayout.FloatField("Speed", speed);
        damage = EditorGUILayout.FloatField("damage", damage);

        // MonsterStatsSO �ʵ��
        monsterName = EditorGUILayout.TextField("Monster Name", monsterName);

        for (int i = 0; i < attackDetectRange.Length; i++)
        {
            attackDetectRange[i] = EditorGUILayout.FloatField($"Attack Range {i + 1}", attackDetectRange[i]);
        }

        detectRange = EditorGUILayout.FloatField("detectRange", detectRange);
        monsterClassType = (MonsterClassType)EditorGUILayout.EnumPopup("Monster Type", monsterClassType);
        monsterPrefab = (GameObject)EditorGUILayout.ObjectField("Monster Prefab", monsterPrefab, typeof(GameObject), false);

        // Create ��ư
        if (GUILayout.Button("Create Monster Stats"))
        {
            CreateMonsterStats();
        }
    }

    private void CreateMonsterStats()
    {
        // ���ο� StatsSO ��ü ����
        StatsSO newStats = ScriptableObject.CreateInstance<StatsSO>();
        newStats.hp = hp;
        newStats.speed = speed;
        newStats.damage = damage;

        // ���ο� MonsterStatsSO ��ü ����
        MonsterStatsSO newMonsterStats = ScriptableObject.CreateInstance<MonsterStatsSO>();
        newMonsterStats.monsterName = monsterName;
        newMonsterStats.attackDetectRange = attackDetectRange;
        newMonsterStats.detectRange = detectRange;
        newMonsterStats.monsterClassType = monsterClassType;
        newMonsterStats.monsterPrefab = monsterPrefab;

        // StatsSO �ʵ� ����
        newMonsterStats.hp = hp;
        newMonsterStats.speed = speed;
        newMonsterStats.damage = damage;

        // MonsterStatsSO ��ü�� ������Ʈ�� ����
        string monsterStatsPath = "Assets/Data/Stats/Monster";
        if (!AssetDatabase.IsValidFolder(monsterStatsPath))
        {
            AssetDatabase.CreateFolder("Assets/Data/Stats", "Monster");
        }

        string monsterStatsAssetPath = $"{monsterStatsPath}/{monsterName}SO.asset";
        AssetDatabase.CreateAsset(newMonsterStats, monsterStatsAssetPath);

        // ���� ������ ����
        CreateMonsterPrefab(newMonsterStats);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // ���� �� �ʵ� �ʱ�ȭ
        monsterName = "";
        attackDetectRange = new float[3];
        detectRange = 0;
        monsterClassType = MonsterClassType.Basic;
        monsterPrefab = null;
        hp = 0;
        speed = 0;
        damage = 0;

        EditorUtility.DisplayDialog("���� ����", "���� ���� ��ũ��Ʈ�� �ִϸ��̼��� �����ؾ��մϴ�.", "OK");
    }

    private void CreateMonsterPrefab(MonsterStatsSO stats)
    {
        if (stats.monsterPrefab == null)
        {
            EditorUtility.DisplayDialog("����", "�������� �־��ּ���.", "OK");
            return;
        }

        // ���ο� GameObject ���� �� ���� ����
        GameObject newMonster = Instantiate(stats.monsterPrefab);
        newMonster.name = stats.monsterName;

        // �ʿ��� ������Ʈ�� �߰��ϰ� ������ ����
        Monster monsterComponent = newMonster.GetComponent<Monster>();
        StatHandler monsterStatHandler = newMonster.GetComponent<StatHandler>();
        if (monsterComponent == null)
        {
            monsterComponent = newMonster.AddComponent<Monster>();
        }
        monsterComponent.stats = stats; // ������ ����
        monsterStatHandler.baseStat.statsSO = stats;


        // ������ GameObject�� ���������� ����
        string path = "Assets/Prefabs/Monster";
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/Prefabs", "Monster");
        }

        string prefabPath = $"{path}/{stats.monsterName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(newMonster, prefabPath);
        DestroyImmediate(newMonster);
    }
}
