using Codice.Client.BaseCommands.Merge.Xml;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
public enum CreateItemType
{
    Equip,
    Passive
}

public enum CreatePlayerActive
{
    None,
    DoubleJump,
    Dash
}
public class ItemCreateTool : EditorWindow
{
    public Sprite itemImage;
    public string itemName;        
    public string description; 
    public int requireCurrency;
    private GameObject ItemPrefab;
    public CreateItemType createitemType;
    public CreatePlayerActive createplayerActive;

    private int hp;
    private float speed;
    private float damage;

    [MenuItem("MyTool/Item Creator")]
    public static void ShowWindow()
    {
        GetWindow<ItemCreateTool>("Item Creator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("������ ���� �� ", EditorStyles.boldLabel);

        // StatsSO �ʵ��
        hp = EditorGUILayout.IntField("HP", hp);
        speed = EditorGUILayout.FloatField("Speed", speed);
        damage = EditorGUILayout.FloatField("damage", damage);

        // ItemSO �ʵ��
        itemImage = (Sprite)EditorGUILayout.ObjectField("itemImage", itemImage, typeof(Sprite), false);
        itemName = EditorGUILayout.TextField("Item Name", itemName);
        description = EditorGUILayout.TextField("description", description);
        ItemPrefab = (GameObject)EditorGUILayout.ObjectField("Item Prefab", ItemPrefab, typeof(GameObject), false);
        createitemType = (CreateItemType)EditorGUILayout.EnumPopup("item Type", createitemType);
        createplayerActive = (CreatePlayerActive)EditorGUILayout.EnumPopup("Player Active", createplayerActive);

        // Create ��ư
        if (GUILayout.Button("Create Item Stats"))
        {
            CreateItemStats();
        }
    }

    private void CreateItemStats()
    {
        // ���ο� StatsSO ��ü ����
        StatsSO newStats = ScriptableObject.CreateInstance<StatsSO>();
        newStats.hp = hp;
        newStats.speed = speed;
        newStats.damage = damage;

        // ���ο� ItemStatsSO ��ü ����
        ItemSO newItemStat = ScriptableObject.CreateInstance<ItemSO>();
        newItemStat.itemImage = itemImage;
        newItemStat.itemName = itemName;
        newItemStat.description = description;
        newItemStat.ItemPrefab = ItemPrefab;
        newItemStat.itemType = (ItemType)createitemType;
        newItemStat.playerActive = (PlayerActive)createplayerActive;


        // StatsSO �ʵ� ����
        newItemStat.hp = hp;
        newItemStat.speed = speed;
        newItemStat.damage = damage;

        // ItemStatsSO ��ü�� ������Ʈ�� ����
        string monsterStatsPath = "Assets/Data/Stats/Item";
        if (!AssetDatabase.IsValidFolder(monsterStatsPath))
        {
            AssetDatabase.CreateFolder("Assets/Data/Item", "Item");
        }

        string monsterStatsAssetPath = $"{monsterStatsPath}/{itemName}SO.asset";
        AssetDatabase.CreateAsset(newItemStat, monsterStatsAssetPath);

        // ������ ������ ����
        CreateItemPrefab(newItemStat);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // ���� �� �ʵ� �ʱ�ȭ
        itemImage = null;
        itemName = "";
        description = "";
        ItemPrefab = null;
        hp = 0;
        speed = 0;
        damage = 0;

        EditorUtility.DisplayDialog("������ ����", "�������� �����Ǿ����ϴ�.", "OK");
    }

    private void CreateItemPrefab(ItemSO stats)
    {
        if (stats.ItemPrefab == null)
        {
            EditorUtility.DisplayDialog("����", "�������� �־��ּ���.", "OK");
            return;
        }

        // ���ο� GameObject ���� �� ���� ����
        GameObject newItem = Instantiate(stats.ItemPrefab);
        newItem.name = stats.itemName;

        // �ʿ��� ������Ʈ�� �߰��ϰ� ������ ����
        RelicsItem ItemComponent = newItem.GetComponent<RelicsItem>();
        if (ItemComponent == null)
        {
            ItemComponent = newItem.AddComponent<RelicsItem>();
        }
        ItemComponent.itemSO = stats; // ������ ����


        // ������ GameObject�� ���������� ����
        string path = "Assets/Prefabs/Item";
        if (!AssetDatabase.IsValidFolder(path))
        {
            AssetDatabase.CreateFolder("Assets/Item", "Item");
        }

        string prefabPath = $"{path}/{stats.itemName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(newItem, prefabPath);
        DestroyImmediate(newItem);
    }
}
