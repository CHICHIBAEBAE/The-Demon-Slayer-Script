using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : InteractableObject
{
    CheckPointUI checkPointUI;
    public string checkPointName;
    public bool isDiscovered = false;
    public Room checkpointRoom;
    public GameObject checkpointIcon;

    MonsterObjectPool monsterObjectPool;
    RoomManager roomManager;

    private void Awake()
    {
        monsterObjectPool = GetComponentInParent<MonsterObjectPool>();
        checkpointRoom = GetComponentInParent<Room>();
        roomManager = GameManager.Instance.roomManager;
    }

    public override void Interact()
    {
        isDiscovered = true;

        UIManager.Instance.ToggleUI(ref checkPointUI, 1f, 1f, false, true);
        GameManager.Instance.TutorialCheckPoint = checkPointUI;
        SaveCheckPoint();
        GameManager.Instance.Player.healthSystem.ResetHealth();
    }

    public void SaveCheckPoint()
    {
        checkPointUI.currentCheckpoint = this;
        // ���� üũ����Ʈ �̸�
        checkPointUI.currentCheckpointName.text = checkPointName;
        // ���ͷ�Ʈ ���� �Ⱥ��̰�
        interactText.SetActive(false);
        // ������ üũ����Ʈ�� ������
        GameManager.Instance.roomManager.lastCheckPointRoomIdx = GameManager.Instance.roomManager.currentRoom.roomIdx;
        // ���� ĳ���Ͱ� �ִ� üũ����Ʈ�� ��ġ, �̸� ����
        GameManager.Instance.roomManager.checkPointPosition = player.transform.position;
        GameManager.Instance.roomManager.lastCheckPointName = checkPointName;
        // ������ ����
        DataManager.Instance.SaveData();
        //üũ����Ʈ ���� �� ���� Ȱ��ȭ 
        SpawnMonster();
        //monsterObjectPool.SaveCheckpointMonsterSpawn();
    }

    public void SpawnMonster()
    {
        //�������� ������Ʈ Ǯ�� �����Ƿ� -1 ������� or if������ ������Ʈ Ǯ null üũ�ϱ� 
        for (int i = 0; i < roomManager.rooms.Length; i++)
        {
            if (roomManager.rooms[i].monsterObjectPool != null)
            {
                roomManager.rooms[i].monsterObjectPool.SaveCheckpointMonsterSpawn();
            }
        }
    }
}
